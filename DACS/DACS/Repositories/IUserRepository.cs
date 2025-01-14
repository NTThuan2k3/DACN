using DACS.Models;

namespace DACS.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
        Task<IEnumerable<User>> SearchUsers(Func<User, bool> predicate);
    }
}
