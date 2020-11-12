using Microsoft.EntityFrameworkCore.Migrations;

namespace ExporterWeb.Migrations
{
    public partial class AddStatusForExporter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "LanguageExporters");

            migrationBuilder.AddColumn<bool>(
                name: "IsShowedOnIndustryPage",
                table: "CommonExporters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CommonExporters",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShowedOnIndustryPage",
                table: "CommonExporters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CommonExporters");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "LanguageExporters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
