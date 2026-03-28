using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapportAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedAndModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AntalAktivaFormaner",
                table: "RapportPoster");

            migrationBuilder.DropColumn(
                name: "Avdelning",
                table: "RapportPoster");

            migrationBuilder.RenameColumn(
                name: "TotalKostnad",
                table: "RapportPoster",
                newName: "SkapadAv");

            migrationBuilder.RenameColumn(
                name: "SkapadDatum",
                table: "RapportPoster",
                newName: "Skapad");

            migrationBuilder.RenameColumn(
                name: "Kommentar",
                table: "RapportPoster",
                newName: "Beskrivning");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SkapadAv",
                table: "RapportPoster",
                newName: "TotalKostnad");

            migrationBuilder.RenameColumn(
                name: "Skapad",
                table: "RapportPoster",
                newName: "SkapadDatum");

            migrationBuilder.RenameColumn(
                name: "Beskrivning",
                table: "RapportPoster",
                newName: "Kommentar");

            migrationBuilder.AddColumn<int>(
                name: "AntalAktivaFormaner",
                table: "RapportPoster",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Avdelning",
                table: "RapportPoster",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
