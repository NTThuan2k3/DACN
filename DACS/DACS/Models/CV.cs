using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class CV
    {
        public int Id { get; set; }
        public string FileCV { get; set; }
        public string? TenCV { get; set; }
        public string? UserId { get; set; }
        public User? Users { get; set; }
    }
}
