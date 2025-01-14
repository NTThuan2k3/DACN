using DACS.Models;
using Microsoft.EntityFrameworkCore;

namespace DACS.Repositories
{
    public class CvRepository : ICvRepository
    {
        private readonly ApplicationDbContext _context;
        public CvRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CV>> GetAllAsync()
        {
            return await _context.CVs.Include(p => p.Users).ToListAsync();
        }
        public async Task<CV> GetByIdAsync(int id)
        {
            return await _context.CVs.FindAsync(id);
        }
        public async Task AddAsync(CV cv)
        {
            _context.CVs.Add(cv);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(CV cv)
        {
            _context.CVs.Update(cv);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var cv = await _context.CVs.FindAsync(id);
            _context.CVs.Remove(cv);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CV>> SearchCVs(Func<CV, bool> predicate)
        {
            return await Task.FromResult(_context.CVs.Where(predicate).ToList());
        }
    }
}
