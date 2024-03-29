﻿// <auto-generated />
using System;
using KompaniaPchor.ORM_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KompaniaPchor.Migrations
{
    [DbContext(typeof(PchorContext))]
    [Migration("20190811202432_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KompaniaPchor.Identity.SystemUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("IdOsoby");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Katalog", b =>
                {
                    b.Property<int>("IdKatalogu");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("NrKompanii");

                    b.HasKey("IdKatalogu")
                        .HasName("PK_KATALOG");

                    b.HasIndex("NrKompanii")
                        .HasName("Relationship_11_FK");

                    b.ToTable("Katalog");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Kompania", b =>
                {
                    b.Property<int>("NrKompanii");

                    b.Property<int>("IdDowodcy");

                    b.HasKey("NrKompanii")
                        .HasName("PK_KOMPANIA");

                    b.HasIndex("IdDowodcy")
                        .HasName("Relationship_7_FK");

                    b.ToTable("Kompania");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Plik", b =>
                {
                    b.Property<int>("IdPliku")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Dodano")
                        .HasColumnType("datetime");

                    b.Property<int?>("IdKatalogu");

                    b.Property<string>("Nazwa")
                        .HasMaxLength(255);

                    b.Property<string>("Opis")
                        .HasMaxLength(255);

                    b.Property<string>("Rozszerzenie")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("IdPliku")
                        .HasName("PK_PLIK");

                    b.HasIndex("IdKatalogu")
                        .HasName("Relationship_9_FK");

                    b.ToTable("Plik");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Pluton", b =>
                {
                    b.Property<int>("NrPlutonu");

                    b.Property<int>("IdDowodcy");

                    b.Property<int>("NrKompanii");

                    b.HasKey("NrPlutonu")
                        .HasName("PK_PLUTON");

                    b.HasIndex("IdDowodcy");

                    b.HasIndex("NrKompanii")
                        .HasName("Idx_Kompania_FK");

                    b.ToTable("Pluton");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Prosba", b =>
                {
                    b.Property<int>("IdProsby")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IdZatwierdzajacego");

                    b.Property<int>("IdZglaszajacego");

                    b.Property<int>("NrKompanii");

                    b.Property<int?>("NrPlutonu");

                    b.Property<bool>("Obsluzona");

                    b.Property<int>("TypProsby");

                    b.HasKey("IdProsby")
                        .HasName("PK_PROSBA");

                    b.HasIndex("IdZatwierdzajacego")
                        .HasName("Relationship_6_FK");

                    b.HasIndex("IdZglaszajacego");

                    b.HasIndex("NrKompanii")
                        .HasName("Relationship_10_FK");

                    b.HasIndex("NrPlutonu");

                    b.ToTable("Prosba");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Zolnierz", b =>
                {
                    b.Property<int>("IdOsoby")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Nazwisko")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int?>("NrKompanii");

                    b.Property<int?>("NrPlutonu");

                    b.Property<string>("Stopień")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("IdOsoby")
                        .HasName("PK_ZOLNIERZ");

                    b.HasIndex("NrKompanii");

                    b.HasIndex("NrPlutonu");

                    b.ToTable("Zolnierz");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Katalog", b =>
                {
                    b.HasOne("KompaniaPchor.ORM_Models.Katalog", "_KatalogNadrzedny")
                        .WithMany("_KatalogiPodrzedne")
                        .HasForeignKey("IdKatalogu")
                        .HasConstraintName("FK_KATALOG_RELATIONS_KATALOGI");

                    b.HasOne("KompaniaPchor.ORM_Models.Kompania", "_Kompania")
                        .WithMany("_Katalogi")
                        .HasForeignKey("NrKompanii")
                        .HasConstraintName("FK_KATALOG_RELATIONS_KOMPANIA");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Kompania", b =>
                {
                    b.HasOne("KompaniaPchor.ORM_Models.Zolnierz", "_Dowodca")
                        .WithMany("_Kompanie")
                        .HasForeignKey("IdDowodcy")
                        .HasConstraintName("FK_KOMPANIA_RELATIONS_ZOLNIERZ")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Plik", b =>
                {
                    b.HasOne("KompaniaPchor.ORM_Models.Katalog", "_Katalog")
                        .WithMany("_Pliki")
                        .HasForeignKey("IdKatalogu")
                        .HasConstraintName("FK_PLIK_RELATIONS_KATALOG");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Pluton", b =>
                {
                    b.HasOne("KompaniaPchor.ORM_Models.Zolnierz", "_Dowodca")
                        .WithMany("_Plutony")
                        .HasForeignKey("IdDowodcy")
                        .HasConstraintName("FK_PLUTON_RELATIONS_ZOLNIERZ");

                    b.HasOne("KompaniaPchor.ORM_Models.Kompania", "_Kompania")
                        .WithMany("_Plutony")
                        .HasForeignKey("NrKompanii")
                        .HasConstraintName("FK_PLUTON_RELATIONS_KOMPANIA")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Prosba", b =>
                {
                    b.HasOne("KompaniaPchor.ORM_Models.Zolnierz", "_Zatwierdzajacy")
                        .WithMany("_ZatwierdzeniaProsb")
                        .HasForeignKey("IdZatwierdzajacego")
                        .HasConstraintName("FK_PROSBA_RELATIONS_ZOLNIERZ");

                    b.HasOne("KompaniaPchor.ORM_Models.Zolnierz", "_Zglaszajacy")
                        .WithMany("_ZgloszoneProsby")
                        .HasForeignKey("IdZglaszajacego")
                        .HasConstraintName("FK_PROSBA_REFERENCE_ZOLNIERZ")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KompaniaPchor.ORM_Models.Kompania", "_Kompania")
                        .WithMany("_Prosby")
                        .HasForeignKey("NrKompanii")
                        .HasConstraintName("FK_PROSBA_RELATIONS_KOMPANIA");

                    b.HasOne("KompaniaPchor.ORM_Models.Pluton", "_Pluton")
                        .WithMany("_Prosby")
                        .HasForeignKey("NrPlutonu")
                        .HasConstraintName("FK_PROSBA_RELATIONS_PLUTON");
                });

            modelBuilder.Entity("KompaniaPchor.ORM_Models.Zolnierz", b =>
                {
                    b.HasOne("KompaniaPchor.ORM_Models.Kompania", "_Kompania")
                        .WithMany("_Zolnierze")
                        .HasForeignKey("NrKompanii")
                        .HasConstraintName("FK_ZOLNIERZ_REFERENCE_KOMPANIA");

                    b.HasOne("KompaniaPchor.ORM_Models.Pluton", "_Pluton")
                        .WithMany()
                        .HasForeignKey("NrPlutonu");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("KompaniaPchor.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("KompaniaPchor.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KompaniaPchor.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("KompaniaPchor.Identity.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
