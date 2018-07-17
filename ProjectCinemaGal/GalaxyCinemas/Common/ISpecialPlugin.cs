using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Isadora Viana Silva ID: 92017784 26/01/2018
namespace Common
{
   public interface ISpecialPlugin
    {
        //CalculateSpecial method checks if a special offer should be aplied if so,it returns the appropriate details.
     bool CalculateSpecial(Booking booking, ref string specialName, ref decimal specialPrice);
    }
}
