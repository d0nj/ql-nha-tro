using QLNhaTro.Data;
using QLNhaTro.Models;

namespace QLNhaTro.Services
{
    public class CaiDatService
    {
        public CaiDat Get()
        {
            using var db = new AppDbContext();
            return db.CaiDats.First();
        }

        public void Update(CaiDat caiDat)
        {
            using var db = new AppDbContext();
            var existing = db.CaiDats.First();
            existing.TenNhaTro = caiDat.TenNhaTro;
            existing.DiaChi = caiDat.DiaChi;
            existing.SoDienThoai = caiDat.SoDienThoai;
            existing.GiaDien = caiDat.GiaDien;
            existing.GiaNuoc = caiDat.GiaNuoc;
            existing.PhiDichVu = caiDat.PhiDichVu;
            existing.NgayCapNhat = DateTime.Now;
            db.SaveChanges();
        }
    }
}
