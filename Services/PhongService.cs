using Microsoft.EntityFrameworkCore;
using QLNhaTro.Data;
using QLNhaTro.Models;

namespace QLNhaTro.Services
{
    public class PhongService
    {
        public List<Phong> GetAll()
        {
            using var db = new AppDbContext();
            return db.Phongs.OrderBy(p => p.MaPhong).ToList();
        }

        public List<Phong> Search(string keyword, TrangThaiPhong? trangThai = null)
        {
            using var db = new AppDbContext();
            var query = db.Phongs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(p => p.MaPhong.ToLower().Contains(keyword)
                    || p.TenPhong.ToLower().Contains(keyword));
            }

            if (trangThai.HasValue)
                query = query.Where(p => p.TrangThai == trangThai.Value);

            return query.OrderBy(p => p.MaPhong).ToList();
        }

        public Phong? GetById(int id)
        {
            using var db = new AppDbContext();
            return db.Phongs.Find(id);
        }

        public void Add(Phong phong)
        {
            using var db = new AppDbContext();
            db.Phongs.Add(phong);
            db.SaveChanges();
        }

        public void Update(Phong phong)
        {
            using var db = new AppDbContext();
            var existing = db.Phongs.Find(phong.Id);
            if (existing == null) return;

            existing.MaPhong = phong.MaPhong;
            existing.TenPhong = phong.TenPhong;
            existing.GiaThue = phong.GiaThue;
            existing.DienTich = phong.DienTich;
            existing.SoNguoiToiDa = phong.SoNguoiToiDa;
            existing.TrangThai = phong.TrangThai;
            existing.MoTa = phong.MoTa;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            using var db = new AppDbContext();
            var phong = db.Phongs.Include(p => p.HopDongs).FirstOrDefault(p => p.Id == id);
            if (phong == null) return false;
            if (phong.HopDongs.Any(h => h.TrangThai == TrangThaiHopDong.ConHieuLuc))
                return false;

            db.Phongs.Remove(phong);
            db.SaveChanges();
            return true;
        }

        public bool IsMaPhongExists(string maPhong, int excludeId = 0)
        {
            using var db = new AppDbContext();
            return db.Phongs.Any(p => p.MaPhong == maPhong && p.Id != excludeId);
        }

        public int CountByTrangThai(TrangThaiPhong trangThai)
        {
            using var db = new AppDbContext();
            return db.Phongs.Count(p => p.TrangThai == trangThai);
        }

        public int CountAll()
        {
            using var db = new AppDbContext();
            return db.Phongs.Count();
        }
    }
}
