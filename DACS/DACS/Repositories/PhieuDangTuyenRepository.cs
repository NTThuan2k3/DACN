using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DACS.Repositories
{
    public class PhieuDangTuyenRepository : IPhieuDangTuyenRepository
    {
        private readonly ApplicationDbContext _context;
        public PhieuDangTuyenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PhieuDangTuyen>> GetAllAsync()
        {
            return await _context.PhieuDangTuyens.Include(p => p.NhaTuyenDungs).Include(p => p.Tinhs).Include(p => p.Statuses).ToListAsync();
        }
        public async Task<PhieuDangTuyen> GetByIdAsync(int id)
        {
            return await _context.PhieuDangTuyens.FindAsync(id);
        }
        public async Task AddAsync(PhieuDangTuyen phieu)
        {
            _context.PhieuDangTuyens.Add(phieu);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(PhieuDangTuyen phieu)
        {
            _context.PhieuDangTuyens.Update(phieu);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var phieu = await _context.PhieuDangTuyens.FindAsync(id);
            _context.PhieuDangTuyens.Remove(phieu);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PhieuDangTuyen>> SearchPhieuDangTuyens(Func<PhieuDangTuyen, bool> predicate)
        {
            return await Task.FromResult(_context.PhieuDangTuyens.Where(predicate).ToList());
        }
    }
}
