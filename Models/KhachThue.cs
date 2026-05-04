using System.ComponentModel.DataAnnotations;

namespace QLNhaTro.Models
{
    public enum GioiTinh
    {
        Nam = 0,
        Nu = 1
    }

    public class KhachThue
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? CCCD { get; set; }

        [MaxLength(15)]
        public string? SoDienThoai { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? QueQuan { get; set; }

        [MaxLength(100)]
        public string? NgheNghiep { get; set; }

        public DateTime? NgaySinh { get; set; }

        public GioiTinh GioiTinh { get; set; } = GioiTinh.Nam;

        public DateTime NgayTao { get; set; } = DateTime.Now;

        public ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();
    }
}
