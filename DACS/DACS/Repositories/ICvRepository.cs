using DACS.Models;

namespace DACS.Repositories
{
    public interface ICvRepository
    {
        Task<IEnumerable<CV>> GetAllAsync();
        Task<CV> GetByIdAsync(int id);
        Task AddAsync(CV cv);
        Task UpdateAsync(CV cv);
        Task DeleteAsync(int id);
        Task<IEnumerable<CV>> SearchCVs(Func<CV, bool> predicate);
    }
}
