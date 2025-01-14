using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DACS.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ApplicationDbContext _context;
        //private readonly List<Status> _status;
        public StatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _context.Statuses.ToListAsync();
        }
        public async Task<Status> GetByIdAsync(int id)
        {
            return await _context.Statuses.FindAsync(id);
        }
        public async Task AddAsync(Status status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Status status)
        {
            _context.Statuses.Update(status);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
        }
    }
}