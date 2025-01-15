using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DACS.Controllers
{
    public class DonUngTuyenController : Controller
    {
        private readonly IDonUngTuyenRepository _donUngTuyenRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DonUngTuyenController(IDonUngTuyenRepository donUngTuyenRepository, UserManager<User> userManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IUserRepository userRepository)
        {
            _donUngTuyenRepository = donUngTuyenRepository;
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userRepository = userRepository;
        }

        [Authorize]
        public async Task<IActionResult> ViewCV(int id)
        {
            var don = await _context.DonUngTuyens.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == id);

            if (don == null || don.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized(); // Chặn truy cập nếu không phải chủ sở hữu CV
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, don.FileCV.TrimStart('/'));
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf");
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != null)
            {
                var don = await _context.DonUngTuyens.Include(p => p.PhieuDangTuyens).Include(p => p.Users).Where(u => u.UserId == currentUser.Id && u.StatusId == 1).ToListAsync();
                return View(don);
            }
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var don = await _donUngTuyenRepository.GetByIdAsync(id);
            if (don == null)
            {
                return NotFound();
            }

            return View(don);
        }

        public async Task<IActionResult> Create()
        {
            string username = "Tên người dùng của bạn";
            ViewBag.Username = username;
			return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FileCV")] DonUngTuyen don, int id, /*IFormFile fileCV,*/ string selectedCV)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                don.UserId = currentUser.Id;
                don.PhieuDangTuyenId = id;
                don.StatusId = 1;
                don.XetDuyet = "Waiting";

				if (!string.IsNullOrEmpty(selectedCV))
                {
                    // Nếu người dùng chọn CV từ danh sách
                    var cvs = await _context.CVs.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == int.Parse(selectedCV));
                    don.TenCV = cvs.TenCV;
					don.FileCV = cvs.FileCV;
				}

                await _donUngTuyenRepository.AddAsync(don);
                return RedirectToAction("Index");
            }
            return View(don);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([Bind("OtherProperties", "FileCV")] DonUngTuyen don, int id, IFormFile fileCV, string selectedCV)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var currentUser = await _userManager.GetUserAsync(User);
        //        don.UserId = currentUser.Id;
        //        don.PhieuDangTuyenId = id;
        //        don.StatusId = 1;

        //        if (!string.IsNullOrEmpty(selectedCV))
        //        {
        //            // Nếu người dùng chọn CV từ danh sách
        //            don.FileCV = selectedCV;
        //        }
        //        else if (fileCV != null && fileCV.Length > 0)
        //        {
        //            // Kiểm tra loại file
        //            if (Path.GetExtension(fileCV.FileName).ToLower() != ".pdf")
        //            {
        //                ModelState.AddModelError("fileCV", "Vui lòng chỉ tải lên file PDF.");
        //                return View(don);
        //            }

        //            // Lưu file nếu là file PDF
        //            var fileName = Path.GetFileName(fileCV.FileName);
        //            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadFile");
        //            var filePath = Path.Combine(uploadsFolder, fileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await fileCV.CopyToAsync(stream);
        //            }
        //            don.FileCV = Path.Combine("/UploadFile", fileName); // Đường dẫn tương đối
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("fileCV", "Vui lòng tải lên file hoặc chọn một CV.");
        //            return View(don);
        //        }

        //        await _donUngTuyenRepository.AddAsync(don);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(don);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([Bind("OtherProperties", "FileCV")] DonUngTuyen don, int id, IFormFile fileCV)
        //{
        //    //ModelState.Remove("fileCV");
        //    if (ModelState.IsValid)
        //    {
        //        var currentUser = await _userManager.GetUserAsync(User);
        //        don.UserId = currentUser.Id;
        //        don.PhieuDangTuyenId = id;
        //        don.StatusId = 1;
        //        if (fileCV != null && fileCV.Length > 0)
        //        {
        //            //Kiểm tra loại file
        //            if (Path.GetExtension(fileCV.FileName).ToLower() != ".pdf")
        //            {
        //                ModelState.AddModelError("fileCV", "Vui lòng chỉ tải lên file PDF.");
        //                return View(don);
        //            }

        //            // Lưu file nếu là file PDF
        //            var fileName = Path.GetFileName(fileCV.FileName);
        //            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadFile");
        //            var filePath = Path.Combine(uploadsFolder, fileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await fileCV.CopyToAsync(stream);
        //            }
        //            don.FileCV = Path.Combine("/UploadFile", fileName); // Đường dẫn tương đối
        //        }
        //        await _donUngTuyenRepository.AddAsync(don);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(don);
        //}


        public async Task<IActionResult> Edit(int id)
        {
            var don = await _donUngTuyenRepository.GetByIdAsync(id);
            if (don == null)
            {
                return NotFound();
            }
            return View(don);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DonUngTuyen don, IFormFile fileCV)
        {
            if (id != don.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (fileCV != null && fileCV.Length > 0)
                {
                    // Kiểm tra loại file
                    if (Path.GetExtension(fileCV.FileName).ToLower() != ".pdf")
                    {
                        ModelState.AddModelError("fileCV", "Vui lòng chỉ tải lên file PDF.");
                        return View(don);
                    }

                    // Lưu file nếu là file PDF 
                    var fileName = Path.GetFileName(fileCV.FileName);
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "UploadFile");
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileCV.CopyToAsync(stream);
                    }
                    don.FileCV = Path.Combine("/UploadFile", fileName); // Đường dẫn tương đối
                }
                don.StatusId = 1;
                await _donUngTuyenRepository.UpdateAsync(don);
                return RedirectToAction(nameof(Index));
            }
            return View(don);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var don = await _donUngTuyenRepository.GetByIdAsync(id);
            if (don == null)
            {
                return NotFound();
            }

            return View(don);
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _donUngTuyenRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DownloadCV(string fileName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadFile", fileName);
            return PhysicalFile(filePath, "application/pdf", fileName); // Đảm bảo fileName có phần mở rộng phù hợp với loại file (ví dụ: .pdf)
        }

        public async Task<IActionResult> DetailsPhieu(int id)
        {
            var don = await _context.DonUngTuyens.Include(p => p.Users).Include(p => p.PhieuDangTuyens).Include(p => p.Statuses).Where(p => p.PhieuDangTuyenId == id).ToListAsync();
            if (don == null)
            {
                return NotFound();
            }
            return View(don);
        }
    }
}
