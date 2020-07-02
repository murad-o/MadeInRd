using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExporterWeb.Migrations
{
    public partial class CreateProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    LanguageExporterId = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    FieldOfActivityId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_FieldsOfActivity_FieldOfActivityId",
                        column: x => x.FieldOfActivityId,
                        principalTable: "FieldsOfActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_LanguageExporters_LanguageExporterId_Language",
                        columns: x => new { x.LanguageExporterId, x.Language },
                        principalTable: "LanguageExporters",
                        principalColumns: new[] { "CommonExporterId", "Language" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_FieldOfActivityId",
                table: "Products",
                column: "FieldOfActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_LanguageExporterId_Language",
                table: "Products",
                columns: new[] { "LanguageExporterId", "Language" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
