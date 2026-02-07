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

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

       


        // Navigation
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ReservationService>? ReservationServices { get; set; }
    }
}
