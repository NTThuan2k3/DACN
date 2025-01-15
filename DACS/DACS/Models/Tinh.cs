namespace DACS.Models
{
    public class Tinh
    {
        public int Id { get; set; }
        public string TenTinh { get; set; }
        public List<NhaTuyenDung>? NhaTuyenDungs { get; set; }
        public List<PhieuDangTuyen>? PhieuDangTuyens { get; set; }
    }
}