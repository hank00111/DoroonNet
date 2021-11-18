using DoroonNet.Command;
using DoroonNet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoroonNet.Views
{
    /// <summary>
    /// FlightInfo.xaml 的互動邏輯
    /// </summary>
    public partial class FlightInfo : UserControl
    {
        private FlightInfoRightCommand firc;
        public FlightInfo()
        {
            InitializeComponent();
            this.DataContext = new InfoViewModel();
            firc = new FlightInfoRightCommand();
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (FlightList.SelectedIndex != -1)
            //{                
            //    firc.DataID = (FlightList.SelectedIndex + 1).ToString();
            //    firc.SdataID = FlightList.SelectedIndex + 1;
            //}
          
            //Console.WriteLine(firc.SdataID);
        }
    }
}
