namespace PensiuneaLotus.Client.Models
{
    public class Reservation
    {
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int GuestID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        // Putem pastra referintele simple daca API-ul le trimite, 
        // dar pentru siguranta, la inceput lucram doar cu ID-uri.
    }
}