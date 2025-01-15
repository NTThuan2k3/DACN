using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IStatusRepository _statusRepository;

        public UserController(IUserRepository userRepository, UserManager<User> userManager, IStatusRepository statusRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _statusRepository = statusRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(user.Id, roles);
            }

            ViewData["UserRoles"] = userRoles;
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    if (!IsImageValid(image))
                    {
                        ModelState.AddModelError("imageUrl", "Hình ảnh không hợp lệ. Vui lòng chọn một hình ảnh có định dạng JPEG, PNG hoặc GIF và kích thước nhỏ hơn 5MB.");
                        return View(user);
                    }
                    // Lưu hình ảnh
                    user.Image = await SaveImage(image);
                }
                user.StatusId = 1;
                await _userRepository.AddAsync(user);
                return RedirectToAction("Index");
            }

            return View(user);
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

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var status = await _statusRepository.GetAllAsync();
            ViewBag.Status = new SelectList(status, "Id", "StatusName");
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, User user, IFormFile image)
        {
            ModelState.Remove("image"); // Loại bỏ xác thực ModelState cho image

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetByIdAsync(id);

                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                if (image == null)
                {
                    user.Image = existingUser.Image;
                }
                else
                {
                    // Lưu hình ảnh mới và gán đường dẫn vào đối tượng User
                    user.Image = await SaveImage(image);
                }

                // Cập nhật các thông tin khác của người dùng
                existingUser.FullName = user.FullName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.CCCD = user.CCCD;
                existingUser.Image = user.Image; // Cập nhật thông tin hình ảnh
                existingUser.StatusId = user.StatusId;

                await _userRepository.UpdateAsync(existingUser);
                return RedirectToAction(nameof(Index)); // Chuyển hướng đến action Index
            }
            var status = await _statusRepository.GetAllAsync();
            ViewBag.Status = new SelectList(status, "Id", "StatusName");
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _userRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
