using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DACS.Repositories
{
    public class DonUngTuyenRepository : IDonUngTuyenRepository
    {
        private readonly ApplicationDbContext _context;
        public DonUngTuyenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DonUngTuyen>> GetAllAsync()
        {
            return await _context.DonUngTuyens.Include(p => p.Statuses).Include(p => p.Users).Include(p => p.PhieuDangTuyens).ToListAsync();
        }
        public async Task<DonUngTuyen> GetByIdAsync(int id)
        {
            return await _context.DonUngTuyens.FindAsync(id);
        }
        public async Task AddAsync(DonUngTuyen don)
        {
            _context.DonUngTuyens.Add(don);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DonUngTuyen don)
        {
            _context.DonUngTuyens.Update(don);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var don = await _context.DonUngTuyens.FindAsync(id);
            _context.DonUngTuyens.Remove(don);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DonUngTuyen>> SearchDonUngTuyens(Func<DonUngTuyen, bool> predicate)
        {
            return await Task.FromResult(_context.DonUngTuyens.Where(predicate).ToList());
        }
    }
}
