using DACS.Models;

namespace DACS.Repositories
{
    public interface ITinhRepository
    {
        Task<IEnumerable<Tinh>> GetAllAsync();
        Task<Tinh> GetByIdAsync(int id);
        Task AddAsync(Tinh tinh);
        Task UpdateAsync(Tinh tinh);
        Task DeleteAsync(int id);
        Task<IEnumerable<Tinh>> SearchTinhs(Func<Tinh, bool> predicate);
    }
}
