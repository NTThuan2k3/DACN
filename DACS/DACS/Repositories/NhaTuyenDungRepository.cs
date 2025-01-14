using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DACS.Repositories
{
    public class NhaTuyenDungRepository : INhaTuyenDungRepository
    {
        private readonly ApplicationDbContext _context;
        public NhaTuyenDungRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<NhaTuyenDung>> GetAllAsync()
        {
            return await _context.NhaTuyenDungs.Include(p => p.Tinhs).Include(p => p.Users).Include(p => p.Statuses).ToListAsync();
        }
        public async Task<NhaTuyenDung> GetByIdAsync(int id)
        {
            return await _context.NhaTuyenDungs.FindAsync(id);
        }
        public async Task AddAsync(NhaTuyenDung ntd)
        {
            _context.NhaTuyenDungs.Add(ntd);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(NhaTuyenDung ntd)
        {
            _context.NhaTuyenDungs.Update(ntd);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var ntd = await _context.NhaTuyenDungs.FindAsync(id);
            _context.NhaTuyenDungs.Remove(ntd);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NhaTuyenDung>> SearchNhaTuyenDungs(Func<NhaTuyenDung, bool> predicate)
        {
            return await Task.FromResult(_context.NhaTuyenDungs.Where(predicate).ToList());
        }
    }
}
