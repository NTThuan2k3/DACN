using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class NhaTuyenDung
    {
        public int Id { get; set; }
        public string? TenNTD { get; set; }
        [StringLength(20, ErrorMessage = "Vui lòng điền số điện thoại hợp lệ.", MinimumLength = 9)]
        public string? SDT { get; set; }
        [Required(ErrorMessage = "Vui lòng điền thông tin")]
        [StringLength(50, ErrorMessage = "Email của bạn không được quá 50 ký tự.")]
        public string? Email { get; set; }
        public string? ChucVu { get; set; }
        public string? TenCty { get; set; }
        public string? GiayPhepKinhDoanh { get; set; }
        public string? MaSoThue { get; set; }
        public string? GiayChungThuc { get; set; }
        public int? TinhId { get; set; }
        public Tinh? Tinhs { get; set; }
        public string? DiaDiem { get; set; }
        public string? Website { get; set; }
        public string? QuyMo { get; set; }
        public string? HinhAnhCty { get; set; }
        public string? ThongTinCty { get; set; }
        public string? ImageDaiDien { get; set; }
        public string? ImageBangTin { get; set; }
        public string? MoTa { get; set; }
        public string? TinNhanThem { get; set; }
        public string? XetDuyet { get; set; }
        public string? UserId { get; set; }
        public User? Users { get; set; }
        public int? StatusId { get; set; }
        public Status? Statuses { get; set; }
    }
}
