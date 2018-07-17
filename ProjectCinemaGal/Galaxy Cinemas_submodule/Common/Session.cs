using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Isadora Viana Silva ID: 92017784 26/01/2018
namespace Common
{
    public class Session
    {
        public int SessionID { get; set; }
        public int MovieID { get; set; }
        public DateTime SessionDate { get; set; }
        public byte CinemaNumber { get; set; }

        
        public String ShortFormat
        {
            get
            {
                 return string.Format("{0:HH:mm}- Cinema{1}", SessionDate, CinemaNumber);
            }
          
        }

    }
}
