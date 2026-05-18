using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLNhaTro.Migrations
{
    /// <inheritdoc />
    public partial class AddThemeMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThemeMode",
                table: "CaiDats",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "light");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeMode",
                table: "CaiDats");
        }
    }
}
