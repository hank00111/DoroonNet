using DoroonNet.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// QuickSettings.xaml 的互動邏輯
    /// </summary>
    public partial class QuickSettings : UserControl
    {
        List<string> Ip = new List<string>();

        bool init = false;
        bool initOK = false;
        int SelectedHost = 0;

        public QuickSettings()
        {
            InitializeComponent();
        }

        public void Init()
        {
            int i = 0;
            if (!init)
            {
                foreach (var ID in TcpServer.NetIPs)
                {
                    Ip.Add(ID.ToString());   
                    
                    if(ID.ToString() == TcpServer.CurrentHost)
                    {
                        //Console.WriteLine(ID.ToString() + " " + i);
                        SelectedHost = i;
                        CurrentHostStr.Text = TcpServer.CurrentHost;
                    }
                    i++;
                }
                ComboIP.ItemsSource = Ip;             
                init = true;
            }

        }

        private void ComboIP_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            ComboIP.SelectedIndex = SelectedHost;
            initOK = true;
        }

        private void ComboIP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initOK)
            {
                Properties.Settings.Default.TCPHost = ComboIP.SelectedItem.ToString();
                Properties.Settings.Default.Save();
                MessageBox.Show("PLEASE RESTART DroneNET");
            }
        }

        private void ComboIP_Unloaded(object sender, RoutedEventArgs e)
        {
            initOK = false;
            //Console.WriteLine("120");
        }
    }

}
