using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaiDats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenNhaTro = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DiaChi = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    SoDienThoai = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    GiaDien = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    GiaNuoc = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    PhiDichVu = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaiDats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KhachThues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoTen = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CCCD = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    SoDienThoai = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    QueQuan = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    NgheNghiep = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GioiTinh = table.Column<int>(type: "INTEGER", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachThues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaPhong = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TenPhong = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    GiaThue = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    DienTich = table.Column<double>(type: "REAL", nullable: false),
                    SoNguoiToiDa = table.Column<int>(type: "INTEGER", nullable: false),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: false),
                    MoTa = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phongs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChiSoDienNuocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhongId = table.Column<int>(type: "INTEGER", nullable: false),
                    Thang = table.Column<int>(type: "INTEGER", nullable: false),
                    Nam = table.Column<int>(type: "INTEGER", nullable: false),
                    ChiSoDienCu = table.Column<int>(type: "INTEGER", nullable: false),
                    ChiSoDienMoi = table.Column<int>(type: "INTEGER", nullable: false),
                    ChiSoNuocCu = table.Column<int>(type: "INTEGER", nullable: false),
                    ChiSoNuocMoi = table.Column<int>(type: "INTEGER", nullable: false),
                    NgayGhi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiSoDienNuocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiSoDienNuocs_Phongs_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HopDongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhongId = table.Column<int>(type: "INTEGER", nullable: false),
                    KhachThueId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaHopDong = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TienCoc = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    GiaThueThucTe = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopDongs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HopDongs_KhachThues_KhachThueId",
                        column: x => x.KhachThueId,
                        principalTable: "KhachThues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HopDongs_Phongs_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HopDongId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaHoaDon = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Thang = table.Column<int>(type: "INTEGER", nullable: false),
                    Nam = table.Column<int>(type: "INTEGER", nullable: false),
                    TienPhong = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TienDien = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TienNuoc = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    PhiDichVu = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoaDons_HopDongs_HopDongId",
                        column: x => x.HopDongId,
                        principalTable: "HopDongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CaiDats",
                columns: new[] { "Id", "DiaChi", "GiaDien", "GiaNuoc", "NgayCapNhat", "PhiDichVu", "SoDienThoai", "TenNhaTro" },
                values: new object[] { 1, null, 3500m, 15000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100000m, null, "Nhà Trọ" });

            migrationBuilder.CreateIndex(
                name: "IX_ChiSoDienNuocs_PhongId_Thang_Nam",
                table: "ChiSoDienNuocs",
                columns: new[] { "PhongId", "Thang", "Nam" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_HopDongId",
                table: "HoaDons",
                column: "HopDongId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_MaHoaDon",
                table: "HoaDons",
                column: "MaHoaDon",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HopDongs_KhachThueId",
                table: "HopDongs",
                column: "KhachThueId");

            migrationBuilder.CreateIndex(
                name: "IX_HopDongs_MaHopDong",
                table: "HopDongs",
                column: "MaHopDong",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HopDongs_PhongId",
                table: "HopDongs",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_KhachThues_CCCD",
                table: "KhachThues",
                column: "CCCD",
                unique: true,
                filter: "[CCCD] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Phongs_MaPhong",
                table: "Phongs",
                column: "MaPhong",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaiDats");

            migrationBuilder.DropTable(
                name: "ChiSoDienNuocs");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "HopDongs");

            migrationBuilder.DropTable(
                name: "KhachThues");

            migrationBuilder.DropTable(
                name: "Phongs");
        }
    }
}
