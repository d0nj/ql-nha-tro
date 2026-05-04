using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaTro.Models
{
    public class ThanhVien
    {
        [Key]
        public int Id { get; set; }

        public int HopDongId { get; set; }

        [Required, MaxLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? CCCD { get; set; }

        [MaxLength(15)]
        public string? SoDienThoai { get; set; }

        [MaxLength(50)]
        public string? QuanHe { get; set; } // e.g., "Vợ", "Con", "Bạn", "Anh/Chị/Em"

        public DateTime NgayTao { get; set; } = DateTime.Now;

        [ForeignKey(nameof(HopDongId))]
        public HopDong HopDong { get; set; } = null!;
    }
}
