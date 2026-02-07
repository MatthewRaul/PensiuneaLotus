namespace PensiuneaLotus.Client.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsOccupied { get; set; }
    }
}