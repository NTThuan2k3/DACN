using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class DonUngTuyen
    {
        public int Id { get; set; }    
        public string? FileCV { get; set; }
        public string? TenCV { get; set; }
        public string? XetDuyet { get; set; }
        public int StatusId { get; set; }
        public Status? Statuses { get; set; }
        public string? UserId { get; set; }
        public User? Users { get; set; }
        public int PhieuDangTuyenId { get; set; }
        public PhieuDangTuyen? PhieuDangTuyens { get; set; }             
    }
}
