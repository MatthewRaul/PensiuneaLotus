using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PensiuneaLotus.Models
{
    public class Service
    {
        public int ID { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0.00, 999999)]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<ReservationService>? ReservationServices { get; set; }
    }
}
