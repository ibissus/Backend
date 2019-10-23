using Microsoft.EntityFrameworkCore.Migrations;

namespace KompaniaPchor.Migrations
{
    public partial class PCAssistants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KATALOG_RELATIONS_PLUTON",
                table: "Katalog");

            migrationBuilder.DropForeignKey(
                name: "FK_PROSBA_RELATIONS_PLUTON",
                table: "Prosba");

            migrationBuilder.DropForeignKey(
                name: "FK_Zolnierz_Pluton_NrPlutonu",
                table: "Zolnierz");

            migrationBuilder.DropIndex(
                name: "IX_Zolnierz_NrKompanii",
                table: "Zolnierz");

            migrationBuilder.DropIndex(
                name: "IX_Zolnierz_NrPlutonu",
                table: "Zolnierz");

            migrationBuilder.DropIndex(
                name: "IX_Prosba_NrPlutonu",
                table: "Prosba");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PLUTON",
                table: "Pluton");

            migrationBuilder.DropIndex(
                name: "IX_Katalog_NrPlutonu",
                table: "Katalog");

            migrationBuilder.AddColumn<bool>(
                name: "Funkcyjny",
                table: "Zolnierz",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PLUTON",
                table: "Pluton",
                columns: new[] { "NrPlutonu", "NrKompanii" });

            migrationBuilder.CreateIndex(
                name: "IX_Zolnierz_NrKompanii_NrPlutonu",
                table: "Zolnierz",
                columns: new[] { "NrPlutonu", "NrKompanii" });

            migrationBuilder.CreateIndex(
                name: "IX_Zolnierz_NrPlutonu_NrKompanii",
                table: "Zolnierz",
                columns: new[] { "NrPlutonu", "NrKompanii" });

            migrationBuilder.CreateIndex(
                name: "IX_Prosba_NrPlutonu_NrKompanii",
                table: "Prosba",
                columns: new[] { "NrPlutonu", "NrKompanii" });

            migrationBuilder.CreateIndex(
                name: "IX_Katalog_NrPlutonu_NrKompanii",
                table: "Katalog",
                columns: new[] { "NrPlutonu", "NrKompanii" });

            migrationBuilder.AddForeignKey(
                name: "FK_KATALOG_RELATIONS_PLUTON",
                table: "Katalog",
                columns: new[] { "NrPlutonu", "NrKompanii" },
                principalTable: "Pluton",
                principalColumns: new[] { "NrPlutonu", "NrKompanii" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PROSBA_RELATIONS_PLUTON",
                table: "Prosba",
                columns: new[] { "NrPlutonu", "NrKompanii" },
                principalTable: "Pluton",
                principalColumns: new[] { "NrPlutonu", "NrKompanii" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ZOLNIERZ_REFERENCE_PLUTON",
                table: "Zolnierz",
                columns: new[] { "NrPlutonu", "NrKompanii" },
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KATALOG_RELATIONS_PLUTON",
                table: "Katalog");

            migrationBuilder.DropForeignKey(
                name: "FK_PROSBA_RELATIONS_PLUTON",
                table: "Prosba");

            migrationBuilder.DropForeignKey(
                name: "FK_ZOLNIERZ_REFERENCE_PLUTON",
                table: "Zolnierz");

            migrationBuilder.DropForeignKey(
                name: "FK_Zolnierz_Pluton_NrPlutonu_NrKompanii",
                table: "Zolnierz");

            migrationBuilder.DropIndex(
                name: "IX_Zolnierz_NrKompanii_NrPlutonu",
                table: "Zolnierz");

            migrationBuilder.DropIndex(
                name: "IX_Zolnierz_NrPlutonu_NrKompanii",
                table: "Zolnierz");

            migrationBuilder.DropIndex(
                name: "IX_Prosba_NrPlutonu_NrKompanii",
                table: "Prosba");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PLUTON",
                table: "Pluton");

            migrationBuilder.DropIndex(
                name: "IX_Katalog_NrPlutonu_NrKompanii",
                table: "Katalog");

            migrationBuilder.DropColumn(
                name: "Funkcyjny",
                table: "Zolnierz");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PLUTON",
                table: "Pluton",
                column: "NrPlutonu");

            migrationBuilder.CreateIndex(
                name: "IX_Zolnierz_NrKompanii",
                table: "Zolnierz",
                column: "NrKompanii");

            migrationBuilder.CreateIndex(
                name: "IX_Zolnierz_NrPlutonu",
                table: "Zolnierz",
                column: "NrPlutonu");

            migrationBuilder.CreateIndex(
                name: "IX_Prosba_NrPlutonu",
                table: "Prosba",
                column: "NrPlutonu");

            migrationBuilder.CreateIndex(
                name: "IX_Katalog_NrPlutonu",
                table: "Katalog",
                column: "NrPlutonu");

            migrationBuilder.AddForeignKey(
                name: "FK_KATALOG_RELATIONS_PLUTON",
                table: "Katalog",
                column: "NrPlutonu",
                principalTable: "Pluton",
                principalColumn: "NrPlutonu",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PROSBA_RELATIONS_PLUTON",
                table: "Prosba",
                column: "NrPlutonu",
                principalTable: "Pluton",
                principalColumn: "NrPlutonu",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zolnierz_Pluton_NrPlutonu",
                table: "Zolnierz",
                column: "NrPlutonu",
                principalTable: "Pluton",
                principalColumn: "NrPlutonu",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
