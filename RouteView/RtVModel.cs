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

        #region FW
        private static double heightTxFW { get; set; }
        public double HeightTxFW
        {
            get
            {
                return heightTxFW;
            }
            set
            {
                heightTxFW = Math.Round(value, 3);
                OnPropertyChanged("HeightTxFW");
            }
        }
        private static double widthTxFW { get; set; }
        public double WidthTxFW
        {
            get
            {
                return widthTxFW;
            }
            set
            {
                widthTxFW = Math.Round(value, 3);
                OnPropertyChanged("WidthTxFW");
            }
        }
        private static double slTxFW { get; set; }
        public double SLTxFW
        {
            get
            {
                return slTxFW;
            }
            set
            {
                slTxFW = value;
                OnPropertyChanged("SLTxFW");
            }
        }
        private static int angleTxFW { get; set; }
        public int AngleTxFW
        {
            get
            {
                return angleTxFW;
            }
            set
            {
                angleTxFW = value;
                OnPropertyChanged("AngleTxFW");
            }
        }
        private static float altTxFW { get; set; }
        public float AltTxFW
        {
            get
            {
                return altTxFW;
            }
            set
            {
                altTxFW = value;
                OnPropertyChanged("AltTxFW");
            }
        }
        #endregion

        public RtVModel()
        {
            HeightTx = 5;
            WidthTx = 5;
            AngleTx = 0;
            SLTx = 5;
            AltTx = 10;
            HeightTxFW = 5;
            WidthTxFW = 5;
            AngleTxFW = 0;
            SLTxFW = 5;
            AltTxFW = 10;
        }
    }
}
