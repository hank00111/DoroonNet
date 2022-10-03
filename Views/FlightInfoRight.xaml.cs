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
        private List<string> UavList = new List<string>();
        private int CheckCount = 0;
        //private bool DebugMode = false;
        public static int SelSend;

        public FlightInfoRight()
        {
            InitializeComponent();
            this.DataContext = new InfoViewModel();
            Firc = new FlightInfoRightCommand();

            //this.DataContext = this.Firc;

            Ref = new DispatcherTimer();
            Ref.Interval = TimeSpan.FromMilliseconds(200);
            Ref.Tick += RefData;
            Ref.Start();

        }

        private void RefData(object sender, EventArgs e)
        {
            UavComboInit();
            SelSend = UavCombo.SelectedIndex;
        }

        private void UavComboInit()
        {

            if (TcpServer.Clients.Count != CheckCount)
            {
                UavList.Clear();
                UavCombo.ItemsSource = null;
                foreach (var ID in TcpServer.Clients)
                {
                    UavList.Add("#" + ((System.Net.IPEndPoint)ID.RemoteEndPoint).Port.ToString());
                    //Console.WriteLine(((System.Net.IPEndPoint)ID.RemoteEndPoint).Port.ToString());
                }
                UavCombo.ItemsSource = UavList;
                CheckCount = TcpServer.Clients.Count;
                UavCombo.SelectedIndex = -1;
            }
        }

        private void CAM_ON_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UavCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("請選擇無人機，以發送相機開啟指令");
                    return;
                }
                else
                {
                    TcpServer.SendCAM(UavCombo.SelectedIndex, 1);
                }
                //Console.WriteLine(Firc.SdataID);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void CAM_OFF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UavCombo.SelectedIndex == -1)   
                {
                    MessageBox.Show("請選擇無人機，以發送相機關閉指令");
                    return;
                }
                else
                {
                    TcpServer.SendCAM(UavCombo.SelectedIndex, 0);
                }
                //Console.WriteLine(Firc.SdataID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //TcpServer Tcplink = new TcpServer();
            //Tcplink.AsyncSocketListeners();
        }

        private void TAKEOFF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UavCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("請選擇無人機，以發送起飛指令");
                    return;
                }
                else
                {
                    TcpServer.SendControl(UavCombo.SelectedIndex, 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void TASK_START_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UavCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("請選擇無人機，以發送起飛指令");
                    return;
                }
                else
                {
                    TcpServer.SendControl(UavCombo.SelectedIndex, 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LANDING_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UavCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("請選擇無人機，以發送降落指令");
                    return;
                }
                else
                {
                    TcpServer.SendControl(UavCombo.SelectedIndex, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }

}
