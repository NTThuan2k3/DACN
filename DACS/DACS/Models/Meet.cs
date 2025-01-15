using System.ComponentModel.DataAnnotations;
namespace DACS.Models
{
    public class Meet
    {
        public int Id { get; set; }
        public string? Method { get; set; }
        public DateOnly? Date { get; set; }
        public TimeOnly? Time { get; set; }
        public string? Address { get; set; }
        public string? Link { get; set; }
        public string? YeuCau { get; set; }
        public string? UserId { get; set; }
        public User? Users { get; set; }
        public int NhaTuyenDungId { get; set; }
        public NhaTuyenDung? NhaTuyenDungs { get; set; }
    }
}
