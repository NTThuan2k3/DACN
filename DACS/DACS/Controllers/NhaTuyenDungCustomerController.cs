using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DACS.Controllers
{
    public class NhaTuyenDungCustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly ITinhRepository _tinhRepository;
        private readonly IPhieuDangTuyenRepository _phieuDangTuyenRepository;
        public NhaTuyenDungCustomerController(ApplicationDbContext context, UserManager<User> userManager, INhaTuyenDungRepository nhaTuyenDungRepository, ITinhRepository tinhRepository, IPhieuDangTuyenRepository phieuDangTuyenRepository)
        {
            _context = context;
            _userManager = userManager;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _tinhRepository = tinhRepository;
            _phieuDangTuyenRepository = phieuDangTuyenRepository;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var list = _context.PhieuDangTuyens.ToList();

            //Lấy tỉnh, tp 
            ViewBag.ListTinhs = _context.Tinhs.Include(p => p.NhaTuyenDungs).ToList();
            //lấy thông tin phiếu đăng tuyển
            ViewBag.ListPhieuDangTuyens = list;
            if (currentUser.Id != null)
            {
                var nhaTuyenDung = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Where(u => u.UserId == currentUser.Id).ToListAsync();
                return View(nhaTuyenDung);
            }
            return View();
        }
        public async Task<string> SaveImage(IFormFile image)
        {
            // Tạo tên file duy nhất để tránh trùng lặp
            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileName);

            // Lưu file vào thư mục wwwroot/images
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn tương đối của file
            return "/images/" + fileName;
        }

        private bool IsImageValid(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            // Kiểm tra loại tệp và kích thước
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
            var maxFileSize = 5 * 1024 * 1024; // 5MB
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) || file.Length > maxFileSize)
            {
                return false;
            }

            // Kiểm tra xem tệp có phải là hình ảnh hợp lệ không, ví dụ kiểm tra magic number hoặc header của file

            return true;
        }

        public async Task<IActionResult> Create()
        {
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhaTuyenDung ntd, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            ModelState.Remove("image1");
            ModelState.Remove("image2");
            ModelState.Remove("image3");
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                ntd.UserId = currentUser.Id;
                if (image1 != null)
                {
                    ntd.GiayPhepKinhDoanh = await SaveImage(image1);
                }
                if (image2 != null)
                {
                    ntd.HinhAnhCty = await SaveImage(image2);
                }
                if (image3 != null)
                {
                    ntd.ImageDaiDien = await SaveImage(image3);
                }
                ntd.StatusId = 2;
                ntd.XetDuyet = "Waiting";
                await _nhaTuyenDungRepository.AddAsync(ntd);
                return RedirectToAction("Index", "Home");
            }

            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View(ntd);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ntd = await _context.NhaTuyenDungs.FindAsync(id);
            if (ntd == null)
            {
                return NotFound();
            }
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View(ntd);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NhaTuyenDung ntd, IFormFile image1, IFormFile image2)
        {
            ModelState.Remove("image1");
            ModelState.Remove("image2");
            if (id != ntd.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingNTD = await _nhaTuyenDungRepository.GetByIdAsync(id);
                if (image1 == null)
                {
                    ntd.ImageDaiDien = existingNTD.ImageDaiDien;
                }
                else
                {
                    // Lưu hình ảnh mới
                    ntd.ImageDaiDien = await SaveImage(image1);
                }
                if (image2 == null)
                {
                    ntd.ImageBangTin = existingNTD.ImageBangTin;
                }
                else
                {
                    // Lưu hình ảnh mới
                    ntd.ImageBangTin = await SaveImage(image2);
                }
                // Cập nhật các thông tin khác của sản phẩm
                existingNTD.TenNTD = ntd.TenNTD;
                existingNTD.SDT = ntd.SDT;
                existingNTD.TinhId = ntd.TinhId;
                existingNTD.DiaDiem = ntd.DiaDiem;
                existingNTD.Email = ntd.Email;
                existingNTD.ImageDaiDien = ntd.ImageDaiDien;
                existingNTD.ImageBangTin = ntd.ImageBangTin;
                existingNTD.StatusId = 1;
                existingNTD.Website = ntd.Website;
                existingNTD.QuyMo = ntd.QuyMo;
                existingNTD.MoTa = ntd.MoTa;

                // Save the updated user in the database
                await _nhaTuyenDungRepository.UpdateAsync(existingNTD);
                return RedirectToAction(nameof(Index));
            }
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View(ntd);
        }
    }
}
