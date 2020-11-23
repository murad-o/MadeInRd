using Microsoft.EntityFrameworkCore.Migrations;

namespace ExporterWeb.Migrations
{
    public partial class AddOrderFieldToIndustry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "IndustryTranslations");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Industries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Industries");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "IndustryTranslations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
