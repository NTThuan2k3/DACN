using DACS.Models;

namespace DACS.Repositories
{
    public interface INhaTuyenDungRepository
    {
        Task<IEnumerable<NhaTuyenDung>> GetAllAsync();
        Task<NhaTuyenDung> GetByIdAsync(int id);
        Task AddAsync(NhaTuyenDung ntd);
        Task UpdateAsync(NhaTuyenDung ntd);
        Task DeleteAsync(int id);
        Task<IEnumerable<NhaTuyenDung>> SearchNhaTuyenDungs(Func<NhaTuyenDung, bool> predicate);
    }
}
