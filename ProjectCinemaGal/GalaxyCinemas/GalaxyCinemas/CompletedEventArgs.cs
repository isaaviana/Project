﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Isadora Viana Silva ID: 92017784 26/01/2018
namespace GalaxyCinemas
{
    public class CompletedEventArgs : EventArgs
    {
        public ImportResult Result { get; set; }

        public CompletedEventArgs(ImportResult result)
        {
            Result = result;
        }
    }
}
