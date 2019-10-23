using System;
using KompaniaPchor.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KompaniaPchor.ORM_Models
{
    public partial class PchorContext : IdentityDbContext<SystemUser>
    {
        /*
            DbContext.Configuration.ProxyCreationEnabled = true;    
            DbContext.Configuration.LazyLoadingEnabled = true;  
             */
        public PchorContext()
        {
        }

        public PchorContext(DbContextOptions<PchorContext> options) : base(options)
        {
        }

        public virtual DbSet<Katalog> Katalog { get; set; }
        public virtual DbSet<Kompania> Kompania { get; set; }
        public virtual DbSet<Pluton> Pluton { get; set; }
        public virtual DbSet<Plik> Plik { get; set; }
        public virtual DbSet<Prosba> Prosba { get; set; }
        public virtual DbSet<Zolnierz> Zolnierz { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Katalog>(entity =>
            {
                entity.HasKey(e => e.IdKatalogu)
                    .HasName("PK_KATALOG");

                entity.Property(e => e.IdKatalogu)
                    .IsRequired()
                    .UseSqlServerIdentityColumn();

                entity.HasIndex(e => e.NrKompanii)
                    .HasName("Relationship_11_FK");

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasIndex(e => e.IdKataloguNadrzednego)
                    .HasName("Relationship_KATALOG");

                entity.HasOne(d => d._KatalogNadrzedny)
                .WithMany(p => p._KatalogiPodrzedne)
                .HasForeignKey(d => d.IdKataloguNadrzednego)
                .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KATALOG_RELATIONS_KATALOG");

                entity.HasOne(d => d._Kompania)
                    .WithMany(p => p._Katalogi)
                    .HasForeignKey(d => d.NrKompanii)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KATALOG_RELATIONS_KOMPANIA");

                entity.HasOne(d => d._Pluton)
                   .WithMany(p => p._Katalogi)
                   .HasForeignKey(e => new { e.NrPlutonu, e.NrKompanii })
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_KATALOG_RELATIONS_PLUTON");

                entity.HasOne(d => d._KatalogNadrzedny)
                    .WithMany(p => p._KatalogiPodrzedne)
                    .HasForeignKey(d => d.IdKatalogu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KATALOG_RELATIONS_KATALOG");

                entity.HasMany(d => d._KatalogiPodrzedne)
                    .WithOne(p => p._KatalogNadrzedny)
                    .HasForeignKey(d=>d.IdKataloguNadrzednego)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KATALOG_RELATIONS_KATALOGI");

                entity.HasMany(d => d._Pliki)
                    .WithOne(d => d._Katalog)
                    .HasForeignKey(d => d.IdKatalogu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KATALOG_RELATIONS_PLIK");
            });

            modelBuilder.Entity<Kompania>(entity =>
            {
                entity.HasKey(e => e.NrKompanii)
                    .HasName("PK_KOMPANIA");

                entity.HasIndex(e => e.IdDowodcy)
                    .HasName("Relationship_7_FK");

                entity.Property(e => e.NrKompanii).ValueGeneratedNever();

                entity.HasOne(d => d._Dowodca)
                    .WithMany(p => p._Kompanie)
                    .HasForeignKey(d => d.IdDowodcy)
                    .HasConstraintName("FK_KOMPANIA_RELATIONS_ZOLNIERZ");

                entity.HasMany(d => d._Prosby)
                    .WithOne(p => p._Kompania)
                    .HasForeignKey(d => d.IdProsby)
                    .HasConstraintName("FK_KOMPANIA_RELATIONS_Prosba");

                entity.HasOne(d => d._PlanZajec)
                    .WithOne(p => p._Kompania)
                    .HasForeignKey<Plik>(d => d.NrKompanii)
                    .HasConstraintName("FK_KOMPANIA_RELATIONS_TIMETABLE");
            });

            modelBuilder.Entity<Pluton>(entity =>
            {
                entity.HasKey(e => new { e.NrPlutonu, e.NrKompanii })
                    .HasName("PK_PLUTON");

                entity.HasIndex(e => e.NrKompanii)
                    .HasName("Idx_Kompania_FK");

                entity.Property(e => e.NrPlutonu).ValueGeneratedNever();

                entity.HasOne(d => d._Kompania)
                   .WithMany(p => p._Plutony)
                   .HasForeignKey(d => d.NrKompanii)
                   .HasConstraintName("FK_PLUTON_RELATIONS_KOMPANIA");

                entity.HasOne(d => d._Dowodca)
                    .WithMany(p => p._Plutony)
                    .HasForeignKey(d => d.IdDowodcy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PLUTON_RELATIONS_ZOLNIERZ");

                entity.HasMany(d => d._Prosby)
                    .WithOne(p => p._Pluton)
                    .HasForeignKey(d => d.IdProsby)
                    .HasConstraintName("FK_PLATOON_RELATIONS_Prosba");
            });

            modelBuilder.Entity<Plik>(entity =>
            {
                entity.HasKey(e => e.IdPliku)
                    .HasName("PK_PLIK");

                entity.Property(e => e.IdPliku)
                .IsRequired();//.UseSqlServerIdentityColumn();

                entity.HasIndex(e => e.IdKatalogu)
                    .HasName("Relationship_9_FK");

                entity.Property(e => e.Dodano).HasColumnType("datetime");

                entity.Property(e => e.Opis).HasMaxLength(255);

                entity.Property(e => e.Rozszerzenie)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d._Katalog)
                    .WithMany(p => p._Pliki)
                    .HasForeignKey(d => d.IdKatalogu)
                    .HasConstraintName("FK_PLIK_RELATIONS_KATALOG");
            });

            modelBuilder.Entity<Prosba>(entity =>
            {
                entity.HasKey(e => e.IdProsby)
                    .HasName("PK_PROSBA");

                entity.HasIndex(e => e.IdZatwierdzajacego)
                    .HasName("Relationship_6_FK");

                entity.HasIndex(e => e.NrKompanii)
                    .HasName("Relationship_10_FK");

                entity.HasOne(d => d._Zatwierdzajacy)
                    .WithMany(p => p._ZatwierdzeniaProsb)
                    .HasForeignKey(d => d.IdZatwierdzajacego)
                    .HasConstraintName("FK_PROSBA_RELATIONS_ZOLNIERZ");

                entity.HasOne(d => d._Zglaszajacy)
                    .WithMany(p => p._ZgloszoneProsby)
                    .HasForeignKey(d => d.IdZglaszajacego)
                    .HasConstraintName("FK_PROSBA_REFERENCE_ZOLNIERZ");

                entity.HasOne(d => d._Kompania)
                    .WithMany(p => p._Prosby)
                    .HasForeignKey(d => d.NrKompanii)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROSBA_RELATIONS_KOMPANIA");

                entity.HasOne(d => d._Pluton)
                    .WithMany(p => p._Prosby)
                    .HasForeignKey(e => new { e.NrPlutonu, e.NrKompanii })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROSBA_RELATIONS_PLUTON");
            });

            modelBuilder.Entity<Zolnierz>(entity =>
            {
                entity.HasKey(e => e.IdOsoby)
                    .HasName("PK_ZOLNIERZ");

                entity.Property(e => e.Imie)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Nazwisko)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d._Kompania)
                    .WithMany(p => p._Zolnierze)
                    .HasForeignKey(d => d.NrKompanii)
                    .HasConstraintName("FK_ZOLNIERZ_REFERENCE_KOMPANIA");

                entity.HasOne(d => d._Pluton)
                   .WithMany(p => p._Zolnierze)
                   .HasForeignKey(d => new { d.NrPlutonu, d.NrKompanii })
                   .HasConstraintName("FK_ZOLNIERZ_REFERENCE_PLUTON");
            });
        }
    }
}
