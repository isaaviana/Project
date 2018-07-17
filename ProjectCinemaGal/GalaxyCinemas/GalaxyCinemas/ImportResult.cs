using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Isadora Viana Silva ID: 92017784 26/01/2018

namespace GalaxyCinemas
{
    public class ImportResult
    {
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public int FailedRows { get; set; }
        
        private List<string> errorMessages = new List<string>();
        
        
        public List<string> ErrorMessages
        {
           get
            {
                return errorMessages;
            }
        }
        //constructor to assigns all int values to 0 and clear the errorMessages List.
        public ImportResult()
        {
            TotalRows = 0;
            ImportedRows = 0;
            FailedRows = 0;
            errorMessages.Clear();

        }
       
    }
}
