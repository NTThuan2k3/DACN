using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Employer.Controllers
{
	[Area("Employer")]
	[Authorize(Roles = SD.Role_Employer)]
	public class DonUngTuyenController : Controller
	{
		private readonly IDonUngTuyenRepository _donUngTuyenRepository;
		private readonly IUserRepository _userRepository;
		private readonly UserManager<User> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IMeetRepository _meetRepository;

        public DonUngTuyenController(IDonUngTuyenRepository donUngTuyenRepository, UserManager<User> userManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IUserRepository userRepository, IMeetRepository meetRepository, IEmailSender emailSender)
		{
			_donUngTuyenRepository = donUngTuyenRepository;
			_userManager = userManager;
			_context = context;
			_webHostEnvironment = webHostEnvironment;
			_userRepository = userRepository;
			_meetRepository = meetRepository;
            _emailSender = emailSender;
        }

        [Authorize]
        public async Task<IActionResult> ViewCV(int id)
        {
            var don = await _context.DonUngTuyens.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == id);
            var phieu = await _context.PhieuDangTuyens.Include(p => p.NhaTuyenDungs).FirstOrDefaultAsync(p => p.Id == don.PhieuDangTuyenId);
            var ntd = await _context.NhaTuyenDungs.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == phieu.NhaTuyenDungId);

            if (don == null || ntd.UserId != _userManager.GetUserId(User))
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

        public async Task<IActionResult> TrungTuyen(int id)
        {
            var don = await _context.DonUngTuyens.Include(p => p.Users).Include(p => p.PhieuDangTuyens).Include(p => p.Statuses).Where(p => p.PhieuDangTuyenId == id && p.XetDuyet == "Yes").ToListAsync();
            if (don == null)
            {
                return NotFound();
            }
            return View(don);
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

        [HttpPost]
        public async Task<IActionResult> DetailsPhieu(int id, string decision)
        {
            //Lấy thông tin đơn và người ứng tuyển
            var don = await _donUngTuyenRepository.GetByIdAsync(id);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == don.UserId);

            if (decision == "YES")
            {
                // Xử lý logic khi bị đồng ý
                don.XetDuyet = "Yes";

                // Gửi email chúc mừng trúng tuyển
                if (user != null)
                {
                    string emailContent = $@"
					Kính gửi {user.FullName},

					Chúc mừng bạn đã vượt qua vòng ứng tuyển tại công ty chúng tôi!
					Chúng tôi sẽ gặp lại bạn vào buổi phỏng vấn sắp tới.

					Hẹn gặp lại và chúc bạn một ngày tốt lành.

					Trân trọng,
					Phòng nhân sự";

                    await _emailSender.SendEmailAsync(user.Email, "Thông báo kết quả ứng tuyển", emailContent);
                }

                _context.DonUngTuyens.Update(don);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsPhieu", id);
            }
            if (decision == "NO")
            {
                // Xử lý logic khi bị từ chối
                don.XetDuyet = "No";

                // Gửi email cảm ơn ứng viên
                if (user != null)
                {
                    string emailContent = $@"
					Kính gửi {user.FullName},

					Chúng tôi cảm ơn bạn đã quan tâm và nộp hồ sơ ứng tuyển vào vị trí tại công ty chúng tôi.
					Sau khi xem xét, rất tiếc chúng tôi chưa thể tiếp nhận hồ sơ của bạn vào thời điểm này.

					Chúng tôi trân trọng mọi nỗ lực và sự quan tâm của bạn và hy vọng sẽ có cơ hội hợp tác trong tương lai.

					Trân trọng,
					Phòng nhân sự";

                    await _emailSender.SendEmailAsync(user.Email, "Thông báo kết quả ứng tuyển", emailContent);
                }

                _context.DonUngTuyens.Update(don);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsPhieu", id);
            }

            return RedirectToAction("DetailsPhieu", id);
        }

        [HttpPost]
        public async Task<IActionResult> HenNhieu(int[] selectedItems, DateTime date, TimeSpan startTime, int interval, string Method, string address, string link, int idPhieu)
        {
            if (selectedItems == null || selectedItems.Length == 0)
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một ứng viên.";
                return RedirectToAction("TrungTuyen", idPhieu);
            }

            // Lấy thông tin người dùng hiện tại (Nhà tuyển dụng)
            var currentUser = await _userManager.GetUserAsync(User);
            var ntd = await _context.NhaTuyenDungs.FirstOrDefaultAsync(ntd => ntd.UserId == currentUser.Id);

            if (ntd == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin nhà tuyển dụng.";
                return RedirectToAction("TrungTuyen");
            }

            TimeSpan currentMeetingTime = startTime;

            // Duyệt qua từng ứng viên đã chọn
            foreach (var id in selectedItems)
            {
                var don = await _context.DonUngTuyens.Include(d => d.Users).FirstOrDefaultAsync(d => d.Id == id);

                var user = don.Users;

                // Tạo nội dung email
                string emailContent = $@"
					Kính gửi {user.FullName},
            
					{ntd.TenNTD} trân trọng mời bạn tham gia buổi phỏng vấn với thông tin như sau:
					- Cách thức: {Method}
					- Thời gian: {currentMeetingTime}, {date.ToShortDateString()} 
					- Địa điểm: {address}
					- Link: {link}
					- Yêu cầu: Ăn mặc nghiêm chỉnh và mang theo CV.

					Trân trọng,
					{ntd.TenNTD}";

                // Gửi email
                await _emailSender.SendEmailAsync(user.Email, "Thư mời phỏng vấn", emailContent);

                // Lưu thông tin vào bảng Meet
                var meet = new Meet
                {
                    Method = Method,
                    Date = DateOnly.FromDateTime(date),
                    Time = TimeOnly.FromTimeSpan(currentMeetingTime),
                    Address = address,
                    Link = link,
                    UserId = user.Id,
                    YeuCau = "Ăn mặc nghiêm chỉnh và mang theo CV.",
                    NhaTuyenDungId = ntd.Id
                };

				_context.Meets.Add(meet);
                await _context.SaveChangesAsync();

				// Tăng thời gian hẹn cho ứng viên tiếp theo theo thời gian giãn cách
				currentMeetingTime = currentMeetingTime.Add(new TimeSpan(0, interval, 0));
			}

            TempData["Success"] = "Đã gửi email thành công.";
            return RedirectToAction("DetailsPhieu", new { id = idPhieu });
        }
    }
}
