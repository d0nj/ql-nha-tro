using Microsoft.EntityFrameworkCore;
using QLNhaTro.Data;
using QLNhaTro.Models;

namespace QLNhaTro.Services
{
    public class HoaDonService
    {
        public List<HoaDon> GetAll()
        {
            using var db = new AppDbContext();
            return db.HoaDons
                .Include(h => h.HopDong).ThenInclude(hd => hd.Phong)
                .Include(h => h.HopDong).ThenInclude(hd => hd.KhachThue)
                .OrderByDescending(h => h.NgayTao)
                .ToList();
        }

        public List<HoaDon> Search(string keyword, TrangThaiHoaDon? trangThai = null, int? thang = null, int? nam = null)
        {
            using var db = new AppDbContext();
            var query = db.HoaDons
                .Include(h => h.HopDong).ThenInclude(hd => hd.Phong)
                .Include(h => h.HopDong).ThenInclude(hd => hd.KhachThue)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(h =>
                    h.MaHoaDon.ToLower().Contains(keyword)
                    || h.HopDong.Phong.TenPhong.ToLower().Contains(keyword)
                    || h.HopDong.KhachThue.HoTen.ToLower().Contains(keyword));
            }

            if (trangThai.HasValue)
                query = query.Where(h => h.TrangThai == trangThai.Value);

            if (thang.HasValue)
                query = query.Where(h => h.Thang == thang.Value);

            if (nam.HasValue)
                query = query.Where(h => h.Nam == nam.Value);

            return query.OrderByDescending(h => h.NgayTao).ToList();
        }

        public string GenerateMaHoaDon()
        {
            using var db = new AppDbContext();
            var count = db.HoaDons.Count() + 1;
            return $"HD{DateTime.Now:yyMM}{count:D4}";
        }

        public HoaDon GenerateInvoice(int hopDongId, int thang, int nam)
        {
            using var db = new AppDbContext();
            var hopDong = db.HopDongs.Include(h => h.Phong).First(h => h.Id == hopDongId);
            var caiDat = db.CaiDats.First();
            var chiSo = db.ChiSoDienNuocs
                .FirstOrDefault(c => c.PhongId == hopDong.PhongId && c.Thang == thang && c.Nam == nam);

            decimal tienDien = 0, tienNuoc = 0;
            if (chiSo != null)
            {
                tienDien = (chiSo.ChiSoDienMoi - chiSo.ChiSoDienCu) * caiDat.GiaDien;
                tienNuoc = (chiSo.ChiSoNuocMoi - chiSo.ChiSoNuocCu) * caiDat.GiaNuoc;
            }

            var hoaDon = new HoaDon
            {
                HopDongId = hopDongId,
                MaHoaDon = GenerateMaHoaDon(),
                Thang = thang,
                Nam = nam,
                TienPhong = hopDong.GiaThueThucTe,
                TienDien = tienDien,
                TienNuoc = tienNuoc,
                PhiDichVu = caiDat.PhiDichVu,
                TrangThai = TrangThaiHoaDon.ChuaThanhToan,
                NgayTao = DateTime.Now
            };
            hoaDon.TongTien = hoaDon.TienPhong + hoaDon.TienDien + hoaDon.TienNuoc + hoaDon.PhiDichVu;

            return hoaDon;
        }

        public void Add(HoaDon hoaDon)
        {
            using var db = new AppDbContext();
            db.HoaDons.Add(hoaDon);
            db.SaveChanges();
        }

        public void MarkAsPaid(int id)
        {
            using var db = new AppDbContext();
            var hd = db.HoaDons.Find(id);
            if (hd == null) return;

            hd.TrangThai = TrangThaiHoaDon.DaThanhToan;
            hd.NgayThanhToan = DateTime.Now;
            db.SaveChanges();
        }

        public bool InvoiceExists(int hopDongId, int thang, int nam)
        {
            using var db = new AppDbContext();
            return db.HoaDons.Any(h => h.HopDongId == hopDongId && h.Thang == thang && h.Nam == nam);
        }

        public decimal GetRevenueByMonth(int thang, int nam)
        {
            using var db = new AppDbContext();
            return db.HoaDons
                .Where(h => h.Thang == thang && h.Nam == nam && h.TrangThai == TrangThaiHoaDon.DaThanhToan)
                .Select(h => h.TongTien)
                .ToList()
                .DefaultIfEmpty(0)
                .Sum();
        }

        public decimal GetTotalUnpaid()
        {
            using var db = new AppDbContext();
            return db.HoaDons
                .Where(h => h.TrangThai == TrangThaiHoaDon.ChuaThanhToan)
                .Select(h => h.TongTien)
                .ToList()
                .DefaultIfEmpty(0)
                .Sum();
        }

        public int CountUnpaid()
        {
            using var db = new AppDbContext();
            return db.HoaDons.Count(h => h.TrangThai == TrangThaiHoaDon.ChuaThanhToan);
        }
    }
}
