namespace DACS.Models
{
    public class Status
    {
        public int Id { get; set; }
        public bool StatusName { get; set; }
        public List<DonUngTuyen>? DonUngTuyens { get; set; }
        public List<NhaTuyenDung>? NhaTuyenDungs { get; set; }
        public List<PhieuDangTuyen>? PhieuDangTuyens { get; set; }
        public List<User>? Users { get; set; }
    }
}
