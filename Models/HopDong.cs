using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaTro.Models
{
    public enum TrangThaiHopDong
    {
        ConHieuLuc = 0,
        HetHan = 1,
        DaHuy = 2
    }

    public class HopDong
    {
        [Key]
        public int Id { get; set; }

        public int PhongId { get; set; }
        public int KhachThueId { get; set; }

        [Required, MaxLength(30)]
        public string MaHopDong { get; set; } = string.Empty;

        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal TienCoc { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal GiaThueThucTe { get; set; }

        public TrangThaiHopDong TrangThai { get; set; } = TrangThaiHopDong.ConHieuLuc;

        public DateTime NgayTao { get; set; } = DateTime.Now;

        [ForeignKey(nameof(PhongId))]
        public Phong Phong { get; set; } = null!;

        [ForeignKey(nameof(KhachThueId))]
        public KhachThue KhachThue { get; set; } = null!;

        public ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
        public ICollection<ThanhVien> ThanhViens { get; set; } = new List<ThanhVien>();
    }
}
