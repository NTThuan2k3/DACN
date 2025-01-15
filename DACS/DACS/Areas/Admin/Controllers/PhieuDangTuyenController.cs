using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class PhieuDangTuyenController : Controller
    {
        private readonly IPhieuDangTuyenRepository _phieuDangTuyenRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITinhRepository _tinhRepository;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;
        private readonly ApplicationDbContext _context;

        public PhieuDangTuyenController(IPhieuDangTuyenRepository phieuDangTuyenRepository, IStatusRepository statusRepository, IUserRepository userRepository, ITinhRepository tinhRepository, ApplicationDbContext context, INhaTuyenDungRepository nhaTuyenDungRepository)
        {
            _phieuDangTuyenRepository = phieuDangTuyenRepository;
            _statusRepository = statusRepository;
            _userRepository = userRepository;
            _tinhRepository = tinhRepository;
            _context = context;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _phieuDangTuyenRepository.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Include(p => p.Statuses).Where(p => p.StatusId == 1).ToListAsync();
            ViewBag.User = new SelectList(user, "Id", "TenNTD");
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhieuDangTuyen phieu)
        {
            if (ModelState.IsValid)
            {
                phieu.StatusId = 1;
                await _phieuDangTuyenRepository.AddAsync(phieu);
                return RedirectToAction("Index");
            }
            var user = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Include(p => p.Statuses).Where(p => p.StatusId == 1).ToListAsync();
            ViewBag.User = new SelectList(user, "Id", "TenNTD");
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View(phieu);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var phieu = await _phieuDangTuyenRepository.GetByIdAsync(id);
            if (phieu == null)
            {
                return NotFound();
            }
            var user = await _context.NhaTuyenDungs.Include(p => p.Statuses).Where(p => p.StatusId == 1).ToListAsync();
            ViewBag.User = new SelectList(user, "Id", "TenNTD");
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            var status = await _statusRepository.GetAllAsync();
            ViewBag.Status = new SelectList(status, "Id", "StatusName");
            return View(phieu);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PhieuDangTuyen phieuDangTuyen)
        {
            if (id != phieuDangTuyen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingPhieu = await _phieuDangTuyenRepository.GetByIdAsync(id);
                existingPhieu.TenViecLam = phieuDangTuyen.TenViecLam;
                existingPhieu.ChucDanh = phieuDangTuyen.ChucDanh;
                existingPhieu.MoTa = phieuDangTuyen.MoTa;
                existingPhieu.DiaDiem = phieuDangTuyen.DiaDiem;
                existingPhieu.NgayLapPhieu = phieuDangTuyen.NgayLapPhieu;
                existingPhieu.HanNopHoSo = phieuDangTuyen.HanNopHoSo;
                existingPhieu.NhaTuyenDungId = phieuDangTuyen.NhaTuyenDungId;
                existingPhieu.TenNganh = phieuDangTuyen.TenNganh;
                existingPhieu.MucLuong = phieuDangTuyen.MucLuong;
                existingPhieu.NamKN = phieuDangTuyen.NamKN;
                existingPhieu.TinhId = phieuDangTuyen.TinhId;
                existingPhieu.StatusId = phieuDangTuyen.StatusId;

                await _phieuDangTuyenRepository.UpdateAsync(existingPhieu);
                return RedirectToAction(nameof(Index));
            }
            var user = await _context.NhaTuyenDungs.Include(p => p.Statuses).Where(p => p.StatusId == 1).ToListAsync();
            ViewBag.User = new SelectList(user, "Id", "TenNTD");
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            var status = await _statusRepository.GetAllAsync();
            ViewBag.Status = new SelectList(status, "Id", "StatusName");
            return View(phieuDangTuyen);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var phieu = await _phieuDangTuyenRepository.GetByIdAsync(id);
            if (phieu == null)
            {
                return NotFound();
            }

            return View(phieu);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _phieuDangTuyenRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
