using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DACS.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            // Seed dữ liệu cho bảng Statuses
            builder.Entity<Status>().HasData(
                new Status { Id = 1, StatusName = true },
                new Status { Id = 2, StatusName = false }
            );
        }
        public DbSet<NhaTuyenDung> NhaTuyenDungs { get; set; }
        public DbSet<PhieuDangTuyen> PhieuDangTuyens { get; set; }
        public DbSet<Tinh> Tinhs { get; set; }
        public DbSet<DonUngTuyen> DonUngTuyens { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CV> CVs { get; set; }
    }
}
