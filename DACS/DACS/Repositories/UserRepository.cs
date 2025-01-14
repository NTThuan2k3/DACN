using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DACS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Include(p => p.Statuses).ToListAsync();
        }
        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task AddAsync(User Users)
        {
            _context.Users.Add(Users);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User Users)
        {
            _context.Users.Update(Users);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> SearchUsers(Func<User, bool> predicate)
        {
            return await Task.FromResult(_context.Users.Where(predicate).ToList());
        }
    }
}
