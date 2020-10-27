using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ExporterWeb.Migrations
{
    public partial class ReplaceFieldOfActivityWithIndustry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonExporters_FieldsOfActivity_FieldOfActivityId",
                table: "CommonExporters");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_FieldsOfActivity_FieldOfActivityId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "FieldsOfActivity");

            migrationBuilder.DropIndex(
                name: "IX_Products_FieldOfActivityId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_CommonExporters_FieldOfActivityId",
                table: "CommonExporters");

            migrationBuilder.DropColumn(
                name: "FieldOfActivityId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FieldOfActivityId",
                table: "CommonExporters");

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "CommonExporters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndustryTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    IndustryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndustryTranslations_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_IndustryId",
                table: "Products",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonExporters_IndustryId",
                table: "CommonExporters",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryTranslations_IndustryId",
                table: "IndustryTranslations",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonExporters_Industries_IndustryId",
                table: "CommonExporters",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Industries_IndustryId",
                table: "Products",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonExporters_Industries_IndustryId",
                table: "CommonExporters");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Industries_IndustryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "IndustryTranslations");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropIndex(
                name: "IX_Products_IndustryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_CommonExporters_IndustryId",
                table: "CommonExporters");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "CommonExporters");

            migrationBuilder.AddColumn<int>(
                name: "FieldOfActivityId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FieldOfActivityId",
                table: "CommonExporters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FieldsOfActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldsOfActivity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_FieldOfActivityId",
                table: "Products",
                column: "FieldOfActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonExporters_FieldOfActivityId",
                table: "CommonExporters",
                column: "FieldOfActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonExporters_FieldsOfActivity_FieldOfActivityId",
                table: "CommonExporters",
                column: "FieldOfActivityId",
                principalTable: "FieldsOfActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FieldsOfActivity_FieldOfActivityId",
                table: "Products",
                column: "FieldOfActivityId",
                principalTable: "FieldsOfActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
