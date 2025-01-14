using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DACS.Controllers
{
    public class UserNTDController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IStatusRepository _statusRepository;

        public UserNTDController(IUserRepository userRepository, UserManager<User> userManager, IStatusRepository statusRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _statusRepository = statusRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetByIdAsync(id);
                existingUser.StatusId = 1;
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.CCCD = user.CCCD;

                await _userRepository.UpdateAsync(existingUser);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
