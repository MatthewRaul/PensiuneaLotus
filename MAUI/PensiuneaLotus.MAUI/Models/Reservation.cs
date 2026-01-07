using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensiuneaLotus.MAUI.Models;

public class Reservation
{
    public int ID { get; set; }
    public int RoomID { get; set; }
    public int GuestID { get; set; } // Legătura cu tabelul de clienți

    public DateTime CheckInDate { get; set; } // Sincronizat cu coloana din Back-end
    public DateTime CheckOutDate { get; set; } // Sincronizat cu coloana din Back-end

    public string Status { get; set; } = "Pendent"; // Status implicit
}
