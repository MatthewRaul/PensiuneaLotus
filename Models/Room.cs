using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PensiuneaLotus.Models
{
    public class Room
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Numarul camerei este obligatoriu")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Numarul camerei trebuie sa aiba maxim 10 caractere")]
        public string Number { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Capacitatea trebuie sa fie intre 1 si 10")]
        public int Capacity { get; set; }

        [Range(0.01, 999999, ErrorMessage = "Pretul pe noapte trebuie sa fie > 0")]
        public decimal PricePerNight { get; set; }


        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
