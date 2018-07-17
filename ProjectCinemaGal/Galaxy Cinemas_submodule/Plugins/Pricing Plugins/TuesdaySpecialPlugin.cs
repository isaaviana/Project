using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using Common; //allow reference Common objects from the Business_Object folder with the Common project.
//Isadora Viana Silva ID: 92017784 26/01/2018
namespace GalaxyCinemas
{
    public class TuesdaySpecialPlugin : ISpecialPlugin
    {

       public bool CalculateSpecial(Booking booking, ref string specialName, ref decimal specialPrice)
        {
            
            decimal basePrice = 0.0m;
            if (!"Tuesday".Equals(booking.SessionDate.DayOfWeek.ToString()))
            {
                
                return false;
            }
            else
            {
                basePrice = booking.OriginalPrice / booking.Quantity;
            }
            decimal discountedPrice = 0.0m;
            if (booking.Quantity <= 5)
            {
                discountedPrice = 11.0m * booking.Quantity;

            }
            else
            {
                for (int discPrice = 0; discPrice < booking.Quantity; discPrice++)
                {
                    if (discPrice < 5)
                    {
                        discountedPrice = 11.0m + discountedPrice;
                    }
                    else
                    {
                        discountedPrice = basePrice + discountedPrice;
                    }
                    
                }

                //discountedPrice = basePrice * booking.Quantity;
                //booking.FinalPrice = discountedPrice;
            }
            booking.FinalPrice = discountedPrice;
            if (discountedPrice < booking.OriginalPrice)
            {
                
                specialPrice = discountedPrice;
                specialName = "Tuesday special!";
                
                return true;
            }
            else
            {
                return false;
            }
        }           
           
            // Calculate the discounted price that we would offer.

            // If this discount is applicable, set specialName and specialPrice to our name and price.

        
    }
}
