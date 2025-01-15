using Microsoft.AspNetCore.Identity;
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

            // Seed dữ liệu cho các vai trò
            var adminRoleId = Guid.NewGuid().ToString();
            var customerRoleId = Guid.NewGuid().ToString();
            var employerRoleId = Guid.NewGuid().ToString();
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId, // Tạo ID duy nhất
                    Name = SD.Role_Admin,
                    NormalizedName = SD.Role_Admin.ToUpper()
                },
                new IdentityRole
                {
                    Id = customerRoleId,
                    Name = SD.Role_Customer,
                    NormalizedName = SD.Role_Customer.ToUpper()
                },
                new IdentityRole
                {
                    Id = employerRoleId,
                    Name = SD.Role_Employer,
                    NormalizedName = SD.Role_Employer.ToUpper()
                }
            );

            // Tạo tài khoản admin
            var adminUserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<User>();

            builder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserId,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "Admin1@gmail.com",
                    NormalizedEmail = "ADMIN1@GMAIL.COM",
                    EmailConfirmed = true,
                    FullName = "Quản trị viên",
                    PhoneNumber = "0123456789",
                    CCCD = "012345678999",
                    StatusId = 1,
                    PasswordHash = hasher.HashPassword(null, "Admin1@gmail.com") // Đặt mật khẩu mạnh
                }
            );

            // Gán vai trò admin cho tài khoản
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );

            // Seed dữ liệu cho bảng Tinhs
            builder.Entity<Tinh>().HasData(
                new Tinh { Id = 1, TenTinh = "Hà Nội" },
                new Tinh { Id = 2, TenTinh = "Hà Giang" },
                new Tinh { Id = 3, TenTinh = "Cao Bằng" },
                new Tinh { Id = 4, TenTinh = "Bắc Kạn" },
                new Tinh { Id = 5, TenTinh = "Tuyên Quang" },
                new Tinh { Id = 6, TenTinh = "Lào Cai" },
                new Tinh { Id = 7, TenTinh = "Điện Biên" },
                new Tinh { Id = 8, TenTinh = "Lai Châu" },
                new Tinh { Id = 9, TenTinh = "Sơn La" },
                new Tinh { Id = 10, TenTinh = "Yên Bái" },
                new Tinh { Id = 11, TenTinh = "Hòa Bình" },
                new Tinh { Id = 12, TenTinh = "Thái Nguyên" },
                new Tinh { Id = 13, TenTinh = "Lạng Sơn" },
                new Tinh { Id = 14, TenTinh = "Quảng Ninh" },
                new Tinh { Id = 15, TenTinh = "Bắc Giang" },
                new Tinh { Id = 16, TenTinh = "Phú Thọ" },
                new Tinh { Id = 17, TenTinh = "Vĩnh Phúc" },
                new Tinh { Id = 18, TenTinh = "Bắc Ninh" },
                new Tinh { Id = 19, TenTinh = "Hải Dương" },
                new Tinh { Id = 20, TenTinh = "Hải Phòng" },
                new Tinh { Id = 21, TenTinh = "Hưng Yên" },
                new Tinh { Id = 22, TenTinh = "Thái Bình" },
                new Tinh { Id = 23, TenTinh = "Hà Nam" },
                new Tinh { Id = 24, TenTinh = "Nam Định" },
                new Tinh { Id = 25, TenTinh = "Ninh Bình" },
                new Tinh { Id = 26, TenTinh = "Thanh Hóa" },
                new Tinh { Id = 27, TenTinh = "Nghệ An" },
                new Tinh { Id = 28, TenTinh = "Hà Tĩnh" },
                new Tinh { Id = 29, TenTinh = "Quảng Bình" },
                new Tinh { Id = 30, TenTinh = "Quảng Trị" },
                new Tinh { Id = 31, TenTinh = "Thừa Thiên Huế" },
                new Tinh { Id = 32, TenTinh = "Đà Nẵng" },
                new Tinh { Id = 33, TenTinh = "Quảng Nam" },
                new Tinh { Id = 34, TenTinh = "Quảng Ngãi" },
                new Tinh { Id = 35, TenTinh = "Bình Định" },
                new Tinh { Id = 36, TenTinh = "Phú Yên" },
                new Tinh { Id = 37, TenTinh = "Khánh Hòa" },
                new Tinh { Id = 38, TenTinh = "Ninh Thuận" },
                new Tinh { Id = 39, TenTinh = "Bình Thuận" },
                new Tinh { Id = 40, TenTinh = "Kon Tum" },
                new Tinh { Id = 41, TenTinh = "Gia Lai" },
                new Tinh { Id = 42, TenTinh = "Đắk Lắk" },
                new Tinh { Id = 43, TenTinh = "Đắk Nông" },
                new Tinh { Id = 44, TenTinh = "Lâm Đồng" },
                new Tinh { Id = 45, TenTinh = "Bình Phước" },
                new Tinh { Id = 46, TenTinh = "Tây Ninh" },
                new Tinh { Id = 47, TenTinh = "Bình Dương" },
                new Tinh { Id = 48, TenTinh = "Đồng Nai" },
                new Tinh { Id = 49, TenTinh = "Bà Rịa - Vũng Tàu" },
                new Tinh { Id = 50, TenTinh = "Hồ Chí Minh" },
                new Tinh { Id = 51, TenTinh = "Long An" },
                new Tinh { Id = 52, TenTinh = "Tiền Giang" },
                new Tinh { Id = 53, TenTinh = "Bến Tre" },
                new Tinh { Id = 54, TenTinh = "Trà Vinh" },
                new Tinh { Id = 55, TenTinh = "Vĩnh Long" },
                new Tinh { Id = 56, TenTinh = "Đồng Tháp" },
                new Tinh { Id = 57, TenTinh = "An Giang" },
                new Tinh { Id = 58, TenTinh = "Kiên Giang" },
                new Tinh { Id = 59, TenTinh = "Cần Thơ" },
                new Tinh { Id = 60, TenTinh = "Hậu Giang" },
                new Tinh { Id = 61, TenTinh = "Sóc Trăng" },
                new Tinh { Id = 62, TenTinh = "Bạc Liêu" },
                new Tinh { Id = 63, TenTinh = "Cà Mau" }
            );

        }
        public DbSet<NhaTuyenDung> NhaTuyenDungs { get; set; }
        public DbSet<PhieuDangTuyen> PhieuDangTuyens { get; set; }
        public DbSet<Tinh> Tinhs { get; set; }
        public DbSet<DonUngTuyen> DonUngTuyens { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Meet> Meets { get; set; }
    }
}
