using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class DACN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tinhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTinh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tinhs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileCV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenCV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NhaTuyenDungs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNTD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenCty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiayPhepKinhDoanh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaSoThue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiayChungThuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhId = table.Column<int>(type: "int", nullable: true),
                    DiaDiem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuyMo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnhCty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThongTinCty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageBangTin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinNhanThem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XetDuyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaTuyenDungs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhaTuyenDungs_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NhaTuyenDungs_Tinhs_TinhId",
                        column: x => x.TinhId,
                        principalTable: "Tinhs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NhaTuyenDungs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Time = table.Column<TimeOnly>(type: "time", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YeuCau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NhaTuyenDungId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meets_NhaTuyenDungs_NhaTuyenDungId",
                        column: x => x.NhaTuyenDungId,
                        principalTable: "NhaTuyenDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Meets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PhieuDangTuyens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNganh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MucLuong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamKN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhId = table.Column<int>(type: "int", nullable: false),
                    TenViecLam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChucDanh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DiaDiem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayLapPhieu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HanNopHoSo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NhaTuyenDungId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuDangTuyens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhieuDangTuyens_NhaTuyenDungs_NhaTuyenDungId",
                        column: x => x.NhaTuyenDungId,
                        principalTable: "NhaTuyenDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhieuDangTuyens_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhieuDangTuyens_Tinhs_TinhId",
                        column: x => x.TinhId,
                        principalTable: "Tinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonUngTuyens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileCV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenCV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XetDuyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhieuDangTuyenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonUngTuyens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonUngTuyens_PhieuDangTuyens_PhieuDangTuyenId",
                        column: x => x.PhieuDangTuyenId,
                        principalTable: "PhieuDangTuyens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonUngTuyens_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DonUngTuyens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "98ba419e-c32c-4cc5-aea1-94d31e894890", null, "Admin", "ADMIN" },
                    { "cb7656d5-6c36-4b01-a070-ca78666dd40b", null, "Employer", "EMPLOYER" },
                    { "f9261c4b-48ac-4b49-a6b1-38968b6939d1", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    { 1, true },
                    { 2, false }
                });

            migrationBuilder.InsertData(
                table: "Tinhs",
                columns: new[] { "Id", "TenTinh" },
                values: new object[,]
                {
                    { 1, "Hà Nội" },
                    { 2, "Hà Giang" },
                    { 3, "Cao Bằng" },
                    { 4, "Bắc Kạn" },
                    { 5, "Tuyên Quang" },
                    { 6, "Lào Cai" },
                    { 7, "Điện Biên" },
                    { 8, "Lai Châu" },
                    { 9, "Sơn La" },
                    { 10, "Yên Bái" },
                    { 11, "Hòa Bình" },
                    { 12, "Thái Nguyên" },
                    { 13, "Lạng Sơn" },
                    { 14, "Quảng Ninh" },
                    { 15, "Bắc Giang" },
                    { 16, "Phú Thọ" },
                    { 17, "Vĩnh Phúc" },
                    { 18, "Bắc Ninh" },
                    { 19, "Hải Dương" },
                    { 20, "Hải Phòng" },
                    { 21, "Hưng Yên" },
                    { 22, "Thái Bình" },
                    { 23, "Hà Nam" },
                    { 24, "Nam Định" },
                    { 25, "Ninh Bình" },
                    { 26, "Thanh Hóa" },
                    { 27, "Nghệ An" },
                    { 28, "Hà Tĩnh" },
                    { 29, "Quảng Bình" },
                    { 30, "Quảng Trị" },
                    { 31, "Thừa Thiên Huế" },
                    { 32, "Đà Nẵng" },
                    { 33, "Quảng Nam" },
                    { 34, "Quảng Ngãi" },
                    { 35, "Bình Định" },
                    { 36, "Phú Yên" },
                    { 37, "Khánh Hòa" },
                    { 38, "Ninh Thuận" },
                    { 39, "Bình Thuận" },
                    { 40, "Kon Tum" },
                    { 41, "Gia Lai" },
                    { 42, "Đắk Lắk" },
                    { 43, "Đắk Nông" },
                    { 44, "Lâm Đồng" },
                    { 45, "Bình Phước" },
                    { 46, "Tây Ninh" },
                    { 47, "Bình Dương" },
                    { 48, "Đồng Nai" },
                    { 49, "Bà Rịa - Vũng Tàu" },
                    { 50, "Hồ Chí Minh" },
                    { 51, "Long An" },
                    { 52, "Tiền Giang" },
                    { 53, "Bến Tre" },
                    { 54, "Trà Vinh" },
                    { 55, "Vĩnh Long" },
                    { 56, "Đồng Tháp" },
                    { 57, "An Giang" },
                    { 58, "Kiên Giang" },
                    { 59, "Cần Thơ" },
                    { 60, "Hậu Giang" },
                    { 61, "Sóc Trăng" },
                    { 62, "Bạc Liêu" },
                    { 63, "Cà Mau" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "CCCD", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Image", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StatusId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2cefda62-e239-442f-bab1-e8ac64dcb228", 0, "012345678999", "d88bfd8e-c0be-4131-b86f-02cedd71c85a", "Admin1@gmail.com", true, "Quản trị viên", null, false, null, "ADMIN1@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEARjpPXGA1q7B+gvHrnK9BnACIg0B0fu2OmC5yaNUuBzSCzL7e+vIWkGU9x5WHHAWw==", "0123456789", false, "353d862e-f9c1-4f8f-942c-bfb6c80d14f2", 1, false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "98ba419e-c32c-4cc5-aea1-94d31e894890", "2cefda62-e239-442f-bab1-e8ac64dcb228" });

            migrationBuilder.CreateIndex(
                name: "IX_CVs_UserId",
                table: "CVs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyens_PhieuDangTuyenId",
                table: "DonUngTuyens",
                column: "PhieuDangTuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyens_StatusId",
                table: "DonUngTuyens",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DonUngTuyens_UserId",
                table: "DonUngTuyens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Meets_NhaTuyenDungId",
                table: "Meets",
                column: "NhaTuyenDungId");

            migrationBuilder.CreateIndex(
                name: "IX_Meets_UserId",
                table: "Meets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NhaTuyenDungs_StatusId",
                table: "NhaTuyenDungs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NhaTuyenDungs_TinhId",
                table: "NhaTuyenDungs",
                column: "TinhId");

            migrationBuilder.CreateIndex(
                name: "IX_NhaTuyenDungs_UserId",
                table: "NhaTuyenDungs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDangTuyens_NhaTuyenDungId",
                table: "PhieuDangTuyens",
                column: "NhaTuyenDungId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDangTuyens_StatusId",
                table: "PhieuDangTuyens",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDangTuyens_TinhId",
                table: "PhieuDangTuyens",
                column: "TinhId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatusId",
                table: "Users",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CVs");

            migrationBuilder.DropTable(
                name: "DonUngTuyens");

            migrationBuilder.DropTable(
                name: "Meets");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "PhieuDangTuyens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "NhaTuyenDungs");

            migrationBuilder.DropTable(
                name: "Tinhs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
