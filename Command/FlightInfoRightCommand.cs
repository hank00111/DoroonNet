using DoroonNet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoroonNet.Command
{
    public class FlightInfoRightCommand : NotifyImplementClass
    {
        private static string imageID { get; set; }
        public string ImageID
        {
            get { return "當前影像顯示 #" + imageID; }
            set
            {
                if (imageID != value)
                {
                    imageID = value;
                    OnPropertyChanged("ImageID");
                }
            }
        }

        private static string dataID { get; set; }
        public string DataID
        {
            get { return "當前資料顯示 #" + dataID; }
            set
            {
                if (dataID != value)
                {
                    dataID = value;
                    OnPropertyChanged("DataID");
                }
            }
        }

        private static int sdataID { get; set; }
        public int SdataID
        {
            get { return sdataID; }
            set
            {
                sdataID = value;
                OnPropertyChanged("SdataID");
            }
        }
    }
  
}
