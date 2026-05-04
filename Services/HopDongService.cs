using Microsoft.EntityFrameworkCore;
using QLNhaTro.Data;
using QLNhaTro.Models;

namespace QLNhaTro.Services
{
    public class HopDongService
    {
        public List<HopDong> GetAll()
        {
            using var db = new AppDbContext();
            return db.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.KhachThue)
                .OrderByDescending(h => h.NgayTao)
                .ToList();
        }

        public List<HopDong> Search(string keyword, TrangThaiHopDong? trangThai = null)
        {
            using var db = new AppDbContext();
            var query = db.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.KhachThue)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(h =>
                    h.MaHopDong.ToLower().Contains(keyword)
                    || h.Phong.TenPhong.ToLower().Contains(keyword)
                    || h.KhachThue.HoTen.ToLower().Contains(keyword));
            }

            if (trangThai.HasValue)
                query = query.Where(h => h.TrangThai == trangThai.Value);

            return query.OrderByDescending(h => h.NgayTao).ToList();
        }

        public HopDong? GetById(int id)
        {
            using var db = new AppDbContext();
            return db.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.KhachThue)
                .FirstOrDefault(h => h.Id == id);
        }

        public HopDong? GetActiveByPhong(int phongId)
        {
            using var db = new AppDbContext();
            return db.HopDongs
                .Include(h => h.KhachThue)
                .FirstOrDefault(h => h.PhongId == phongId && h.TrangThai == TrangThaiHopDong.ConHieuLuc);
        }

        public string GenerateMaHopDong()
        {
            using var db = new AppDbContext();
            var count = db.HopDongs.Count() + 1;
            return $"HD{count:D5}";
        }

        public void Add(HopDong hopDong)
        {
            using var db = new AppDbContext();
            db.HopDongs.Add(hopDong);

            var phong = db.Phongs.Find(hopDong.PhongId);
            if (phong != null)
                phong.TrangThai = TrangThaiPhong.DangThue;

            db.SaveChanges();
        }

        public void Terminate(int id)
        {
            using var db = new AppDbContext();
            var hd = db.HopDongs.Find(id);
            if (hd == null) return;

            hd.TrangThai = TrangThaiHopDong.DaHuy;
            hd.NgayKetThuc = DateTime.Now;

            var phong = db.Phongs.Find(hd.PhongId);
            if (phong != null)
                phong.TrangThai = TrangThaiPhong.Trong;

            db.SaveChanges();
        }

        public List<HopDong> GetExpiringSoon(int days = 30)
        {
            using var db = new AppDbContext();
            var cutoff = DateTime.Now.AddDays(days);
            return db.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.KhachThue)
                .Where(h => h.TrangThai == TrangThaiHopDong.ConHieuLuc
                    && h.NgayKetThuc.HasValue
                    && h.NgayKetThuc.Value <= cutoff)
                .OrderBy(h => h.NgayKetThuc)
                .ToList();
        }

        public int CountActive()
        {
            using var db = new AppDbContext();
            return db.HopDongs.Count(h => h.TrangThai == TrangThaiHopDong.ConHieuLuc);
        }

        public List<HopDong> GetActive()
        {
            using var db = new AppDbContext();
            return db.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.KhachThue)
                .Where(h => h.TrangThai == TrangThaiHopDong.ConHieuLuc)
                .ToList();
        }
    }
}
