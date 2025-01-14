using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class NhaTuyenDung
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Please enter title name")]
        //[StringLength(100, ErrorMessage = "The title name cannot be more than 100 characters.")]
        public string? TenNTD { get; set; }
        //[Required(ErrorMessage = "Please enter Phone")]
        //[StringLength(13, ErrorMessage = "The title name cannot be more than 13 characters.")]
        public string? SDT { get; set; }
        public int? TinhId { get; set; }
        public Tinh? Tinhs { get; set; }
        //[Required(ErrorMessage = "Please enter address")]
        //[StringLength(300, ErrorMessage = "The title name cannot be more than 300 characters.")]
        public string? DiaDiem { get; set; }
        //[Required(ErrorMessage = "Please enter email")]
        //[StringLength(300, ErrorMessage = "The title name cannot be more than 300 characters.")]
        public string? Email { get; set; }
        public string? ImageDaiDien { get; set; }
        public string? ImageBangTin { get; set; }
        public string? Website { get; set; }
        public string? QuyMo { get; set; }
        public string? MoTa { get; set; }
        public string? XetDuyet { get; set; }
        public string? UserId { get; set; }
        public User? Users { get; set; }
        public int? StatusId { get; set; }
        public Status? Statuses { get; set; }
    }
}
