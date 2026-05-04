using Microsoft.EntityFrameworkCore;
using QLNhaTro.Models;

namespace QLNhaTro.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<KhachThue> KhachThues { get; set; }
        public DbSet<HopDong> HopDongs { get; set; }
        public DbSet<ChiSoDienNuoc> ChiSoDienNuocs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<CaiDat> CaiDats { get; set; }
        public DbSet<ThanhVien> ThanhViens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "qlnhatro.db");
            options.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phong>()
                .HasIndex(p => p.MaPhong)
                .IsUnique();

            modelBuilder.Entity<KhachThue>()
                .HasIndex(k => k.CCCD)
                .IsUnique()
                .HasFilter("[CCCD] IS NOT NULL");

            modelBuilder.Entity<HopDong>()
                .HasIndex(h => h.MaHopDong)
                .IsUnique();

            modelBuilder.Entity<HoaDon>()
                .HasIndex(h => h.MaHoaDon)
                .IsUnique();

            modelBuilder.Entity<ChiSoDienNuoc>()
                .HasIndex(c => new { c.PhongId, c.Thang, c.Nam })
                .IsUnique();

            modelBuilder.Entity<CaiDat>().HasData(new CaiDat
            {
                Id = 1,
                TenNhaTro = "Nhà Trọ",
                GiaDien = 3500,
                GiaNuoc = 15000,
                PhiDichVu = 100000,
                NgayCapNhat = new DateTime(2026, 1, 1)
            });
        }
    }
}
