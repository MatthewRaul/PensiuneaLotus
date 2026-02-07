namespace PensiuneaLotus.Client.Models
{
    public class Guest
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        // Aici am corectat! Inainte scrisesem PhoneNumber, dar tu ai Phone in backend.
        public string Phone { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}