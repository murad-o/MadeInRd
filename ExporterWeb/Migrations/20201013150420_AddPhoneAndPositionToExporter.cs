using Microsoft.EntityFrameworkCore.Migrations;

namespace ExporterWeb.Migrations
{
    public partial class AddPhoneAndPositionToExporter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DirectorSecondName",
                table: "LanguageExporters",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DirectorFirstName",
                table: "LanguageExporters",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "LanguageExporters",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "LanguageExporters",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "LanguageExporters");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "LanguageExporters");

            migrationBuilder.AlterColumn<string>(
                name: "DirectorSecondName",
                table: "LanguageExporters",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DirectorFirstName",
                table: "LanguageExporters",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
