using System.ComponentModel.DataAnnotations;

namespace PensiuneaLotus.Models
{
    public class ReservationService
    {
        public int ID { get; set; }

        [Required]
        public int ReservationID { get; set; }
        public Reservation? Reservation { get; set; }

        [Required]
        public int ServiceID { get; set; }
        public Service? Service { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }
}
