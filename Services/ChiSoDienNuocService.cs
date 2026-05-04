using Microsoft.EntityFrameworkCore;
using QLNhaTro.Data;
using QLNhaTro.Models;

namespace QLNhaTro.Services
{
    public class ChiSoDienNuocService
    {
        public List<ChiSoDienNuoc> GetByThangNam(int thang, int nam)
        {
            using var db = new AppDbContext();
            return db.ChiSoDienNuocs
                .Include(c => c.Phong)
                .Where(c => c.Thang == thang && c.Nam == nam)
                .OrderBy(c => c.Phong.MaPhong)
                .ToList();
        }

        public ChiSoDienNuoc? GetByPhongThangNam(int phongId, int thang, int nam)
        {
            using var db = new AppDbContext();
            return db.ChiSoDienNuocs
                .FirstOrDefault(c => c.PhongId == phongId && c.Thang == thang && c.Nam == nam);
        }

        public ChiSoDienNuoc? GetPrevious(int phongId, int thang, int nam)
        {
            using var db = new AppDbContext();
            int prevThang = thang == 1 ? 12 : thang - 1;
            int prevNam = thang == 1 ? nam - 1 : nam;
            return db.ChiSoDienNuocs
                .FirstOrDefault(c => c.PhongId == phongId && c.Thang == prevThang && c.Nam == prevNam);
        }

        public void SaveOrUpdate(ChiSoDienNuoc record)
        {
            using var db = new AppDbContext();
            var existing = db.ChiSoDienNuocs
                .FirstOrDefault(c => c.PhongId == record.PhongId && c.Thang == record.Thang && c.Nam == record.Nam);

            if (existing != null)
            {
                existing.ChiSoDienCu = record.ChiSoDienCu;
                existing.ChiSoDienMoi = record.ChiSoDienMoi;
                existing.ChiSoNuocCu = record.ChiSoNuocCu;
                existing.ChiSoNuocMoi = record.ChiSoNuocMoi;
                existing.NgayGhi = DateTime.Now;
            }
            else
            {
                record.NgayGhi = DateTime.Now;
                db.ChiSoDienNuocs.Add(record);
            }

            db.SaveChanges();
        }
    }
}
