using System.ComponentModel.DataAnnotations;

namespace PensiuneaLotus.Models
{
    public class Guest
    {
        public int ID { get; set; }

        [Required]
        [StringLength(60)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage ="Numar de telefon obligatoriu")]
        [RegularExpression(@"^0\d{3}([ .-]?\d{3}){2}$",
        ErrorMessage = "Telefonul trebuie sa fie de forma 0722-123-123 sau 0722.123.123 sau 0722 123 123")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Required(ErrorMessage ="Email obligatoriu")]
        [EmailAddress(ErrorMessage = "Email invalid")]
        [StringLength(120)]
        public string? Email { get; set; }


        // Navigation
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
