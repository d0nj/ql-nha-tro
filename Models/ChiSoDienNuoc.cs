using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaTro.Models
{
    public class ChiSoDienNuoc
    {
        [Key]
        public int Id { get; set; }

        public int PhongId { get; set; }

        public int Thang { get; set; }
        public int Nam { get; set; }

        public int ChiSoDienCu { get; set; }
        public int ChiSoDienMoi { get; set; }

        public int ChiSoNuocCu { get; set; }
        public int ChiSoNuocMoi { get; set; }

        public DateTime NgayGhi { get; set; } = DateTime.Now;

        [ForeignKey(nameof(PhongId))]
        public Phong Phong { get; set; } = null!;
    }
}
