using DACS.Models;

namespace DACS.Repositories
{
    public interface IMeetRepository
    {
        Task<IEnumerable<Meet>> GetAllAsync();
        Task<Meet> GetByIdAsync(int id);
        Task AddAsync(Meet met);
        Task UpdateAsync(Meet met);
        Task DeleteAsync(int id);
        Task<IEnumerable<Meet>> SearchMeets(Func<Meet, bool> predicate);
    }
}
