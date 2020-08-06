using Microsoft.EntityFrameworkCore.Migrations;

namespace ExporterWeb.Migrations
{
    public partial class AddLogoToExporter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "CommonExporters");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "LanguageExporters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "LanguageExporters");

            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "CommonExporters",
                type: "text",
                nullable: true);
        }
    }
}
