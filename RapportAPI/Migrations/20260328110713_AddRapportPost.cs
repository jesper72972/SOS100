using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapportAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRapportPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ansokningar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FormanId = table.Column<int>(type: "INTEGER", nullable: false),
                    MedarbetarNamn = table.Column<string>(type: "TEXT", nullable: false),
                    Beviljad = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ansokningar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ansokningar_Formaner_FormanId",
                        column: x => x.FormanId,
                        principalTable: "Formaner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RapportPoster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titel = table.Column<string>(type: "TEXT", nullable: false),
                    Avdelning = table.Column<string>(type: "TEXT", nullable: false),
                    TotalKostnad = table.Column<decimal>(type: "TEXT", nullable: false),
                    AntalAktivaFormaner = table.Column<int>(type: "INTEGER", nullable: false),
                    SkapadDatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Kommentar = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportPoster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ansokningar_FormanId",
                table: "Ansokningar",
                column: "FormanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ansokningar");

            migrationBuilder.DropTable(
                name: "RapportPoster");
        }
    }
}
