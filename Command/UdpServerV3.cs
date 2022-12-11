using DoroonNet.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoroonNet.Command
{
    class UdpServerV3
    {
        static Socket server;


        public static Thread t1, t2;
        public static int SendSpeed = 200;

        private static EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        public static MemoryStream memStream = new MemoryStream();
        public static List<long> SendTicks = new List<long>();
        public static List<long> RecvTicks = new List<long>();
        public static bool CanSend = false;
        public static bool Calculate = false;
        public static bool CalculateOK = false;
        private static string FilePath = MainWindow.Func_exe_path() + @"\ImageCache\1.jpg";
        private static FileStream f = new FileStream(FilePath, FileMode.Open);

        //C:\Users\user\Desktop\
        //E:\Program\DoroonNet\bin\Debug\net5.0-windows\ImageCache\
        public void UDPStart()
        {
            //Start(); //啟動 StartListening
            Init();
        }

        private static void Init()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Any, 54088));//繫結埠號和IP
            Console.WriteLine("服務端已經開啟");
            t1 = new Thread(ReciveMsg);//開啟接收訊息執行緒
            t1.Start();
            t2 = new Thread(sendMsg);//開啟發送訊息執行緒
            t2.Start();
        }

        private static void ReciveMsg()
        {
            while (true)
            {
                //RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);//用來儲存傳送方的ip和埠號
                byte[] buffer = new byte[102400];
                int length = server.ReceiveFrom(buffer, ref RemoteIpEndPoint);//接收資料報
                if(length > 0)
                {
                    try
                    {
                        FlightInfoRightCommand FIRC = new FlightInfoRightCommand() { ImageID = (RemoteIpEndPoint as IPEndPoint).Port.ToString() };
                        memStream.Write(buffer, 0, length);
                        //string message = Encoding.UTF8.GetString(buffer, 0, length);                        
                        if (BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty).Contains("537A547A4E"))
                        {
                            //CanSend = true;
                            if (!Calculate)
                            {
                                RecvTicks.Add(DateTime.Now.Ticks);
                                Console.WriteLine($"{DateTime.Now.Ticks}-R");
                            }       
                            //sendMsg(RemoteIpEndPoint);
                        }

                        if (Encoding.ASCII.GetString(memStream.ToArray(), 0, length).Contains("hello"))
                        {
                            CanSend = true;
                            Console.WriteLine(RemoteIpEndPoint.ToString());
                            //sendMsg(RemoteIpEndPoint);
                            //SYS.SendIP = (RemoteIpEndPoint as IPEndPoint).Address.ToString();

                            //if (SYS.UdpRunTest)
                            //{
                            //    //Task.Run(() =>
                            //    //{
                            //    //    SYS.UdpSend(past, RemoteIpEndPoint);
                            //    ////});
                            //    //Console.WriteLine((RemoteIpEndPoint as IPEndPoint).Address.ToString());
                            //    //SYS.UdpSend(past, RemoteIpEndPoint);
                            //}
                        }
                        //ImageConverterV2.HexTXTbufferV2(BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty));
                        memStream.SetLength(0);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "!!!!!!!");
                    }
                }                
            }
        }

        private static void sendMsg()
        {
            while (true)
            {
                if (CanSend)
                {

                    string end = "SzTzN";
                    Thread.Sleep(SendSpeed);
                    while (true)
                    {
                        byte[] bits = new byte[4096];
                        int r = f.Read(bits, 0, bits.Length);
                        if (r <= 0) break;                      
                        server.SendTo(bits, r, SocketFlags.None, RemoteIpEndPoint);
                        Thread.Sleep(1);
                    }
                    server.SendTo(Encoding.UTF8.GetBytes(end), SocketFlags.None, RemoteIpEndPoint);
                    SendTicks.Add(DateTime.Now.Ticks);
                    Console.WriteLine($"{DateTime.Now.Ticks}-S Speed:{SendSpeed}");
                    f.Position = 0;
                    //f.Close();
                }

                if (Calculate && !CalculateOK)
                {
                    CalculateMs();
                }
                Thread.Sleep(1);
            }
            //Console.WriteLine("Close");
            //CalculateMs();
        }
        private static void CalculateMs()
        {
            if (SendTicks.Count != 0 && RecvTicks.Count != 0)
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
                    Console.WriteLine($"{elapsedSpan.TotalMilliseconds}ms {elapsedSpan.TotalSeconds}S SendCount:{SendTicks.Count} RecvCount:{RecvTicks.Count}");
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
                    Console.WriteLine($"{elapsedSpan.TotalMilliseconds}ms {elapsedSpan.TotalSeconds}S SendCount:{SendTicks.Count} RecvCount:{RecvTicks.Count}");
                }
                CalculateOK = true;
                CanSend = false;
                SendTicks.Clear();
                RecvTicks.Clear();
                f.Close();
                f = new FileStream(FilePath, FileMode.Open);
            }
            //t2.r;
        }
    }
}
