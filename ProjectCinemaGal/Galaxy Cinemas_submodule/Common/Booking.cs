using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Isadora Viana Silva ID: 92017784 26/01/2018
namespace Common
{
    [Serializable]
    public class Booking
    {
        public int BookingNumber { get; set; }
        public int SessionID { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public string Special { get; set; }
        public DateTime SessionDate { get; set; }
        
    }
}
