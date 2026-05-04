using Microsoft.EntityFrameworkCore;
using QLNhaTro.Data;
using QLNhaTro.Models;

namespace QLNhaTro.Services
{
    public class KhachThueService
    {
        public List<KhachThue> GetAll()
        {
            using var db = new AppDbContext();
            return db.KhachThues.OrderBy(k => k.HoTen).ToList();
        }

        public List<KhachThue> Search(string keyword)
        {
            using var db = new AppDbContext();
            if (string.IsNullOrWhiteSpace(keyword))
                return db.KhachThues.OrderBy(k => k.HoTen).ToList();

            keyword = keyword.ToLower();
            return db.KhachThues
                .Where(k => k.HoTen.ToLower().Contains(keyword)
                    || (k.CCCD != null && k.CCCD.Contains(keyword))
                    || (k.SoDienThoai != null && k.SoDienThoai.Contains(keyword)))
                .OrderBy(k => k.HoTen)
                .ToList();
        }

        public KhachThue? GetById(int id)
        {
            using var db = new AppDbContext();
            return db.KhachThues.Find(id);
        }

        public void Add(KhachThue kt)
        {
            using var db = new AppDbContext();
            db.KhachThues.Add(kt);
            db.SaveChanges();
        }

        public void Update(KhachThue kt)
        {
            using var db = new AppDbContext();
            var existing = db.KhachThues.Find(kt.Id);
            if (existing == null) return;

            existing.HoTen = kt.HoTen;
            existing.CCCD = kt.CCCD;
            existing.SoDienThoai = kt.SoDienThoai;
            existing.Email = kt.Email;
            existing.QueQuan = kt.QueQuan;
            existing.NgheNghiep = kt.NgheNghiep;
            existing.NgaySinh = kt.NgaySinh;
            existing.GioiTinh = kt.GioiTinh;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            using var db = new AppDbContext();
            var kt = db.KhachThues.Include(k => k.HopDongs).FirstOrDefault(k => k.Id == id);
            if (kt == null) return false;
            if (kt.HopDongs.Any(h => h.TrangThai == TrangThaiHopDong.ConHieuLuc))
                return false;

            db.KhachThues.Remove(kt);
            db.SaveChanges();
            return true;
        }

        public List<KhachThue> GetAvailableTenants()
        {
            using var db = new AppDbContext();
            var tenantsWithActiveContracts = db.HopDongs
                .Where(h => h.TrangThai == TrangThaiHopDong.ConHieuLuc)
                .Select(h => h.KhachThueId)
                .ToList();

            return db.KhachThues
                .Where(k => !tenantsWithActiveContracts.Contains(k.Id))
                .OrderBy(k => k.HoTen)
                .ToList();
        }

        public int CountAll()
        {
            using var db = new AppDbContext();
            return db.KhachThues.Count();
        }
    }
}
