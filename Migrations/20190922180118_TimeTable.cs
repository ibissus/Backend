using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KompaniaPchor.Migrations
{
    public partial class TimeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NrKompanii",
                table: "Plik",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdPlanuZajec",
                table: "Kompania",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plik_NrKompanii",
                table: "Plik",
                column: "NrKompanii",
                unique: true,
                filter: "[NrKompanii] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_KOMPANIA_RELATIONS_TIMETABLE",
                table: "Plik",
                column: "NrKompanii",
                principalTable: "Kompania",
                principalColumn: "NrKompanii",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KOMPANIA_RELATIONS_TIMETABLE",
                table: "Plik");

            migrationBuilder.DropIndex(
                name: "IX_Plik_NrKompanii",
                table: "Plik");

            migrationBuilder.DropColumn(
                name: "NrKompanii",
                table: "Plik");

            migrationBuilder.DropColumn(
                name: "IdPlanuZajec",
                table: "Kompania");
        }
    }
}
