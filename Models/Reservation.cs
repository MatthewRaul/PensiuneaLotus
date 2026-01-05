using System.ComponentModel.DataAnnotations;

namespace PensiuneaLotus.Models
{
    public class Reservation
    {
        public int ID { get; set; }

        [Required]
        public int RoomID { get; set; }
        public Room? Room { get; set; }

        [Required]
        public int GuestID { get; set; }
        public Guest? Guest { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [StringLength(30)]
        public string Status { get; set; } = "New"; // New/Confirmed/Cancelled/CheckedIn/CheckedOut

        // Navigation
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ReservationService>? ReservationServices { get; set; }
    }
}
