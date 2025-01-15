using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = SD.Role_Employer)]
    public class PhieuDangTuyenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IPhieuDangTuyenRepository _phieuDangTuyenRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITinhRepository _tinhRepository;
        private readonly IDonUngTuyenRepository _donUngTuyenRepository;
        private readonly INhaTuyenDungRepository _nhaTuyenDungRepository;

        public PhieuDangTuyenController(ApplicationDbContext context, UserManager<User> userManager, IStatusRepository statusRepository, IPhieuDangTuyenRepository phieuDangTuyenRepository, IUserRepository userRepository, ITinhRepository tinhRepository, IDonUngTuyenRepository donUngTuyenRepository, INhaTuyenDungRepository nhaTuyenDungRepository)
        {
            _context = context;
            _userManager = userManager;
            _phieuDangTuyenRepository = phieuDangTuyenRepository;
            _statusRepository = statusRepository;
            _userRepository = userRepository;
            _tinhRepository = tinhRepository;
            _donUngTuyenRepository = donUngTuyenRepository;
            _nhaTuyenDungRepository = nhaTuyenDungRepository;
        }

        public async Task<IActionResult> Index(string search, string tinh, string nganh)
        {
            var list = await _phieuDangTuyenRepository.GetAllAsync();
            ViewBag.FilterTinhs = _context.Tinhs.OrderBy(p => p.TenTinh).ToList();
            ViewBag.FilterNganhs = _context.PhieuDangTuyens.Select(p => p.TenNganh).Distinct().OrderBy(p => p).ToList();

            ViewBag.SelectedTenTinh = tinh; // Gán giá trị cho ViewBag.SelectedProvinceId
            ViewBag.SelectedTenNganh = nganh; // Gán giá trị cho ViewBag.SelectedSkillId

            //Lấy tỉnh, tp 
            ViewBag.ListTinhs = _context.Tinhs.Include(p => p.NhaTuyenDungs).ToList();
            //lấy thông tin phiếu đăng tuyển
            ViewBag.ListPhieuDangTuyens = list;


            if (!String.IsNullOrEmpty(search))
            {
                list = list.Where(s => s.TenViecLam.Contains(search)).ToList();
                if (tinh != null)
                {
                    list = list.Where(s => s.Tinhs.TenTinh == tinh && s.StatusId == 1).ToList();
                }
                if (nganh != null)
                {
                    list = list.Where(s => s.TenNganh == nganh && s.StatusId == 1).ToList();
                }
                return View(list);
            }
            else
            {
                if (tinh != null)
                {
                    list = list.Where(s => s.Tinhs.TenTinh == tinh && s.StatusId == 1).ToList();
                }
                if (nganh != null)
                {
                    list = list.Where(s => s.TenNganh == nganh && s.StatusId == 1).ToList();
                }
                return View(list);
            }
        }

        public async Task<IActionResult> Details(int id, string tinh, string nganh)
        {
            var list = await _phieuDangTuyenRepository.GetAllAsync();
            ViewBag.FilterTinhs = _context.Tinhs.OrderBy(p => p.TenTinh).ToList();
            ViewBag.FilterNganhs = _context.PhieuDangTuyens.Select(p => p.TenNganh).Distinct().OrderBy(p => p).ToList();

            ViewBag.SelectedTenTinh = tinh; // Gán giá trị cho ViewBag.SelectedProvinceId
            ViewBag.SelectedTenNganh = nganh; // Gán giá trị cho ViewBag.SelectedSkillId

            //Lấy tỉnh, tp 
            ViewBag.ListTinhs = _context.Tinhs.Include(p => p.NhaTuyenDungs).ToList();
            //lấy thông tin phiếu đăng tuyển
            ViewBag.ListPhieuDangTuyens = list;

            var phieu = await _context.PhieuDangTuyens.Include(p => p.NhaTuyenDungs).Include(p => p.Tinhs).Include(p => p.Statuses).FirstOrDefaultAsync(p => p.Id == id);
            if (phieu == null)
            {
                return NotFound();
            }
            var userManager = HttpContext.RequestServices.GetService(typeof(UserManager<User>)) as UserManager<User>;
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                // Truyền thông tin người dùng vào ViewBag hoặc ViewData
                ViewBag.FullName = user.FullName;
                ViewBag.PhoneNumber = user.PhoneNumber;
                ViewBag.Email = user.Email;
            }

            var cvs = await _context.CVs.Include(p => p.Users).Where(p => p.UserId == user.Id).ToListAsync();
            ViewBag.CV = new SelectList(cvs, "Id", "FileCV");

            return View(phieu);
        }

        public async Task<IActionResult> DetailsPhieu(int id)
        {
            var phieu = await _context.PhieuDangTuyens.Include(p => p.NhaTuyenDungs).Include(p => p.Tinhs).Include(p => p.Statuses).FirstOrDefaultAsync(p => p.Id == id);
            if (phieu == null)
            {
                return NotFound();
            }
            return View(phieu);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //var user = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Include(p => p.Statuses).Where(p => p.StatusId == 1).ToListAsync();
            //ViewBag.User = new SelectList(user, "Id", "TenNTD");
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhieuDangTuyen phieu)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var ntd = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Include(p => p.Statuses).FirstOrDefaultAsync(p => p.UserId == currentUser.Id);
                phieu.NhaTuyenDungId = ntd.Id;
                phieu.StatusId = 1;
                await _phieuDangTuyenRepository.AddAsync(phieu);
                return RedirectToAction("Index");
            }
            //var user = await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Include(p => p.Statuses).Where(p => p.StatusId == 1).ToListAsync();
            //ViewBag.User = new SelectList(user, "Id", "TenNTD");
            var tinh = await _context.Tinhs.ToListAsync();
            ViewBag.Tinh = new SelectList(tinh, "Id", "TenTinh");
            return View(phieu);
        }
        public async Task<IActionResult> IndexCaNhan(string id)
        {
            var phieu = await _context.PhieuDangTuyens.Include(p => p.NhaTuyenDungs).Include(p => p.Tinhs).Include(p => p.Statuses).Where(p => p.NhaTuyenDungs.UserId == id).ToListAsync();
            if (phieu == null)
            {
                return NotFound();
            }
            return View(phieu);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var don = await _phieuDangTuyenRepository.GetByIdAsync(id);
            if (don == null)
            {
                return NotFound();
            }

            return View(don);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _phieuDangTuyenRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
