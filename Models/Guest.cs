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

        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(120)]
        public string? Email { get; set; }

        // Navigation
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
