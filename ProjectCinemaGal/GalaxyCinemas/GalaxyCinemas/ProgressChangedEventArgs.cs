using System;
//Isadora Viana Silva ID: 92017784 26/01/2018
namespace GalaxyCinemas
{
    public class ProgressChangedEventArgs : EventArgs
    {
        public float Progress { get; set; }

        public ProgressChangedEventArgs(float progress)
        {
            Progress = progress;
        }
    }
}
