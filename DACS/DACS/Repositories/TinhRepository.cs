using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DACS.Repositories
{
    public class TinhRepository : ITinhRepository
    {
        private readonly ApplicationDbContext _context;
        public TinhRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Tinh>> GetAllAsync()
        {
            return await _context.Tinhs.ToListAsync();
        }
        public async Task<Tinh> GetByIdAsync(int id)
        {
            return await _context.Tinhs.FindAsync(id);
        }
        public async Task AddAsync(Tinh tinh)
        {
            _context.Tinhs.Add(tinh);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Tinh tinh)
        {
            _context.Tinhs.Update(tinh);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var tinh = await _context.Tinhs.FindAsync(id);
            _context.Tinhs.Remove(tinh);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tinh>> SearchTinhs(Func<Tinh, bool> predicate)
        {
            return await Task.FromResult(_context.Tinhs.Where(predicate).ToList());
        }
    }
}
