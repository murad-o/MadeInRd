using Microsoft.EntityFrameworkCore.Migrations;

namespace ExporterWeb.Migrations
{
    public partial class CreateExporters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommonExporters",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    INN = table.Column<string>(maxLength: 12, nullable: false),
                    OGRN_IP = table.Column<string>(maxLength: 15, nullable: false),
                    LogoPath = table.Column<string>(nullable: true),
                    FieldOfActivityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonExporters", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CommonExporters_FieldsOfActivity_FieldOfActivityId",
                        column: x => x.FieldOfActivityId,
                        principalTable: "FieldsOfActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommonExporters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageExporters",
                columns: table => new
                {
                    CommonExporterId = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ContactPersonFirstName = table.Column<string>(nullable: false),
                    ContactPersonSecondName = table.Column<string>(nullable: false),
                    ContactPersonPatronymic = table.Column<string>(nullable: true),
                    DirectorFirstName = table.Column<string>(nullable: false),
                    DirectorSecondName = table.Column<string>(nullable: false),
                    DirectorPatronymic = table.Column<string>(nullable: true),
                    WorkingTime = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Approved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageExporters", x => new { x.CommonExporterId, x.Language });
                    table.ForeignKey(
                        name: "FK_LanguageExporters_CommonExporters_CommonExporterId",
                        column: x => x.CommonExporterId,
                        principalTable: "CommonExporters",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonExporters_FieldOfActivityId",
                table: "CommonExporters",
                column: "FieldOfActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageExporters");

            migrationBuilder.DropTable(
                name: "CommonExporters");
        }
    }
}
