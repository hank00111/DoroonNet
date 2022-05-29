using DoroonNet.Command;
using DoroonNet.Views;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DoroonNet.NavView
{
    /// <summary>
    /// Nav.xaml 的互動邏輯
    /// </summary>
    public partial class Nav : Window
    {
        private static DispatcherTimer Ref;
        private int t = 0;
        private int Counta = 1;
        private int pos = 0;
        private bool GoHome;
        private bool IsGoHomeSel;        
        private string GoHomeStr;
        private bool NavIsLoad;
        private bool AltSet;

        public static bool ReqDel;

        public Nav()
        {
            InitializeComponent();
            this.NavDataGrid.ItemsSource = null;
            this.NavDataGrid.ItemsSource = NavData.NavList;

            Ref = new DispatcherTimer();
            Ref.Interval = TimeSpan.FromMilliseconds(1);
            Ref.Tick += RefData;
            //Ref.Start();
        }
        public void NavDGRef()
        {
            Ref.Start();
            if (t == NavData.NavList.Count)
            {
                Ref.Stop();
            }
            
        }
        private void RefData(object sender, EventArgs e)
        {
            if(t!= NavData.NavList.Count || AltSet)
            {
                this.NavDataGrid.ItemsSource = null;
                this.NavDataGrid.ItemsSource = NavData.NavList;
                t = NavData.NavList.Count;
                foreach(var data in NavData.NavList)
                {
                    data.ALT = float.Parse(AltTxTBox.Text);
                    //Console.WriteLine(data.ALT);
                }
                AltSet = false;
            }
            //this.NavDataGrid.ItemsSource = null;
        }

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var window = (Window)sender;
            window.Topmost = true;
        }

        private void SendControlBt_Click(object sender, RoutedEventArgs e)
        {
            //byte[] NavDATAByte = new byte[NavData.NavList.Count * 24];
            //NavData.NavList.ForEach(item => Console.Write(item.ID + ","+item.SPD + ","+item.ALT + ","+item.LAT + ","+item.LNG));
            string LogSave = "";
            int lap = 1;
            if (GoHomeBt.IsChecked == true || NoGoHomeBt.IsChecked == true)
            {
                IsGoHomeSel = true;
            }
            else
            {
                IsGoHomeSel = false;
            }

            if (IsGoHomeSel)
            {
                if (MessageBox.Show("請於下方確認無人機選擇是否正確。\n共" + NavData.NavList.Count + "個導航點高度是否設定完成。(1/2)", "傳送導航資料", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK && IsGoHomeSel && FlightInfoRight.SelSend != -1)
                {
                    if (MessageBox.Show("將傳送 " + NavData.NavList.Count + "個導航點，返回起始點:" + GoHomeStr + "\n是否傳送導航資料(2/2)", "傳送導航資料", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        byte[] NavDATAByte = new byte[(NavData.NavList.Count * 24) + 4 + 1 + 1];//NavData.NavList.Count 10
                        foreach (var item in NavData.NavList)
                        {
                            Counta++;
                            Buffer.BlockCopy(BitConverter.GetBytes(item.SPD), 0, NavDATAByte, pos, 4);
                            LogSave += "[" + item.SPD + ",";
                            pos = pos + 4;
                            Buffer.BlockCopy(BitConverter.GetBytes(item.ALT), 0, NavDATAByte, pos, 4);
                            LogSave += item.ALT + ",";
                            pos = pos + 4;
                            Buffer.BlockCopy(BitConverter.GetBytes(item.NavPt.Lat), 0, NavDATAByte, pos, 8);
                            LogSave += "[" + item.NavPt.Lat + ",";
                            pos = pos + 8;
                            Buffer.BlockCopy(BitConverter.GetBytes(item.NavPt.Lng), 0, NavDATAByte, pos, 8);
                            LogSave += item.NavPt.Lng + "]],";
                            pos = pos + 8;
                            //var A = BitConverter.ToSingle(NavDATAByte.Take(4).ToArray(), 0);
                            //var B = BitConverter.ToSingle(NavDATAByte.Skip(4).Take(4).ToArray(), 0);
                            //var C = BitConverter.ToDouble(NavDATAByte.Skip(8).Take(8).ToArray(), 0);
                            //var D = BitConverter.ToDouble(NavDATAByte.Skip(16).Take(8).ToArray(), 0);
                            Console.WriteLine(item.SPD + "," + item.ALT + "," + item.NavPt.Lat + "," + item.NavPt.Lng);
                            //Console.WriteLine(A + "," + B + "," + C + "," + D);
                        }
                        Buffer.BlockCopy(BitConverter.GetBytes(lap), 0, NavDATAByte, pos, 4);
                        pos = pos + 4;
                        Buffer.BlockCopy(BitConverter.GetBytes(true), 0, NavDATAByte, pos, 1);
                        pos = pos + 1;
                        Buffer.BlockCopy(BitConverter.GetBytes(GoHome), 0, NavDATAByte, pos, 1);

                        TcpServer.SendNAV(FlightInfoRight.SelSend, NavDATAByte, LogSave, 0);
                        Console.WriteLine(NavDATAByte.Length);
                    }
                    pos = 0;
                    Counta = 1;
                }
                else
                {
                    MessageBox.Show("請於下方選擇傳送資料的對象!!", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("您尚未選擇是否反回起始點!!", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoHomeBt_Checked(object sender, RoutedEventArgs e)
        {
            if (NavIsLoad)
            {
                GoHome = true;
                GoHomeStr = "是";
                NoGoHomeBt.IsChecked = false;
            }

        }

        private void NoGoHomeBt_Checked(object sender, RoutedEventArgs e)
        {
            if (NavIsLoad)
            {
                GoHome = false;
                GoHomeStr = "否";
                GoHomeBt.IsChecked = false;
            }

        }

        private void GoHomeBt_Unchecked(object sender, RoutedEventArgs e)
        {
            if (NoGoHomeBt.IsChecked == false)
            {
                GoHomeBt.IsChecked = true;
            }
        }

        private void NoGoHomeBt_Unchecked(object sender, RoutedEventArgs e)
        {
            if (GoHomeBt.IsChecked == false)
            {
                NoGoHomeBt.IsChecked = true;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NavIsLoad = true;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavIsLoad = false;
            MainWindow.IsEdit = false;
            MainWindow.DelegateModeMapObj.Invoke(false);
        }

        private void ClearNavPtBt_Click(object sender, RoutedEventArgs e)
        {
            NavData.NavList.Clear();
            NavDGRef();
            ReqDel = true;
        }
        private void AltTxTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Console.WriteLine(AltTxTBox.Text);
            if (NavIsLoad)
            {
                AltSet = true;
                Ref.Start();
                if (AltSet != true)
                {
                    Ref.Stop();
                }
            }
            //AltSet = false;
        }
    }
}
