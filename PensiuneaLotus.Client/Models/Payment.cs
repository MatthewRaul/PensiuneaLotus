namespace PensiuneaLotus.Client.Models
{
    public class Payment
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } // "Cash", "Card", etc.
        public DateTime PaidAt { get; set; }
    }
}