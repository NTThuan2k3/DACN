using DACS.Models;
using DACS.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index(string id)
        {
            var cv = await _context.CVs.Include(p => p.Users).Where(p => p.UserId == id).ToListAsync();
            return View(cv);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FileCV")] CV cv, IFormFile fileCV)
        {
            ModelState.Remove("fileCV");
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                cv.UserId = currentUser.Id;

                if (fileCV != null && fileCV.Length > 0)
                {
                    // Kiểm tra loại file
                    if (Path.GetExtension(fileCV.FileName).ToLower() != ".pdf")
                    {
                        ModelState.AddModelError("fileCV", "Vui lòng chỉ tải lên file PDF.");
                        return View(cv);
                    }

                    // Tạo thư mục người dùng nếu chưa tồn tại
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "CVs", currentUser.Id);
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Lưu file nếu là file PDF
                    var fileName = Path.GetFileName(fileCV.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileCV.CopyToAsync(stream);
                    }
                    cv.FileCV = Path.Combine("/CVs", currentUser.Id, fileName); // Đường dẫn tương đối
                    cv.TenCV = fileName;
                }

                await _cvRepository.AddAsync(cv);
                return RedirectToAction("Index", new { id = currentUser.Id });
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
}



