using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaTro.Models
{
    public class CaiDat
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string TenNhaTro { get; set; } = "Nhà Trọ";

        [MaxLength(300)]
        public string? DiaChi { get; set; }

        [MaxLength(15)]
        public string? SoDienThoai { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal GiaDien { get; set; } = 3500;

        [Column(TypeName = "decimal(18,0)")]
        public decimal GiaNuoc { get; set; } = 15000;

        [Column(TypeName = "decimal(18,0)")]
        public decimal PhiDichVu { get; set; } = 100000;

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        [MaxLength(10)]
        public string ThemeMode { get; set; } = "light";
    }
}
