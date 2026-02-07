using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensiuneaLotus.MAUI.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string Number { get; set; } // Required, max 10
        public int Capacity { get; set; } // Range 1-10
        public decimal PricePerNight { get; set; } // Min 0.01
        public bool IsActive { get; set; }
    }
}
