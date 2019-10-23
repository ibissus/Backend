using Microsoft.EntityFrameworkCore.Migrations;

namespace KompaniaPchor.Migrations
{
    public partial class PlatoonsFolders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NrPlutonu",
                table: "Katalog",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KATALOG_RELATIONS_PLUTON",
                table: "Katalog");

            migrationBuilder.DropIndex(
                name: "IX_Katalog_NrPlutonu",
                table: "Katalog");

            migrationBuilder.DropColumn(
                name: "NrPlutonu",
                table: "Katalog");
        }
    }
}
