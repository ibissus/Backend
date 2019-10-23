using Microsoft.EntityFrameworkCore.Migrations;

namespace KompaniaPchor.Migrations
{
    public partial class RootFolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdKataloguNadrzednego",
                table: "Katalog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "Relationship_KATALOG",
                table: "Katalog",
                column: "IdKataloguNadrzednego");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Relationship_KATALOG",
                table: "Katalog");

            migrationBuilder.DropColumn(
                name: "IdKataloguNadrzednego",
                table: "Katalog");
        }
    }
}
