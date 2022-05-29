using DoroonNet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.RouteView
{
    public class RtVModel : NotifyImplementClass
    {
        private static double heightTx { get; set; }
        public double HeightTx
        {
            get
            {
                return heightTx;
            }
            set
            {
                heightTx = Math.Round(value, 3);
                OnPropertyChanged("HeightTx");
            }
        }

        private static double widthTx { get; set; }
        public double WidthTx
        {
            get
            {
                return widthTx;
            }
            set
            {
                widthTx = Math.Round(value,3);
                OnPropertyChanged("WidthTx");
            }
        }
        private static double slTx { get; set; }
        public double SLTx
        {
            get
            {
                return slTx;
            }
            set
            {
                slTx = value;
                OnPropertyChanged("SLTx");
            }
        }
        private static int angleTx { get; set; }
        public int AngleTx
        {
            get
            {
                return angleTx;
            }
            set
            {
                angleTx = value;
                OnPropertyChanged("AngleTx");
            }
        }
        private static float altTx { get; set; }
        public float AltTx
        {
            get
            {
                return altTx;
            }
            set
            {
                altTx = value;
                OnPropertyChanged("AltTx");
            }
        }

        public RtVModel()
        {
            HeightTx = 5;
            WidthTx = 5;
            AngleTx = 0;
            SLTx = 5;
            AltTx = 10;
        }
    }
}
