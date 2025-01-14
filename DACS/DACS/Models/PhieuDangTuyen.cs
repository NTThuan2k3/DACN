using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DACS.Models
{
    public class PhieuDangTuyen
    {
        public int Id { get; set; }
        public string? TenNganh {  get; set; }
        public string? MucLuong { get; set; }
        public string? NamKN { get; set; }
        public int TinhId { get; set; }
        public Tinh? Tinhs { get; set; }
        public string TenViecLam { get; set; }
        public string? ChucDanh { get; set; }
        public string? MoTa { get; set; }
        [Required(ErrorMessage = "Please enter title address")]
        [StringLength(300, ErrorMessage = "The title name cannot be more than 100 characters.")]
        public string? DiaDiem { get; set; }
        public DateTime? NgayLapPhieu { get; set; }
        public DateTime? HanNopHoSo { get; set; }
        public int NhaTuyenDungId { get; set; }
        public NhaTuyenDung? NhaTuyenDungs { get; set; }
        public int StatusId { get; set; }
        public Status? Statuses { get; set; }
    }
}
