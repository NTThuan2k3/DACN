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
    public class TinhController : Controller
    {
        private readonly ITinhRepository _tinhRepository;
        private readonly IStatusRepository _statusRepository;

        public TinhController(ITinhRepository tinhRepository, IStatusRepository statusRepository)
        {
            _tinhRepository = tinhRepository;
            _statusRepository = statusRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tinh = await _tinhRepository.GetAllAsync();
            return View(tinh);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tinh tinh)
        {
            if (ModelState.IsValid)
            {
                await _tinhRepository.AddAsync(tinh);
                return RedirectToAction(nameof(Index));
            }
            return View(tinh);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tinh = await _tinhRepository.GetByIdAsync(id);
            if (tinh == null)
            {
                return NotFound();
            }
            var status = await _statusRepository.GetAllAsync();
            ViewBag.Status = new SelectList(status, "Id", "StatusName");
            return View(tinh);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Tinh tinh)
        {
            if (id != tinh.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingTinh = await _tinhRepository.GetByIdAsync(id);
                existingTinh.TenTinh = tinh.TenTinh;
                
                await _tinhRepository.UpdateAsync(existingTinh);
                return RedirectToAction(nameof(Index));
            }
            return View(tinh);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tinh = await _tinhRepository.GetByIdAsync(id);
            if (tinh == null)
            {
                return NotFound();
            }

            return View(tinh);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tinhRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
