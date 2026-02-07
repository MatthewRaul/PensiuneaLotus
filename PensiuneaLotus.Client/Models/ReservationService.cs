namespace PensiuneaLotus.Client.Models
{
    public class ReservationService
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        public int ServiceID { get; set; }
        public int Quantity { get; set; } = 1;
    }
}