using Microsoft.EntityFrameworkCore.Migrations;

namespace KompaniaPchor.Migrations
{
    public partial class RequestsReflection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZOLNIERZ_REFERENCE_PLUTON",
                table: "Zolnierz");

            migrationBuilder.DropForeignKey(
                name: "FK_Zolnierz_Pluton_NrPlutonu_NrKompanii",
                table: "Zolnierz");

            migrationBuilder.DropIndex(
                name: "IX_Zolnierz_NrKompanii_NrPlutonu",
                table: "Zolnierz");

            migrationBuilder.CreateIndex(
                name: "IX_Zolnierz_NrKompanii",
                table: "Zolnierz",
                column: "NrKompanii");

            migrationBuilder.AddForeignKey(
                name: "FK_ZOLNIERZ_REFERENCE_PLUTON",
                table: "Zolnierz",
                columns: new[] { "NrPlutonu", "NrKompanii" },
                principalTable: "Pluton",
                principalColumns: new[] { "NrPlutonu", "NrKompanii" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZOLNIERZ_REFERENCE_PLUTON",
                table: "Zolnierz");

            migrationBuilder.DropIndex(
                name: "IX_Zolnierz_NrKompanii",
                table: "Zolnierz");

            migrationBuilder.CreateIndex(
                name: "IX_Zolnierz_NrKompanii_NrPlutonu",
                table: "Zolnierz",
                columns: new[] { "NrKompanii", "NrPlutonu" });

            migrationBuilder.AddForeignKey(
                name: "FK_ZOLNIERZ_REFERENCE_PLUTON",
                table: "Zolnierz",
                columns: new[] { "NrKompanii", "NrPlutonu" },
                principalTable: "Pluton",
                principalColumns: new[] { "NrPlutonu", "NrKompanii" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zolnierz_Pluton_NrPlutonu_NrKompanii",
                table: "Zolnierz",
                columns: new[] { "NrPlutonu", "NrKompanii" },
                principalTable: "Pluton",
                principalColumns: new[] { "NrPlutonu", "NrKompanii" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
