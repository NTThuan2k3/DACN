using DACS.Models;

namespace DACS.Repositories
{
    public interface IDonUngTuyenRepository
    {
        Task<IEnumerable<DonUngTuyen>> GetAllAsync();
        Task<DonUngTuyen> GetByIdAsync(int id);
        Task AddAsync(DonUngTuyen don);
        Task UpdateAsync(DonUngTuyen don);
        Task DeleteAsync(int id);
        Task<IEnumerable<DonUngTuyen>> SearchDonUngTuyens(Func<DonUngTuyen, bool> predicate);
    }
}
