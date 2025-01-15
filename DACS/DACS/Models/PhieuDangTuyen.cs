using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DACS.Models
{
    public class PhieuDangTuyen
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng điền thông tin")]
        [StringLength(100, ErrorMessage = "Tên ngành không được quá 100 ký tự.")]
        public string? TenNganh {  get; set; }
        public string? MucLuong { get; set; }
        public string? NamKN { get; set; }
        public int TinhId { get; set; }
        public Tinh? Tinhs { get; set; }
        [Required(ErrorMessage = "Vui lòng điền thông tin")]
        [StringLength(100, ErrorMessage = "Tên việc làm không được quá 100 ký tự.")]
        public string TenViecLam { get; set; }
        public string? ChucDanh { get; set; }
        [StringLength(500, ErrorMessage = "Vui lòng không điền quá 500 kýt tự.")] 
        public string? MoTa { get; set; }
        public string? DiaDiem { get; set; }
        public DateTime? NgayLapPhieu { get; set; }
        public DateTime? HanNopHoSo { get; set; }
        public int NhaTuyenDungId { get; set; }
        public NhaTuyenDung? NhaTuyenDungs { get; set; }
        public int StatusId { get; set; }
        public Status? Statuses { get; set; }
    }
}
