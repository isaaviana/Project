using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common; //allow reference Common objects from the Business_Object folder with the Common project.

//Isadora Viana Silva ID: 92017784 26/01/2018
namespace GalaxyCinemas
{
    public class MidDaySpecialPlugin : ISpecialPlugin
    {
       public bool CalculateSpecial(Booking booking, ref string specialName, ref decimal specialPrice)
        {
            
            decimal discountedPrice;
            TimeSpan timeOfDay = booking.SessionDate.TimeOfDay;
            // If not mid-day, not applicable.
            // If movie doesn't start between 11am and 1pm
            if (timeOfDay < new TimeSpan(11,0,0) || timeOfDay > new TimeSpan(13, 0, 0))
            {
                
                return false;
            }
            else // Calculate the discounted price that we would offer.
            {
                
                
                discountedPrice = booking.OriginalPrice * 0.2m;
                
                if (discountedPrice < booking.OriginalPrice)// If this discount is applicable, set specialName and specialPrice to our name and price.
                {
                    
                    specialPrice = booking.OriginalPrice - discountedPrice;
                    specialName = "Mid-day special";
                    return true;
                }
                else
                {
                    return false;
                }
            }            
            
        }

    }
}
   

