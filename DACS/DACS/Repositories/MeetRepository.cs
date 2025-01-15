using DACS.Models;
using Microsoft.EntityFrameworkCore;

namespace DACS.Repositories
{
    public class MeetRepository : IMeetRepository
    {
        private readonly ApplicationDbContext _context;
        public MeetRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Meet>> GetAllAsync()
        {
            return await _context.Meets.Include(p => p.NhaTuyenDungs).ToListAsync();
        }
        public async Task<Meet> GetByIdAsync(int id)
        {
            return await _context.Meets.FindAsync(id);
        }
        public async Task AddAsync(Meet met)
        {
            _context.Meets.Add(met);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Meet met)
        {
            _context.Meets.Update(met);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var met = await _context.Meets.FindAsync(id);
            _context.Meets.Remove(met);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Meet>> SearchMeets(Func<Meet, bool> predicate)
        {
            return await Task.FromResult(_context.Meets.Where(predicate).ToList());
        }
    }
}
