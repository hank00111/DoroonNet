using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DoroonNet.ViewModel;

namespace DoroonNet.Command
{
    class VariableRes : NotifyImplementClass
    {
        public static int fpsCount = 0;
        public int FpsCount
        {
            get { return fpsCount; }
            set
            {
                fpsCount = value;
                //OnPropertyChanged("FpsCount");
                RaisePropertyChanged();
            }          

        }

        #region mapinfo       
        public static int HDG { get; set; }
        public static double Lat { get; set; }
        public static double Lng { get; set; }

        public static bool ifDel { get; set; }
        #endregion


        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnProperyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

    }
}

