using Microsoft.EntityFrameworkCore.Migrations;

namespace KompaniaPchor.Migrations
{
    public partial class RemoveRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stopień",
                table: "Zolnierz");

            migrationBuilder.AlterColumn<int>(
                name: "IdDowodcy",
                table: "Pluton",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Stopień",
                table: "Zolnierz",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "IdDowodcy",
                table: "Pluton",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
