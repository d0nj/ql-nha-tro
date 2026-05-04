using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Migrations
{
    /// <inheritdoc />
    public partial class AddThanhVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThanhViens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HopDongId = table.Column<int>(type: "INTEGER", nullable: false),
                    HoTen = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CCCD = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    SoDienThoai = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    QuanHe = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhViens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThanhViens_HopDongs_HopDongId",
                        column: x => x.HopDongId,
                        principalTable: "HopDongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThanhViens_HopDongId",
                table: "ThanhViens",
                column: "HopDongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThanhViens");
        }
    }
}
