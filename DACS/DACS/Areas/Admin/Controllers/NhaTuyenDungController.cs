using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class NhaTuyenDungController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly ITinhRepository _tinhRepository;
        private readonly IPhieuDangTuyenRepository _phieuDangTuyenRepository;
        public NhaTuyenDungController(ApplicationDbContext context, UserManager<User> userManager, INhaTuyenDungRepository nhaTuyenDungRepository, ITinhRepository tinhRepository, IPhieuDangTuyenRepository phieuDangTuyenRepository)
        {
            _context = context;
            _userManager = userManager;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _tinhRepository = tinhRepository;
            _phieuDangTuyenRepository = phieuDangTuyenRepository;
        }

        public async Task<IActionResult> XetDuyet()
        {
            var list = _context.PhieuDangTuyens.ToList();

            //Lấy tỉnh, tp 
            ViewBag.ListTinhs = _context.Tinhs.Include(p => p.NhaTuyenDungs).ToList();
            //lấy thông tin phiếu đăng tuyển
            ViewBag.ListPhieuDangTuyens = list;
            var nhaTuyenDung = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Where(u => u.XetDuyet == "Waiting").ToListAsync();
            return View(nhaTuyenDung);
        }

        [HttpPost]
        public async Task<IActionResult> XetDuyet(int id, string decision)
        {
            if (decision == "YES")
            {
                // Xử lý logic khi bị đồng ý
                var ntd = await _nhaTuyenDungRepository.GetByIdAsync(id);
                ntd.StatusId = 1;
                ntd.XetDuyet = "Yes";
                _context.NhaTuyenDungs.Update(ntd);

                // Đổi quyền user => employer
                var user = await _userManager.FindByIdAsync(ntd.UserId);
                if (user != null)
                {
                    // Xóa quyền cũ của người dùng (nếu có)
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (currentRoles.Contains(SD.Role_Customer))
                    {
                        await _userManager.RemoveFromRoleAsync(user, SD.Role_Customer);
                    }

                    // Thêm quyền Employer
                    await _userManager.AddToRoleAsync(user, SD.Role_Employer);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("XetDuyet");
            }
            if (decision == "NO")
            {
                // Xử lý logic khi bị từ chối
                var ntd = await _nhaTuyenDungRepository.GetByIdAsync(id);
                ntd.XetDuyet = "No";
                _context.NhaTuyenDungs.Update(ntd);

                await _context.SaveChangesAsync();
                return RedirectToAction("XetDuyet");
            }

            return RedirectToAction("XetDuyet");
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
                var nhaTuyenDung = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Where(u => u.UserId == currentUser.Id && u.XetDuyet == "Đã Duyệt").ToListAsync();
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
        public async Task<IActionResult> Edit(int id, NhaTuyenDung ntd, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            ModelState.Remove("image1");
            ModelState.Remove("image2");
            ModelState.Remove("image3");
            if (id != ntd.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingNTD = await _nhaTuyenDungRepository.GetByIdAsync(id);
                if (image1 != null)
                {
                    existingNTD.GiayPhepKinhDoanh = await SaveImage(image1);
                }
                if (image2 != null)
                {
                    existingNTD.HinhAnhCty = await SaveImage(image2);
                }
                if (image3 != null)
                {
                    existingNTD.ImageDaiDien = await SaveImage(image3);
                }

                // Cập nhật các thông tin khác của sản phẩm
                existingNTD.TenNTD = ntd.TenNTD;
                existingNTD.SDT = ntd.SDT;
                existingNTD.TinhId = ntd.TinhId;
                existingNTD.ChucVu = ntd.ChucVu;
                existingNTD.Email = ntd.Email;
                existingNTD.TenCty = ntd.TenCty;
                existingNTD.MaSoThue = ntd.MaSoThue;
                existingNTD.DiaDiem = ntd.DiaDiem;
                existingNTD.Website = ntd.Website;
                existingNTD.QuyMo = ntd.QuyMo;
                existingNTD.ThongTinCty = ntd.ThongTinCty;
                existingNTD.StatusId = 1;

                // Save the updated user in the database
                _context.Update(existingNTD);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View(ntd);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ntd = await _nhaTuyenDungRepository.GetByIdAsync(id);
            if (ntd == null)
            {
                return NotFound();
            }

            return View(ntd);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _nhaTuyenDungRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
