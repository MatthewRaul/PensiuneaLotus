using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PensiuneaLotus.Models
{
    public class Payment
    {
        public int ID { get; set; }

        [Required]
        public int ReservationID { get; set; }
        public Reservation? Reservation { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 999999)]
        public decimal Amount { get; set; }

        [StringLength(20)]
        public string Method { get; set; } = "Cash"; // Cash/Card/Transfer

        [DataType(DataType.Date)]
        public DateTime PaidAt { get; set; } = DateTime.Now;
    }
}
