using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class User : IdentityUser
    {
        [StringLength(100, ErrorMessage = "Họ tên không quá 100 ký tự")]
        public string FullName { get; set; }
        [StringLength(13, ErrorMessage = "Số điện thoại không quá 13 ký tự")]
        public string? PhoneNumber { get; set; }
        [StringLength(13, ErrorMessage = "Căn cước công dân không quá 13 ký tự")]
        public string? CCCD { get; set; }        
        public string? Image { get; set; }
        public int StatusId { get; set; }
        public Status? Statuses { get; set; }
    }
}
