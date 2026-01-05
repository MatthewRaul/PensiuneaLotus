using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PensiuneaLotus.Models
{
    public class Room
    {
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string Number { get; set; } = string.Empty;

        [Range(1, 10)]
        public int Capacity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 999999)]
        public decimal PricePerNight { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
