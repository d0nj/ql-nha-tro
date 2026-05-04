using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaTro.Models
{
    public enum TrangThaiPhong
    {
        Trong = 0,
        DangThue = 1,
        SuaChua = 2
    }

    public class Phong
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string MaPhong { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string TenPhong { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,0)")]
        public decimal GiaThue { get; set; }

        public double DienTich { get; set; }

        public int SoNguoiToiDa { get; set; } = 4;

        public TrangThaiPhong TrangThai { get; set; } = TrangThaiPhong.Trong;

        [MaxLength(500)]
        public string? MoTa { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();
        public ICollection<ChiSoDienNuoc> ChiSoDienNuocs { get; set; } = new List<ChiSoDienNuoc>();
    }
}
