using DACS.Models;
using DACS.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Linq;

namespace DACS.Controllers
{
    public class CVController : Controller 
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICvRepository _cvRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CVController(ApplicationDbContext context, UserManager<User> userManager, IUserRepository userRepository, ICvRepository cvRepository, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _userRepository = userRepository;
            _cvRepository = cvRepository;
            _webHostEnvironment = webHostEnvironment;
        }       

        [Authorize]
        public async Task<IActionResult> ViewCV(int id)
        {
            var cv = await _context.CVs.FindAsync(id);
            if (cv == null || cv.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized(); // Không cho phép nếu không đúng người dùng
            }

            // Đường dẫn file tuyệt đối trên server
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, cv.FileCV.TrimStart('/'));

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf");
        }

        public async Task<IActionResult> Index(string id)
        {
            var cv = await _context.CVs.Include(p => p.Users).Where(p => p.UserId == id).ToListAsync();
            return View(cv);
        }

        public IActionResult JobRecommendations(List<PhieuDangTuyen> goodjob)
        {
            return View(goodjob);
        }

        private List<string> GetIndustriesFromDatabase()
        {
            // Lấy danh sách ngành từ CSDL
            return _context.PhieuDangTuyens.Select(i => i.TenNganh).Distinct().ToList();
        }

        private List<string> GetJobTitlesFromDatabase()
        {
            // Lấy danh sách tên công việc từ CSDL
            return _context.PhieuDangTuyens.Select(j => j.TenViecLam).Distinct().ToList();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Create([Bind("FileCV")] CV cv, IFormFile fileCV, string userChoice)
		{
			ModelState.Remove("fileCV");
			if (ModelState.IsValid)
			{
				var currentUser = await _userManager.GetUserAsync(User);
				cv.UserId = currentUser.Id;

				if (fileCV != null && fileCV.Length > 0)
				{
					if (Path.GetExtension(fileCV.FileName).ToLower() != ".pdf")
					{
						ModelState.AddModelError("fileCV", "Vui lòng chỉ tải lên file PDF.");
						return View(cv);
					}

					// Lưu file PDF
					var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "CVs", currentUser.Id);
					if (!Directory.Exists(uploadsFolder))
					{
						Directory.CreateDirectory(uploadsFolder);
					}
					var fileName = Path.GetFileName(fileCV.FileName);
					var filePath = Path.Combine(uploadsFolder, fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await fileCV.CopyToAsync(stream);
					}

					cv.FileCV = Path.Combine("/CVs", currentUser.Id, fileName);
					cv.TenCV = fileName;

					// Xử lý theo lựa chọn của người dùng
					if (userChoice == "Yes")
					{
						var reader = new CVReader();
						var cvText = reader.ExtractTextFromPDF(filePath);

						// Tìm kiếm công việc phù hợp
						var jobList = await _context.PhieuDangTuyens.ToListAsync();
						var jobMatcher = new JobMatcher();
						var matchedJobs = jobMatcher.FindMatchingJobs(cvText, jobList);

                        // Lưu CV vào data
                        await _cvRepository.AddAsync(cv);

                        // Hiển thị danh sách công việc phù hợp
                        return View("JobRecommendations", matchedJobs);
					}
					if (userChoice == "No")
					{
						await _cvRepository.AddAsync(cv);
						return RedirectToAction("Index", new { id = currentUser.Id });
					}

					return RedirectToAction("Index", new { id = currentUser.Id });
				}
			}
			return View(cv);
		}

		public async Task<string> SaveFile(IFormFile pdfFile)
        {
            // Lấy tên file từ file tải lên
            var fileName = Path.GetFileName(pdfFile.FileName);

            // Tạo đường dẫn lưu file trong thư mục wwwroot/pdfs
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CVs/", fileName);

            // Lưu file vào thư mục wwwroot/pdfs
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn tương đối của file
            return "/CVs/" + fileName;
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cv = await _context.CVs.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == id);
            if (cv == null)
            {
                return NotFound();
            }

            return View(cv);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            await _cvRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { id = currentUser.Id });
        }

    }

    public class CVReader
    {
        public string ExtractTextFromPDF(string filePath)
        {
            using (PdfReader pdfReader = new PdfReader(filePath))
            using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
            {
                string text = string.Empty;
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    text += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                }
                return text;
            }
        }
    }

    public class JobMatcher
    {
        public List<PhieuDangTuyen> FindMatchingJobs(string cvText, List<PhieuDangTuyen> jobList)
        {
            // Tách từ khóa ngành và tên công việc từ CV
            var industries = ExtractIndustriesFromCV(cvText);
            var jobTitles = ExtractJobTitlesFromCV(cvText);

            // Lọc danh sách công việc dựa trên ngành và từ khóa
            return jobList.Where(job =>
                industries.Any(industry => job.TenNganh.Contains(industry, StringComparison.OrdinalIgnoreCase)) ||
                jobTitles.Any(title => job.TenViecLam.Contains(title, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        private List<string> ExtractIndustriesFromCV(string cvText)
        {
            // Danh sách ngành nghề mẫu (hoặc lấy từ database)
            var predefinedIndustries = new List<string>
        {
            "Công nghệ thông tin", "Tài chính", "Giáo dục", "Y tế", "Marketing"
        };

            return predefinedIndustries
                .Where(industry => cvText.Contains(industry, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private List<string> ExtractJobTitlesFromCV(string cvText)
        {
            // Danh sách tên công việc mẫu (hoặc lấy từ database)
            var predefinedJobTitles = new List<string>
            {
                "Lập trình viên", "Quản lý dự án IT", "Nhân viên kinh doanh", "Kỹ sư phần mềm", "Chuyên viên an ninh mạng",
				"Kỹ sư hệ thống", "Quản lý tài chính", "Chuyên viên đầu tư", "Kế toán trưởng", "Chuyên viên marketing",
				"Trưởng phòng marketing", "Quản lý truyền thông", "Người sáng tạo nội dung", "Chuyên viên SEO",
				"Kỹ sư xây dựng", "Giám sát công trình", "Trưởng phòng thiết kế", "Chuyên viên quản lý dự án", "Kỹ sư kết cấu",
				"Kỹ sư cơ khí", "Kỹ sư điện tử", "Quản lý sản xuất", "Kỹ sư tự động hóa", "Kỹ thuật viên sửa chữa"
		    };

            return predefinedJobTitles
                .Where(title => cvText.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}


//[HttpPost]
//public async Task<IActionResult> Create([Bind("FileCV")] CV cv, IFormFile fileCV)
//{
//    ModelState.Remove("fileCV");
//    if (ModelState.IsValid)
//    {
//        var currentUser = await _userManager.GetUserAsync(User);
//        cv.UserId = currentUser.Id;

//        if (fileCV != null && fileCV.Length > 0)
//        {
//            // Kiểm tra loại file
//            if (Path.GetExtension(fileCV.FileName).ToLower() != ".pdf")
//            {
//                ModelState.AddModelError("fileCV", "Vui lòng chỉ tải lên file PDF.");
//                return View(cv);
//            }

//            // Lưu file PDF
//            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "CVs", currentUser.Id);
//            if (!Directory.Exists(uploadsFolder))
//            {
//                Directory.CreateDirectory(uploadsFolder);
//            }
//            var fileName = Path.GetFileName(fileCV.FileName);
//            var filePath = Path.Combine(uploadsFolder, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await fileCV.CopyToAsync(stream);
//            }

//            cv.FileCV = Path.Combine("/CVs", currentUser.Id, fileName);
//            cv.TenCV = fileName;

//            // Đọc nội dung từ file PDF
//            var reader = new CVReader();
//            var cvText = reader.ExtractTextFromPDF(filePath);

//            // Tìm kiếm công việc phù hợp
//            var jobList = await _context.PhieuDangTuyens.ToListAsync();
//            var jobMatcher = new JobMatcher();
//            var matchedJobs = jobMatcher.FindMatchingJobs(cvText, jobList);

//            // Lưu CV vào data
//            await _cvRepository.AddAsync(cv);

//            // Hiển thị danh sách công việc phù hợp
//            return View("JobRecommendations", matchedJobs);
//        }

//        return RedirectToAction("Index", new { id = currentUser.Id });
//    }
//    return View(cv);
//}