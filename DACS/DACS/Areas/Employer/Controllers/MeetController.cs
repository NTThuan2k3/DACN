using DACS.Models;
using DACS.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = SD.Role_Employer)]
    public class MeetController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly IMeetRepository _meetRepository;
        private readonly IPhieuDangTuyenRepository _phieuDangTuyenRepository;
        public MeetController(ApplicationDbContext context, UserManager<User> userManager, INhaTuyenDungRepository nhaTuyenDungRepository, IPhieuDangTuyenRepository phieuDangTuyenRepository, IMeetRepository meetRepository)
        {
            _context = context;
            _userManager = userManager;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
            _meetRepository = meetRepository;
            _phieuDangTuyenRepository = phieuDangTuyenRepository;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != null)
            {
                var ntd = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).FirstOrDefaultAsync(u => u.UserId == currentUser.Id);
                var met = await _context.Meets.Include(p => p.NhaTuyenDungs).Include(p => p.Users).Where(u => u.NhaTuyenDungId == ntd.Id).ToListAsync();
                return View(met);
            }
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Meet met)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser.Id != null)
                {
                    var ntd = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).FirstOrDefaultAsync(u => u.UserId == currentUser.Id);
                    met.NhaTuyenDungId = ntd.Id;
                    await _meetRepository.AddAsync(met);
                    return RedirectToAction("Index");
                }
            }
            return View(met);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var met = await _context.Meets.FindAsync(id);
            if (met == null)
            {
                return NotFound();
            }
            return View(met);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Meet met)
        {
            if (id != met.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingMet = await _meetRepository.GetByIdAsync(id);
                existingMet.Date = met.Date;
                existingMet.Time = met.Time;
                existingMet.Address = met.Address;
                existingMet.Link = met.Link;
                existingMet.YeuCau = met.YeuCau;

                // Save the updated user in the database
                await _meetRepository.UpdateAsync(existingMet);
                return RedirectToAction(nameof(Index));
            }
            return View(met);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var met = await _context.Meets.Include(p => p.NhaTuyenDungs).FirstOrDefaultAsync(p => p.Id == id);
            if (met == null)
            {
                return NotFound();
            }

            return View(met);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _meetRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
