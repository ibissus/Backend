using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KompaniaPchor.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IdOsoby = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plik",
                columns: table => new
                {
                    IdPliku = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdKatalogu = table.Column<int>(nullable: true),
                    Rozszerzenie = table.Column<string>(maxLength: 255, nullable: false),
                    Opis = table.Column<string>(maxLength: 255, nullable: true),
                    Dodano = table.Column<DateTime>(type: "datetime", nullable: false),
                    Naglowek = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLIK", x => x.IdPliku);
                });

            migrationBuilder.CreateTable(
                name: "Katalog",
                columns: table => new
                {
                    IdKatalogu = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NrKompanii = table.Column<int>(nullable: false),
                    Nazwa = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KATALOG", x => x.IdKatalogu);
                });

            migrationBuilder.CreateTable(
                name: "Pluton",
                columns: table => new
                {
                    NrPlutonu = table.Column<int>(nullable: false),
                    NrKompanii = table.Column<int>(nullable: false),
                    IdDowodcy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLUTON", x => x.NrPlutonu);
                });

            migrationBuilder.CreateTable(
                name: "Prosba",
                columns: table => new
                {
                    IdProsby = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NrKompanii = table.Column<int>(nullable: false),
                    NrPlutonu = table.Column<int>(nullable: true),
                    IdZatwierdzajacego = table.Column<int>(nullable: true),
                    IdZglaszajacego = table.Column<int>(nullable: false),
                    Obsluzona = table.Column<bool>(nullable: false),
                    TypProsby = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROSBA", x => x.IdProsby);
                    table.ForeignKey(
                        name: "FK_PROSBA_RELATIONS_PLUTON",
                        column: x => x.NrPlutonu,
                        principalTable: "Pluton",
                        principalColumn: "NrPlutonu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zolnierz",
                columns: table => new
                {
                    IdOsoby = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NrKompanii = table.Column<int>(nullable: true),
                    NrPlutonu = table.Column<int>(nullable: true),
                    Imie = table.Column<string>(maxLength: 255, nullable: false),
                    Nazwisko = table.Column<string>(maxLength: 255, nullable: false),
                    Stopień = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZOLNIERZ", x => x.IdOsoby);
                    table.ForeignKey(
                        name: "FK_Zolnierz_Pluton_NrPlutonu",
                        column: x => x.NrPlutonu,
                        principalTable: "Pluton",
                        principalColumn: "NrPlutonu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kompania",
                columns: table => new
                {
                    NrKompanii = table.Column<int>(nullable: false),
                    IdDowodcy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOMPANIA", x => x.NrKompanii);
                    table.ForeignKey(
                        name: "FK_KOMPANIA_RELATIONS_ZOLNIERZ",
                        column: x => x.IdDowodcy,
                        principalTable: "Zolnierz",
                        principalColumn: "IdOsoby",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Relationship_11_FK",
                table: "Katalog",
                column: "NrKompanii");

            migrationBuilder.CreateIndex(
                name: "Relationship_7_FK",
                table: "Kompania",
                column: "IdDowodcy");

            migrationBuilder.CreateIndex(
                name: "Relationship_9_FK",
                table: "Plik",
                column: "IdKatalogu");

            migrationBuilder.CreateIndex(
                name: "IX_Pluton_IdDowodcy",
                table: "Pluton",
                column: "IdDowodcy");

            migrationBuilder.CreateIndex(
                name: "Idx_Kompania_FK",
                table: "Pluton",
                column: "NrKompanii");

            migrationBuilder.CreateIndex(
                name: "Relationship_6_FK",
                table: "Prosba",
                column: "IdZatwierdzajacego");

            migrationBuilder.CreateIndex(
                name: "IX_Prosba_IdZglaszajacego",
                table: "Prosba",
                column: "IdZglaszajacego");

            migrationBuilder.CreateIndex(
                name: "Relationship_10_FK",
                table: "Prosba",
                column: "NrKompanii");

            migrationBuilder.CreateIndex(
                name: "IX_Prosba_NrPlutonu",
                table: "Prosba",
                column: "NrPlutonu");

            migrationBuilder.CreateIndex(
                name: "IX_Zolnierz_NrKompanii",
                table: "Zolnierz",
                column: "NrKompanii");

            migrationBuilder.CreateIndex(
                name: "IX_Zolnierz_NrPlutonu",
                table: "Zolnierz",
                column: "NrPlutonu");

            migrationBuilder.AddForeignKey(
                name: "FK_PLIK_RELATIONS_KATALOG",
                table: "Plik",
                column: "IdKatalogu",
                principalTable: "Katalog",
                principalColumn: "IdKatalogu",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KATALOG_RELATIONS_KOMPANIA",
                table: "Katalog",
                column: "NrKompanii",
                principalTable: "Kompania",
                principalColumn: "NrKompanii",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PLUTON_RELATIONS_KOMPANIA",
                table: "Pluton",
                column: "NrKompanii",
                principalTable: "Kompania",
                principalColumn: "NrKompanii",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PLUTON_RELATIONS_ZOLNIERZ",
                table: "Pluton",
                column: "IdDowodcy",
                principalTable: "Zolnierz",
                principalColumn: "IdOsoby",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PROSBA_RELATIONS_KOMPANIA",
                table: "Prosba",
                column: "NrKompanii",
                principalTable: "Kompania",
                principalColumn: "NrKompanii",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PROSBA_RELATIONS_ZOLNIERZ",
                table: "Prosba",
                column: "IdZatwierdzajacego",
                principalTable: "Zolnierz",
                principalColumn: "IdOsoby",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PROSBA_REFERENCE_ZOLNIERZ",
                table: "Prosba",
                column: "IdZglaszajacego",
                principalTable: "Zolnierz",
                principalColumn: "IdOsoby",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZOLNIERZ_REFERENCE_KOMPANIA",
                table: "Zolnierz",
                column: "NrKompanii",
                principalTable: "Kompania",
                principalColumn: "NrKompanii",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PLUTON_RELATIONS_KOMPANIA",
                table: "Pluton");

            migrationBuilder.DropForeignKey(
                name: "FK_ZOLNIERZ_REFERENCE_KOMPANIA",
                table: "Zolnierz");

            migrationBuilder.DropForeignKey(
                name: "FK_PLUTON_RELATIONS_ZOLNIERZ",
                table: "Pluton");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Plik");

            migrationBuilder.DropTable(
                name: "Prosba");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Katalog");

            migrationBuilder.DropTable(
                name: "Kompania");

            migrationBuilder.DropTable(
                name: "Zolnierz");

            migrationBuilder.DropTable(
                name: "Pluton");
        }
    }
}
