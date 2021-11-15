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
using System.Windows.Threading;

namespace DoroonNet.Views
{
    /// <summary>
    /// FlightInfoRight.xaml 的互動邏輯
    /// </summary>
    public partial class FlightInfoRight : UserControl
    {
        private static DispatcherTimer Ref;
        private FlightInfoRightCommand Firc;

        public FlightInfoRight()
        {
            InitializeComponent();
            Firc = new FlightInfoRightCommand();

            this.DataContext = this.Firc;

            Ref = new DispatcherTimer();
            Ref.Interval = TimeSpan.FromMilliseconds(700);
            Ref.Tick += RefData;
            Ref.Start();

        }

        private void RefData(object sender, EventArgs e)
        {
            FlightInfoRightCommand VarR = new FlightInfoRightCommand();
            ImageShowID.DataContext = VarR;
            DataShowID.DataContext = VarR;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TcpServer.SendC(Firc.SdataID);
           
        }
    }

}
