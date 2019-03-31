using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GlobalCityManager.Models
{
    public partial class WorldContext : DbContext
    {
        public WorldContext()
        {
        }

        public WorldContext(DbContextOptions<WorldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Ciudades { get; set; }
        public virtual DbSet<Country> Paises { get; set; }

        private static WorldContext _context;
        public static WorldContext Instance
        {
            get
            {
                if (_context == null)
                {
                    _context = new WorldContext();
                }

                return _context;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                /* #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings. */
                optionsBuilder.UseMySql("Server=localhost;Database=world;Port=3306;Uid=root;Pwd=Fesopo1945$;SslMode=none");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasIndex(e => e.CountryCode)
                    .HasName("CountryCode");

                entity.Property(e => e.CountryCode).HasDefaultValueSql("''");

                entity.Property(e => e.District).HasDefaultValueSql("''");

                entity.Property(e => e.Name).HasDefaultValueSql("''");

                entity.Property(e => e.Population).HasDefaultValueSql("'0'");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("city_ibfk_1");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PRIMARY");

                entity.Property(e => e.Code).HasDefaultValueSql("''");

                entity.Property(e => e.Name).HasDefaultValueSql("''");

                entity.Property(e => e.NationalFlag).HasDefaultValueSql("'Images\\\\Default.png'");

                entity.Property(e => e.Region).HasDefaultValueSql("''");
            });
        }
    }
}
