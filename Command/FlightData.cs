using DoroonNet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.Command
{
    public class FlightData : NotifyImplementClass
    {
        private static int iD { get; set; }
        public int ID
        {
            get
            {
                return iD;
            }
            set
            {
                iD = value;
                OnPropertyChanged("ID");
            }
        }

        private static string flightID { get; set; }
        public string FlightID
        {
            get
            {
                return flightID;
            }
            set
            {
                flightID = value;
                OnPropertyChanged("FlightID");
            }
        }       

        private static int flightHDG { get; set; }
        public int FlightHDG
        {
            get
            {
                return flightHDG;
            }
            set
            {
                flightHDG = value;
                OnPropertyChanged("FlightHDG");
            }
        }

        private static float flightSPD { get; set; }
        public float FlightSPD
        {
            get
            {
                return flightSPD;
            }
            set
            {
                flightSPD = value;
                OnPropertyChanged("FlightSPD");
            }
        }

        private static float flightALT { get; set; }
        public float FlightALT
        {
            get
            {
                return flightALT;
            }
            set
            {
                flightALT = value;
                OnPropertyChanged("FlightALT");
            }
        }

        private static double flightLAT { get; set; }
        public double FlightLAT
        {
            get
            {
                return flightLAT;
            }
            set
            {
                flightLAT = value;
                OnPropertyChanged("FlightLAT");
            }
        }


        private static double flightLNG { get; set; }
        public double FlightLNG
        {
            get
            {
                return flightLNG;
            }
            set
            {
                flightLNG = value;
                OnPropertyChanged("FlightLNG");
            }
        }
    }
}
