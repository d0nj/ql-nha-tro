using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaTro.Models
{
    public enum TrangThaiHoaDon
    {
        ChuaThanhToan = 0,
        DaThanhToan = 1
    }

    public class HoaDon
    {
        [Key]
        public int Id { get; set; }

        public int HopDongId { get; set; }

        [Required, MaxLength(30)]
        public string MaHoaDon { get; set; } = string.Empty;

        public int Thang { get; set; }
        public int Nam { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal TienPhong { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal TienDien { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal TienNuoc { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal PhiDichVu { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal TongTien { get; set; }

        public TrangThaiHoaDon TrangThai { get; set; } = TrangThaiHoaDon.ChuaThanhToan;

        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? NgayThanhToan { get; set; }

        [ForeignKey(nameof(HopDongId))]
        public HopDong HopDong { get; set; } = null!;
    }
}
