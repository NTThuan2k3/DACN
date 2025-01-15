using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DACS.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IStatusRepository _statusRepository;
        private readonly ApplicationDbContext _context;

        public UserController(IUserRepository userRepository, UserManager<User> userManager, IStatusRepository statusRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _statusRepository = statusRepository;
        }
        public async Task<IActionResult> Index(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user, IFormFile image)
        {
            ModelState.Remove("image");

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.GetUserAsync(User);

                if (image == null)
                {
                    user.Image = existingUser.Image;
                }
                else
                {
                    // Lưu hình ảnh mới
                    user.Image = await SaveImage(image);
                }

                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.CCCD = user.CCCD;
                existingUser.Image = user.Image;
                existingUser.StatusId = 1;

                await _userRepository.UpdateAsync(existingUser);
                return RedirectToAction(nameof(Details));
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
    }
}
