using DoroonNet.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoroonNet.Command
{
    //class State
    //{
    //    public Socket ClientSocket = null;
    //    public const int BufferSize = 1024;
    //    public byte[] Buffer = new byte[BufferSize];
    //    public StringBuilder MsgBuilder = new StringBuilder();
    //}

    //class DatagramState : State
    //{
    //    public EndPoint EndPoint = new IPEndPoint(IPAddress.Any, 0);
    //    public bool Recursive = false;
    //}

    public class ConnectOBJ
    {
        // Size of receive buffer.  
        public const int BufferSize = 102400;//32768 5242800
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
        public StringBuilder sab2 = new StringBuilder();

        //public MemoryStream memStream = new MemoryStream();
        //public MemoryStream o2MemoryStream = new MemoryStream();

        // Client socket.
        public Socket WorkSocket = null;
        public EndPoint EndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

  
    public class UDPServer
    {
        public static ManualResetEvent Next = new ManualResetEvent(false);
        public static MemoryStream memStream = new MemoryStream();
        public static EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 54088);
        public static ConsolePrint ConSoPt = new ConsolePrint();
        public static Stopwatch tims = new Stopwatch();


        public static int revbyte = 0;

        public void UDPStart()
        {
            Start(); //啟動 StartListening
              
        }

        public static void Start()
        {
            //EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 54088);   
            try
            {
                Socket S = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                S.Bind(new IPEndPoint(IPAddress.Any, 54088));
                S.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
                Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}][Info] UDP Server Start...");
                while (true)
                {
                    Next.Reset();
                    ConnectOBJ State = new ConnectOBJ();
                    State.WorkSocket = S;
                    tims.Start();
                    //Console.WriteLine("listenning....");
                    S.BeginReceiveFrom(State.buffer, 0, ConnectOBJ.BufferSize, 0, ref RemoteIpEndPoint, new AsyncCallback(RecieveCallback), State);

                    Next.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.Read();
        }

        public static void RecieveCallback(IAsyncResult Result)
        {
            ConnectOBJ State = (ConnectOBJ)Result.AsyncState;
            int bytes;
            Socket S = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            S.Bind(new IPEndPoint(IPAddress.Any, 0));
            Socket past = State.WorkSocket;
            State.WorkSocket = S;
            
            try
            {
                bytes = past.EndReceiveFrom(Result, ref RemoteIpEndPoint);

                if (bytes > 0)
                {
                    State.sb.Append(BitConverter.ToString(State.buffer, 0, bytes));
                    memStream.Write(State.buffer, 0, bytes);
                    Next.Set();
                    try
                    {
                        FlightInfoRightCommand FIRC = new FlightInfoRightCommand() { ImageID = (RemoteIpEndPoint as IPEndPoint).Port.ToString() };
                        #region old
                        //if (BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty).Contains("537A547A4E"))
                        //{
                        //    revbyte += 1;
                        //    //Console.WriteLine(revbyte);
                        //    ImageConverter.HexWriterTxtAsync(BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty));
                        //    ConSoPt.ConsoPrint(revbyte.ToString()+" "+(RemoteIpEndPoint as IPEndPoint).Address.ToString()+" "+ (RemoteIpEndPoint as IPEndPoint).Port.ToString());
                        //    memStream.Seek(0, SeekOrigin.Begin);
                        //    memStream.SetLength(0);
                        //}
                        //ImageConverter.HexTXTbuffer(BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty));

                        //Console.WriteLine(BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty));
                        //string r = Calculate(content).ToString();
                        //Send(r, Dstate);                    
                        //memStream.Flush();
                        #endregion

                        //if (BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty).Contains("537A547A4E"))
                        //{
                        //    Console.WriteLine($"{DateTime.Now.Ticks}-R");
                        //    //Thread.Sleep(800);
                        //    SYS.RecvTicks.Add(DateTime.Now.Ticks);
                        //    if (SYS.UdpRunTest)
                        //    {
                        //        //Task.Run(() =>
                        //        //{
                        //        //    SYS.UdpSend(past, RemoteIpEndPoint);
                        //        //});
                        //        SYS.UdpSend(past,RemoteIpEndPoint);
                        //    }
                        //    //Thread.Sleep(1000);
                        //}

                        //if (Encoding.ASCII.GetString(State.buffer, 0, bytes).Contains("hello"))
                        //{
                        //    SYS.SendIP = (RemoteIpEndPoint as IPEndPoint).Address.ToString();

                        //    if (SYS.UdpRunTest)
                        //    {
                        //        //Task.Run(() =>
                        //        //{
                        //        //    SYS.UdpSend(past, RemoteIpEndPoint);
                        //        //});
                        //        Console.WriteLine((RemoteIpEndPoint as IPEndPoint).Address.ToString());
                        //        SYS.UdpSend(past,RemoteIpEndPoint);
                        //    }
                        //}
                        //Console.WriteLine((RemoteIpEndPoint as IPEndPoint).Address.ToString());

                        ImageConverterV2.HexTXTbufferV2(BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty));

                        memStream.SetLength(0);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "!!!!!!!");
                        //Send(e.Message, Dstate);
                    }
                    
                }
                //Console.WriteLine((RemoteIpEndPoint as IPEndPoint).Address.ToString());
            }
            catch (Exception e)
            {
                Next.Set();
                Console.WriteLine(e.Message);
                //Send("Respect the buffer size which is " + DatagramState.BufferSize.ToString(), Dstate);
                return;
            }
        }
        


    }


}
