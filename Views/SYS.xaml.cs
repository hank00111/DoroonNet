using DoroonNet.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DoroonNet.Views
{
    /// <summary>
    /// SYS.xaml 的互動邏輯
    /// </summary>
    public partial class SYS : Window
    {
        double[] xx = new double[729];//894//827
        double[] yy = new double[729];
        int ix = 0;
        int iy = 0;
        UdpClient UdpTest;
        public static bool UdpRunTest = false;
        public static int Sck_con = 0;
        public static int SendCount = 0;
        public static string SendIP = "114.32.137.51";
        public static List<long> SendTicks = new List<long>();
        public static List<long> RecvTicks = new List<long>();
        //private readonly ScottPlot.Plottable.ScatterPlot MyScatterPlot;
        //private readonly ScottPlot.Plottable.ScatterPlot HighlightedPoint;
        //private int LastHighlightedIndex = -1;

        public SYS()
        {
            InitializeComponent();
            SendSpeedTextBox.Text = UdpServerV3.SendSpeed.ToString();
            SpeedLabel.Content = $"發送速度:{UdpServerV3.SendSpeed} ms";
            //FF();
        }

        private void FF()
        {
            //var plt = new ScottPlot.Plot(600, 400);

            // sample data
            string path = @"C:\Users\001\Desktop\X.txt";
            string path2 = @"C:\Users\001\Desktop\Y.txt";
            //lat 1/101.77545*100000
            //lng 1/110.9362*100000
            // This text is added only once to the file.

            // This text is always added, making the file longer over time
            // if it is not deleted.
            //string appendText = "This is extra text" + Environment.NewLine;
            //File.AppendAllText(path, appendText);

            // Open the file to read from.
            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                double.TryParse(s, out double x);
                xx[ix] = x * (1 / 110.9362 * 100000);
                //Console.WriteLine(x);
                ix += 1;
            }
            string[] yyyy = File.ReadAllLines(path2);
            foreach (string s in yyyy)
            {
                double.TryParse(s, out double y);
                yy[iy] = y * (1 / 101.77545 * 100000);
                iy += 1;
            }


            // plot the data
            //FK.Plot.AddScatter(xx, yy);//lineWidth: 0
            //                           //FK.Plot.AddScatter(xs, cos);


            //// customize the axis labels
            ////FK.Plot.Title("ScottPlot Quickstart");
            ////FK.Plot.XLabel("Horizontal Axis");
            ////FK.Plot.YLabel("Vertical Axis");
            //FK.Render();
            //FK.Refresh();
        }

        private void FK_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void UDPTestBT_Click(object sender, RoutedEventArgs e)
        {
            //UdpServerV3.Calculate = true;
            if (UdpServerV3.Calculate)
            {
                UdpServerV3.Calculate = false;
                UdpServerV3.CalculateOK = false;
                //UdpServerV3.t2.Start();
            }
            else UdpServerV3.Calculate = true;
            //if (UdpServerV3.Calculate)
            //{
            //    UdpRunTest = false;
            //}else UdpRunTest = true;

            //Task.Run(() => {
            //    UdpSend();
            //});
        }

        //static FileStream f = new FileStream(@"E:\Program\DoroonNet\bin\Debug\net5.0-windows\ImageCache\1.jpg", FileMode.Open);
        public static void UdpSend(Socket s, EndPoint En)
        {
            //while (UdpRunTest)//Sck_con < 1000
            //{
            //    if (UdpRunTest)
            //    {
            //        try
            //        {
            //            Sck_con += 1;
            //            //byte[] b = System.Text.Encoding.UTF8.GetBytes("hello");
            //            //byte[] Start = new byte[] { 0x01, 0x53, 0x54, 0x48, 0x01 };
            //            //string FileTest = FlieName[img_con];
            //            string end = "SzTzN";

            //            ////Console.WriteLine(System.IO.Path.Combine(@"E:\Program\Doroon\bin\Debug\ImgCaches", FileTest));//fileName                       
            //            //C:\Users\user\Desktop\1.jpg
            //            //E:\Program\DoroonNet\bin\Debug\net5.0-windows\ImageCache\1.jpg
            //            //if (Sck_con == 1)
            //            //{
            //            //    UdpTest = new UdpClient();
            //            //}
            //            ////uc.Send(Start, Start.Length, new IPEndPoint(IPAddress.Parse("192.168.0.150"), 54088));
            //            //UdpTest.Send(b, b.Length, new IPEndPoint(IPAddress.Parse(SendIP), 54088));

            //            //while (true)
            //            //{
            //            //    byte[] bits = new byte[4096];
            //            //    int r = f.Read(bits, 0, bits.Length);
            //            //    if (r <= 0) break;
            //            //    //UdpTest.Send(bits, r, new IPEndPoint(IPAddress.Parse(SendIP), 54088));
            //            //    s.SendTo(bits, r, SocketFlags.None, En);
            //            //    Thread.Sleep(1);
            //            //}

            //            //var args = new SocketAsyncEventArgs { RemoteEndPoint = En };
            //            //args.SetBuffer(System.Text.Encoding.Default.GetBytes(end), 0, end.Length);
            //            s.SendTo(System.Text.Encoding.Default.GetBytes(end), 0, end.Length,SocketFlags.None, En);
                      
            //            SendTicks.Add(DateTime.Now.Ticks);
            //            Console.WriteLine($"{DateTime.Now.Ticks}-S");//ToString("HH:mm:ss:ffff")
            //            f.Position = 0;
            //            //f.Close();
            //            ////img_con += 1;
            //            Thread.Sleep(1000);
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //    else
            //    {
            //        //UdpTest.Close();
            //        //CalculateMs();
            //        f.Close();
            //        Sck_con = 0;
            //        break;
            //    }                
            //}
            //CalculateMs();
        }

        private static void CalculateMs()
        {
            if (RecvTicks.Count > SendTicks.Count)
            {
                List<long> latency = new List<long>();
                for (int i = 0; i < SendTicks.Count; i++)
                {
                    var ms = RecvTicks[i] - SendTicks[i];
                    latency.Add(ms);
                }
                TimeSpan elapsedSpan = new TimeSpan((long)latency.Average());
                Console.WriteLine($"{elapsedSpan.TotalMilliseconds}ms {elapsedSpan.TotalSeconds}S");
            }
            else
            {
                List<long> latency = new List<long>();
                for (int i = 0; i < RecvTicks.Count; i++)
                {
                    var ms = RecvTicks[i] - SendTicks[i];
                    latency.Add(ms);
                }
                TimeSpan elapsedSpan = new TimeSpan((long)latency.Average());
                Console.WriteLine($"{elapsedSpan.TotalMilliseconds}ms {elapsedSpan.TotalSeconds}S");
            }
        }

        private void SetSpeedBT_Click(object sender, RoutedEventArgs e)
        {
            //SendSpeedTextBox.Text = UdpServerV3.SendSpeed.ToString();
            UdpServerV3.SendSpeed = int.Parse(SendSpeedTextBox.Text);
            SpeedLabel.Content = $"發送速度:{UdpServerV3.SendSpeed} ms";
        }
    }
}
