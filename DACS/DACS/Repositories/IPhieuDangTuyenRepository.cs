using DACS.Models;

namespace DACS.Repositories
{
    public interface IPhieuDangTuyenRepository
    {
        Task<IEnumerable<PhieuDangTuyen>> GetAllAsync();
        Task<PhieuDangTuyen> GetByIdAsync(int id);
        Task AddAsync(PhieuDangTuyen phieu);
        Task UpdateAsync(PhieuDangTuyen phieu);
        Task DeleteAsync(int id);
        Task<IEnumerable<PhieuDangTuyen>> SearchPhieuDangTuyens(Func<PhieuDangTuyen, bool> predicate);
    }
}
