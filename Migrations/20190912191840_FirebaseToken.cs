using Microsoft.EntityFrameworkCore.Migrations;

namespace KompaniaPchor.Migrations
{
    public partial class FirebaseToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirebaseToken",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KATALOG_RELATIONS_KATALOGI",
                table: "Katalog",
                column: "IdKataloguNadrzednego",
                principalTable: "Katalog",
                principalColumn: "IdKatalogu",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KATALOG_RELATIONS_KATALOGI",
                table: "Katalog");

            migrationBuilder.DropColumn(
                name: "FirebaseToken",
                table: "AspNetUsers");
        }
    }
}
