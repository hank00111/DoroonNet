using DoroonNet.DoroonDB;
using DoroonNet.ViewModel;
using DoroonNet.Views;
using GMap.NET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DoroonNet.Command
{
    public class ConnectObject
    {
        public const int BufferSize = 102400;
        public byte[] buffer = new byte[BufferSize];
        public Socket workSocket = null;
    }

    public class GData
    {
        public int X = 0;
        //int Y = 1;
        //int Z = 2;
        public double[,] Gdata1 = new double[3, 4000];
    }

    public class SocketClients
    {
        public int id { get; set; }
        public Socket socket { get; set; }
        public PointLatLng Pos { get; set; }
    }

    public class TcpServer
    {
        static Socket listener;
        static DoroonSQLLiteDB DoroonDB = new DoroonSQLLiteDB();
        static ConsolePrint ConSoPt = new ConsolePrint();
        static MemoryStream memStream = new MemoryStream();
        static ManualResetEvent allDone = new ManualResetEvent(false);
        static InfoViewModel ins = new InfoViewModel();
        static List<FlightListData> FlightList;
        static int RecvCount = 0;
        static int LossCount = 0;
        static int OtherCount = 0;
        static int CurrentFlightID;
        static string SysDir = MainWindow.Func_exe_path();
        static string LogPath = SysDir + @"\Log\";
        static string TcpHost = Properties.Settings.Default.TCPHost;
        static string SysOpen = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fffff");
        static Hashtable map = new Hashtable();
        static Dictionary<string, DBFlightData> dicFlightData = new Dictionary<string, DBFlightData>();
        public static List<FlightDataL> CLP = new List<FlightDataL>();
        public static List<Socket> Clients = new List<Socket>();
        public static List<SocketClients> Clisents = new List<SocketClients>();
        public static List<IPAddress> NetIPs = new List<IPAddress>();  
        public static int MapClients = 0;
        public static int LinkID;
        public static int CurrentMapClients;
        public static int CurrentMoveClients = 0;
        public static int NewCurrentMoveClients;
        //public static int FirstClients;
        //public static bool test = false;
        public static string CurrentHost;

        public void AsyncSocketListeners()
        {
            //test = true;
            StartListening(); //啟動 StartListening

        }

        public static void StartListening()
        {
            int ip = 0;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            for (int index = 0; index < ipHostInfo.AddressList.Length; index++)
            {
                NetIPs.Add(ipHostInfo.AddressList[index]);
                if (ipHostInfo.AddressList[index].ToString().Contains(Properties.Settings.Default.TCPHost))
                {
                    ip = index;
                }
                //Console.WriteLine(ipHostInfo.AddressList[index]);
            }
            #region Set IP/Port
            IPAddress ipAddress = ipHostInfo.AddressList[ip];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 7414);//7414 8080
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ConSoPt.ConsoPrint("TCP Using " + ipAddress.ToString() + " " + localEndPoint);
            CurrentHost = ipHostInfo.AddressList[ip].ToString();
            #endregion
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                //listener.IOControl(IOControlCode.KeepAliveValues, KeepAlive(1, 1000, 1000), null);
                while (true)
                {
                    allDone.Reset(); // Start an asynchronous socket to listen for connections.  
                    ConSoPt.ConsoPrint("[Debug] " + "Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();// Wait until a connection is made before continuing.                      
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            //Console.Read();
        }
        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;// Get the socket that handles the client request.  
            Socket handler = listener.EndAccept(ar);
            ConnectObject state = new ConnectObject();// Create the state object.  
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, ConnectObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            ClientComp(handler);
            //ConSoPt.ConsoPrint("Client " + IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString())
            //    + " on port " + ((IPEndPoint)handler.RemoteEndPoint).Port.ToString());
        }
        public static void ReadCallback(IAsyncResult ar)
        {
            ConnectObject state = (ConnectObject)ar.AsyncState;
            Socket handler = state.workSocket;
            try
            {
                int bytesRead = handler.EndReceive(ar);
      
                if (bytesRead > 0)
                {
                    memStream.Write(state.buffer, 0, bytesRead);
                    //LinkID = ((IPEndPoint)handler.RemoteEndPoint).Port;
                    //Console.WriteLine("STAR"+BitConverter.ToString(memStream.ToArray())+" END");
                    //Console.WriteLine(bytesRead);
                    Task iD = ID(((IPEndPoint)handler.RemoteEndPoint).Port);
                    //ID(((IPEndPoint)handler.RemoteEndPoint).Port);
                    //NewCurrentMoveClients = ((IPEndPoint)handler.RemoteEndPoint).Port;

                    if (bytesRead == 71 && BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty).Contains("30"))
                    {
                        int ClientDataCheck = Clients.FindIndex(X => X.RemoteEndPoint == handler.RemoteEndPoint);
                        //LinkID = ((IPEndPoint)handler.RemoteEndPoint).Port;
 
                        CurrentMoveClients = ClientDataCheck;
                        if (ClientDataCheck == XYZChart.SelClient) WaveDataConvert(ClientDataCheck, memStream.ToArray());
                        //Console.WriteLine("ClientDataCheck:{0} SelClient{1} CurrentMoveClients{2}", ClientDataCheck, XYZChart.SelClient, CurrentMoveClients);

                        if (FlightList != null && ClientDataCheck != -1)
                        {
                            int CurrentFlightID = FlightList.IndexOf(FlightList.FirstOrDefault(X => int.Parse(X.linkid) == ((IPEndPoint)handler.RemoteEndPoint).Port)) + 1;
                            //byte[] Data = memStream.ToArray();
                            FlightDataConvert(handler, memStream.ToArray());
                        }
                        RecvCount += 1;
                        //Console.WriteLine("STAR "+BitConverter.ToString(memStream.ToArray())+" END");

                        memStream.SetLength(0);
                    }
                    else if (bytesRead > 71 && BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty).Contains("30"))
                    {
                        //Console.WriteLine("URA " + memStream.Length);
                        RecvCount += 1;
                        BigBytes(memStream, bytesRead, handler);
                    }
                    else
                    {
                        LossCount += 1;
                        RecvCount -= 1;
                        Console.WriteLine("Data Error");
                    }

                    if(bytesRead == 2 && BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty).Contains("49"))
                    {
                        int ClientDataCheck = Clisents.FindIndex(X => X.socket.RemoteEndPoint == handler.RemoteEndPoint);
                        Clisents[ClientDataCheck].id = memStream.ToArray()[1];

                    }
                    memStream.SetLength(0);
                    //Console.WriteLine("STAR " + BitConverter.ToString(memStream.ToArray()) + " END");
                    //Console.WriteLine("#"+((IPEndPoint)handler.RemoteEndPoint).Port.ToString()+" "+ bytesRead);
                    memStream.Flush();
                }
                else
                {
                    ClinetDel(handler);
                }

                if (bytesRead > 1)
                {
                    handler.BeginReceive(state.buffer, 0, ConnectObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
                else
                {
                    // Not all data received. Get more.  
                    //handler.BeginReceive(state.buffer, 0, ConnectObject.BufferSize, 0,
                    //new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception e)
            {
                ClinetDel(handler);

            }


        }
        private static Task ID(int port)
        {
            NewCurrentMoveClients = port;
            //Console.WriteLine(NewCurrentMoveClients);
            return Task.CompletedTask;
        }
        private static void IDCHG()
        {
            foreach(var s in Clisents)
            {
                ins.CollectionListPartial[Clients.Count - 1].ID = FlightList.Count;
            }
        }

        #region Send
        private static void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
        public static void SendKeep()
        {
            byte[] byteData = new byte[1];
            for (int i = 0; i < Clients.Count; i++)
            {
                //Console.WriteLine(Clients[i]);
                var ClientHandelr = Clients[i];
                try
                {
                    ClientHandelr.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), ClientHandelr);
                }
                catch
                {
                    Console.WriteLine(Clients[i]);
                    //
                    ClinetDel(ClientHandelr);
                }
            }
        }

        public static void SendCAM(int Client, int CAM)
        {

            int lag = 213;
            byte[] Data = BitConverter.GetBytes(lag);
            byte[] byteData = Encoding.ASCII.GetBytes(((DoroonCAMEnum)CAM).ToString());

            bool f = true;
            byte[] bf = BitConverter.GetBytes(f);

            //byte[] FinalData = new byte[byteData.Length + Data.Length];
            byte[] FinalData = new byte[bf.Length];
            Buffer.BlockCopy(bf, 0, FinalData, 0, bf.Length);
            //Buffer.BlockCopy(byteData, 0, FinalData, 0, byteData.Length);
            //Buffer.BlockCopy(Data, 0, FinalData, byteData.Length, Data.Length);

            Console.WriteLine(Client);
            try
            {
                if (Clients[Client] != null)
                {
                    var ClientHandelr = Clients[Client];
                    ClientHandelr.BeginSend(FinalData, 0, FinalData.Length, 0,
                    new AsyncCallback(SendCallback), ClientHandelr);
                }
            }
            catch
            {
                Console.WriteLine("Client 0");
            }

        }

        public static void SendControl(int Client, int TakeoffOrLanding)
        {

            byte[] HeadByte = Encoding.ASCII.GetBytes("CUAV");
            byte[] TailByte = Encoding.ASCII.GetBytes("TUAV");
            byte[] ToLByte = Encoding.ASCII.GetBytes(TakeoffOrLanding.ToString());
            byte[] FinalData = new byte[HeadByte.Length + TailByte.Length + ToLByte.Length];
            Buffer.BlockCopy(HeadByte, 0, FinalData, 0, HeadByte.Length);
            Buffer.BlockCopy(ToLByte, 0, FinalData, HeadByte.Length, ToLByte.Length);
            Buffer.BlockCopy(TailByte, 0, FinalData, HeadByte.Length + ToLByte.Length, TailByte.Length);

            try
            {
                if (Clients[Client] != null && Clients.Count == 1)
                {
                    var ClientHandelr = Clients[Client];
                    ClientHandelr.BeginSend(FinalData, 0, FinalData.Length, 0,
                    new AsyncCallback(SendCallback), ClientHandelr);
                    SendLog($"[{ DateTime.Now.ToString("HH:mm:ss:fffff")}] SendTo#{((IPEndPoint)ClientHandelr.RemoteEndPoint).Port} - CUAV,{TakeoffOrLanding}TUAV\r\n");
                }
                else if (Clients[Client] != null && Clients.Count > 1)
                {
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        //Console.WriteLine(Clients[i]);
                        var ClientHandelr = Clients[i];
                        ClientHandelr.BeginSend(FinalData, 0, FinalData.Length, 0,
                            new AsyncCallback(SendCallback), ClientHandelr);
                        SendLog($"[{ DateTime.Now.ToString("HH:mm:ss:fffff")}] SendTo#{((IPEndPoint)ClientHandelr.RemoteEndPoint).Port} - CUAV,{TakeoffOrLanding}TUAV\r\n");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Client 0");
            }

        }

        public static void SendNAV(int Client, byte[] NavData, string log, int Mode)
        {
            byte[] HeadByte = Mode != 1 ? Encoding.ASCII.GetBytes("GUAV") : Encoding.ASCII.GetBytes("EUAV");
            byte[] TailByte = Encoding.ASCII.GetBytes("TUAV");
            int DataLength = HeadByte.Length + NavData.Length + TailByte.Length + 4;
            byte[] DataLengthByte = BitConverter.GetBytes(DataLength);

            byte[] FinalData = new byte[HeadByte.Length + TailByte.Length + NavData.Length + DataLengthByte.Length];

            Buffer.BlockCopy(HeadByte, 0, FinalData, 0, HeadByte.Length);
            Buffer.BlockCopy(DataLengthByte, 0, FinalData, HeadByte.Length, DataLengthByte.Length);
            Buffer.BlockCopy(NavData, 0, FinalData, HeadByte.Length + 4, NavData.Length);
            Buffer.BlockCopy(TailByte, 0, FinalData, HeadByte.Length + NavData.Length + DataLengthByte.Length, TailByte.Length);
            //12
            try
            {
                if (Clients[Client] != null)
                {
                    var ClientHandelr = Clients[Client];
                    ClientHandelr.BeginSend(FinalData, 0, FinalData.Length, 0,
                    new AsyncCallback(SendCallback), ClientHandelr);

                    //using (FileStream fs = new FileStream(LogPath + SysOpen + ".txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    //{
                    //    using (StreamWriter StreamWriter = new StreamWriter(fs, Encoding.UTF8))
                    //    {
                    //        StreamWriter.Write(BitConverter.ToString(FinalData.ToArray()).Replace("-", string.Empty));
                    //        StreamWriter.Close();
                    //        StreamWriter.Dispose();
                    //    }
                    //    fs.Close();
                    //    fs.Dispose();
                    //}
                    SendLog($"[{ DateTime.Now.ToString("HH:mm:ss:fffff")}] SendTo#{((IPEndPoint)ClientHandelr.RemoteEndPoint).Port} - {BitConverter.ToString(FinalData.ToArray()).Replace("-", string.Empty)}--GUAV,{log}TUAV\r\n\r\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client 0");
                MessageBox.Show("傳送錯誤!!\n" + ex, "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public static void SendNAVFW(byte[] NavData,float x,float y,float z, string log, int Mode)
        {
            byte[] HeadByte = Mode != 1 ? Encoding.ASCII.GetBytes("GUAV") : Encoding.ASCII.GetBytes("EUAV");
            byte[] TailByte = Encoding.ASCII.GetBytes("TUAV");
            int DataLength = HeadByte.Length + NavData.Length + TailByte.Length + 4;
            byte[] DataLengthByte = BitConverter.GetBytes(DataLength);

            byte[] FinalData = new byte[HeadByte.Length + TailByte.Length + NavData.Length + DataLengthByte.Length];
            //byte[] OffSetData = new byte[20];

            Buffer.BlockCopy(HeadByte, 0, FinalData, 0, HeadByte.Length);
            Buffer.BlockCopy(DataLengthByte, 0, FinalData, HeadByte.Length, DataLengthByte.Length);
            Buffer.BlockCopy(NavData, 0, FinalData, HeadByte.Length + 4, NavData.Length);
            Buffer.BlockCopy(TailByte, 0, FinalData, HeadByte.Length + NavData.Length + DataLengthByte.Length, TailByte.Length);



            try
            {
                if (Clients != null)
                {
                    int ClientLeaderCheck = Clisents.FindIndex(X => X.id == 0);
                    var ClientHandelr = Clisents[ClientLeaderCheck].socket;
                    ClientHandelr.BeginSend(FinalData, 0, FinalData.Length, 0,
                    new AsyncCallback(SendCallback), ClientHandelr);
                    int zz = 0;
                    foreach(var H in Clisents)
                    {
                        //if()
                        //byte[] OffSetData = new byte[20];
                        //Buffer.BlockCopy(Encoding.ASCII.GetBytes("OUAV"), 0, OffSetData, 0, 4);
                        //Buffer.BlockCopy(BitConverter.GetBytes(x*z), 0, OffSetData, 4, 4);
                        //Buffer.BlockCopy(BitConverter.GetBytes(y), 0, OffSetData, 8, 4);
                        //Buffer.BlockCopy(BitConverter.GetBytes(z), 0, OffSetData, 12, 4);
                        //Buffer.BlockCopy(Encoding.ASCII.GetBytes("TUAV"), 0, OffSetData, 16, 4);
                        zz++;
                        int ClientFWCheck = Clisents.FindIndex(X => X.id == zz);
                        if (ClientFWCheck != -1)
                         {
                            //zz++;
                            byte[] OffSetData = new byte[20];
                            Buffer.BlockCopy(Encoding.ASCII.GetBytes("OUAV"), 0, OffSetData, 0, 4);
                            Buffer.BlockCopy(BitConverter.GetBytes(x * zz), 0, OffSetData, 4, 4);
                            Buffer.BlockCopy(BitConverter.GetBytes(y * zz), 0, OffSetData, 8, 4);
                            Buffer.BlockCopy(BitConverter.GetBytes(z * zz ), 0, OffSetData, 12, 4);
                            Buffer.BlockCopy(Encoding.ASCII.GetBytes("TUAV"), 0, OffSetData, 16, 4);
                            var FwHandelr = Clisents[ClientFWCheck].socket;
                            FwHandelr.BeginSend(FinalData, 0, FinalData.Length, 0,
                                new AsyncCallback(SendCallback), FwHandelr);
                            FwHandelr.BeginSend(OffSetData, 0, OffSetData.Length, 0,
                                new AsyncCallback(SendCallback), FwHandelr);
                        }
                        Console.WriteLine($"ClientFWCheck:{0}", ClientFWCheck);
                        //break;
                    }

                    //int FWCheck = Clisents.FindIndex(X => X.id == 2);
                    //var OffSetHandelr = Clisents[1].socket;
                    //OffSetHandelr.BeginSend(NavOffSetData, 0, NavOffSetData.Length, 0,
                    //new AsyncCallback(SendCallback), OffSetHandelr);


                    SendLog($"[{ DateTime.Now.ToString("HH:mm:ss:fffff")}] SendTo#{((IPEndPoint)ClientHandelr.RemoteEndPoint).Port} - {BitConverter.ToString(FinalData.ToArray()).Replace("-", string.Empty)}--GUAV+OUAV,{log}TUAV\r\n\r\n");
                    MessageBox.Show("傳送成功!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client 0");
                MessageBox.Show("傳送錯誤!!\n" + ex, "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;// Retrieve the socket from the state object.   
                int bytesSent = handler.EndSend(ar); // Complete sending the data to the remote device.  
                if (bytesSent > 1)
                {
                    //MessageBox.Show("傳送成功!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Sent {0} bytes to {1}.傳送成功!", bytesSent, ((IPEndPoint)handler.RemoteEndPoint).Port);
                    Console.ResetColor();
                }
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion

        //歐拉角 XYZ 180 -180 0-360
        #region Other
        private static void WaveDataConvert(int CurrentID, byte[] WaveByte)
        {
            XYZChart.IsDisconnect = false;
            //int ID = BitConverter.ToInt16(WaveByte.Skip(71).Take(4).ToArray(), 0);
            if (WaveByte.Length > 71)
            {

                //Console.WriteLine("ID " + CurrentID + " Byte ID" + ID);
            }
            if (WaveByte.Length == 71)
            {
                //GData data = new GData();
                //data.Gdata1
                XYZChart.X = BitConverter.ToSingle(WaveByte.Skip(1).Take(4).ToArray(), 0);
                XYZChart.Y = BitConverter.ToSingle(WaveByte.Skip(5).Take(4).ToArray(), 0);
                XYZChart.Z = BitConverter.ToSingle(WaveByte.Skip(9).Take(4).ToArray(), 0);
                XYZChart.X1 = BitConverter.ToSingle(WaveByte.Skip(13).Take(4).ToArray(), 0);
                XYZChart.Y1 = BitConverter.ToSingle(WaveByte.Skip(17).Take(4).ToArray(), 0);
                XYZChart.Z1 = BitConverter.ToSingle(WaveByte.Skip(21).Take(4).ToArray(), 0);
                XYZChart.X2 = BitConverter.ToSingle(WaveByte.Skip(25).Take(4).ToArray(), 0);
                XYZChart.Y2 = BitConverter.ToSingle(WaveByte.Skip(29).Take(4).ToArray(), 0);
                XYZChart.Z2 = BitConverter.ToSingle(WaveByte.Skip(33).Take(4).ToArray(), 0);
            }
            //Console.WriteLine("ID " + CurrentID);
            //WaveByte = null;
        }
        public static async void FlightDataConvert(Socket handler, byte[] LatLngByte)
        {
            XYZChart.IsDisconnect = false;
            try
            {                
                if (LatLngByte.Length == 71 && LatLngByte != null)
                {
                    byte id = LatLngByte[69];
                    int selClient = Clisents.FindIndex(X => X.id == id);
                    if (id == selClient)
                    {
                        List<FlightListData> FindID = FlightList.FindAll(x => int.Parse(x.linkid) == ((IPEndPoint)Clisents[selClient].socket.RemoteEndPoint).Port);
                        int CurrentID = FindID[FindID.Count - 1].FlightID;

                        float X = BitConverter.ToSingle(LatLngByte.Skip(1).Take(4).ToArray(), 0);
                        float Y = BitConverter.ToSingle(LatLngByte.Skip(5).Take(4).ToArray(), 0);
                        float Z = BitConverter.ToSingle(LatLngByte.Skip(9).Take(4).ToArray(), 0);
                        float X1 = BitConverter.ToSingle(LatLngByte.Skip(13).Take(4).ToArray(), 0);
                        float Y1 = BitConverter.ToSingle(LatLngByte.Skip(17).Take(4).ToArray(), 0);
                        float Z1 = BitConverter.ToSingle(LatLngByte.Skip(21).Take(4).ToArray(), 0);
                        float X2 = BitConverter.ToSingle(LatLngByte.Skip(25).Take(4).ToArray(), 0);
                        float Y2 = BitConverter.ToSingle(LatLngByte.Skip(29).Take(4).ToArray(), 0);
                        float Z2 = BitConverter.ToSingle(LatLngByte.Skip(33).Take(4).ToArray(), 0);
                        int HDG = BitConverter.ToInt32(LatLngByte.Skip(37).Take(4).ToArray(), 0);
                        float SPD = MathF.Round(BitConverter.ToSingle(LatLngByte.Skip(41).Take(4).ToArray(), 0), 2);//
                        float ALT = MathF.Round(BitConverter.ToSingle(LatLngByte.Skip(45).Take(4).ToArray(), 0), 2);
                        double LAT = Math.Round(BitConverter.ToDouble(LatLngByte.Skip(49).Take(8).ToArray(), 0), 12);
                        double LNG = Math.Round(BitConverter.ToDouble(LatLngByte.Skip(57).Take(8).ToArray(), 0), 12);
                        int BAT = BitConverter.ToInt32(LatLngByte.Skip(65).Take(4).ToArray());
                        //Clisents[id].Pos = new PointLatLng(LAT, LNG);

                        if (LAT != 0)
                        {
                            Clisents[id].Pos = new PointLatLng(LAT, LNG);
                        }


                        DBFlightData f = new DBFlightData
                        {
                            FlightID = CurrentID,
                            latitude = LAT,
                            longitude = LNG,
                            altitude = ALT,
                            groundspeed = SPD,
                            heading = HDG,
                            X = X,
                            Y = Y,
                            Z = Z,
                            X1 = X1,
                            Y1 = Y1,
                            Z1 = Z1,
                            X2 = X2,
                            Y2 = Y2,
                            Z2 = Z2,
                            datetimestamp = DateTime.Now.Ticks
                        };
                        await Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            try
                            {
                                ins.CollectionListPartial[selClient].FlightHDG = HDG;
                                ins.CollectionListPartial[selClient].FlightSPD = SPD;
                                ins.CollectionListPartial[selClient].FlightALT = ALT;
                                ins.CollectionListPartial[selClient].FlightLAT = LAT;
                                ins.CollectionListPartial[selClient].FlightLNG = LNG;
                                ins.CollectionListPartial[selClient].FlightBAT = BAT;

                                CLP[selClient].FlightHDG = HDG;
                                CLP[selClient].FlightSPD = SPD;
                                CLP[selClient].FlightALT = ALT;
                                CLP[selClient].FlightLAT = LAT;
                                CLP[selClient].FlightLNG = LNG;

                                if (f.latitude != 0)
                                {
                                    //int ck = dicFlightData[dicFlightData.Keys.Count - 1].latitude - f.latitude;
                                    //if (ck > 0.00005)
                                    //{
                                    //    Console.WriteLine(ck);
                                    //}                                    
                                    dicFlightData.Add(dicFlightData.Count.ToString(), f);
                                }
                                
                                if (dicFlightData.Count > 60)
                                {
                                    //foreach (KeyValuePair<string, FlightData> v in dicFlightData)            
                                    if (DoroonDB.IsRun)
                                    {
                                        dicFlightData.Clear();
                                        //Console.WriteLine(dicFlightData.Count);
                                        //Console.WriteLine("Clear");
                                        //Console.WriteLine(dicFlightData.Count);
                                        DoroonDB.IsRun = false;
                                    }
                                    else
                                    {
                                        Task a = DoroonDB.WriteFlightPointV2(dicFlightData);
                                    }
                                    //Console.WriteLine(dicFlightData.Count);
                                }
                                //Console.WriteLine("ID:{0}:{1}", CurrentID,selClient);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            //DoroonDB.WriteFlightPoint(CurrentID, LAT, LNG, ALT, SPD, HDG, X, Y, Z, X1, Y1, Z1, X2, Y2, Z2);
                            LatLngByte = null;
                        });

                    }
                }
                else LatLngByte = null;
                //await Task.Run(() => Console.WriteLine("Pushing new call {0} with {1} id"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public static void ClientComp(Socket handler)
        {
            FlightInfoRightCommand firc = new FlightInfoRightCommand();
            //LinkID = ((IPEndPoint)handler.RemoteEndPoint).Port;
            var ClientID = ((IPEndPoint)handler.RemoteEndPoint).Port;
            //map
            firc.DataID = ClientID.ToString();
            Clients.Add(handler);
            Clisents.Add(new SocketClients { id = Clisents.Count, socket = handler });
            DoroonDB.WriteFlight(ClientID.ToString());
            FlightList = new List<FlightListData>();
            FlightList = DoroonDB.GetFlightID();
            //Console.WriteLine(FlightList.Count);
            Console.WriteLine(((IPEndPoint)handler.RemoteEndPoint).Port.ToString());
            if (Clients.Count == ins.CollectionListPartial.Count && Clients.Count != 0)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    ins.CollectionListPartial[Clients.Count - 1].ID = FlightList.Count;
                    ins.CollectionListPartial[Clients.Count - 1].FlightID = "#" + ClientID.ToString();
                });
                CLP.Add(new FlightDataL()
                { ID = FlightList.Count, FlightID = "#" + ClientID.ToString() });
            }
            else if (Clients.Count > ins.CollectionListPartial.Count)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    ins.CollectionListPartial.Add(new FlightData() { ID = FlightList.Count, FlightID = "#" + ClientID });
                });
                CLP.Add(new FlightDataL() { ID = FlightList.Count, FlightID = "#" + ClientID.ToString() });
            }
            //foreach (var s in CLP)
            //{
            //    Console.WriteLine(s.FlightID);
            //}
            //MainWindow.MulitDroneLocation();
            MapClients += 1;
            int ClientDataCheck = Clients.IndexOf(Clients.FirstOrDefault(X => X.RemoteEndPoint == handler.RemoteEndPoint));
            NewCurrentMoveClients = ClientID;
            MainWindow.DelegateAddAircraftObj.Invoke(NewCurrentMoveClients, ClientDataCheck, FlightList.Count);
            ConSoPt.ConsoPrint("#" + ClientID + " Connected "
                + ins.CollectionListPartial.Count.ToString() + " " + Clients.Count.ToString());
        }
        private static void ClinetDel(Socket handler)
        {
            FlightInfoRightCommand FIRC = new FlightInfoRightCommand() { DataID = "" };
            int DelClient = Clients.IndexOf(Clients.FirstOrDefault(X => X.RemoteEndPoint == handler.RemoteEndPoint));
            int DBid = FlightList.FindIndex(X => X.linkid == ((IPEndPoint)handler.RemoteEndPoint).Port.ToString());
            //CurrentMoveClients = 0;
            CurrentMapClients = ((IPEndPoint)handler.RemoteEndPoint).Port;
            CLP.RemoveAt(DelClient);
            Clisents.RemoveAt(DelClient);
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                ins.CollectionListPartial.RemoveAt(DelClient);
            });
            Clients.Remove(handler);
            MapClients -= 1;
            if (Clients.Count < XYZChart.SelClient + 1)
            {
                XYZChart.SelClient = Clients.Count - 1;
            }
            else if (Clients.Count == 0)
            {
                XYZChart.IsDisconnect = true;
                dicFlightData.Clear();
            }
            NewCurrentMoveClients = ((IPEndPoint)handler.RemoteEndPoint).Port;
            MainWindow.DelegateDelAircraftObj.Invoke(NewCurrentMoveClients, DBid);
            ConSoPt.ConsoPrint("#" + ((IPEndPoint)handler.RemoteEndPoint).Port.ToString() + " Disconnect");
        }
        public static void BigBytes(MemoryStream mem, int bytesRead, Socket handler)
        {
            int ClientDataCheck = Clients.IndexOf(Clients.FirstOrDefault(X => X.RemoteEndPoint == handler.RemoteEndPoint));
            //CurrentMoveClients = ClientDataCheck;
            NewCurrentMoveClients = ((IPEndPoint)handler.RemoteEndPoint).Port;
            if (FlightList != null && ClientDataCheck != -1)
            {
                CurrentFlightID = FlightList.IndexOf(FlightList.FirstOrDefault(X => int.Parse(X.linkid) == ((IPEndPoint)handler.RemoteEndPoint).Port)) + 1;
            }
            //Console.WriteLine(" ThreadID " + Thread.CurrentThread.ManagedThreadId);
            if (bytesRead % 71 == 0 && bytesRead > 71)
            {
                byte[] buf = new byte[bytesRead];
                byte[] buf2 = new byte[71];
                buf = mem.ToArray();
                //Console.WriteLine("URA " + mem.Length + " URC " + buf.Length);
                //int ClientDataCheck = Clients.IndexOf(Clients.FirstOrDefault(X => X.RemoteEndPoint == handler.RemoteEndPoint));
                //CurrentMoveClients = ClientDataCheck;
                try
                {
                    for (int i = 0; bytesRead / 71 > i; i++)
                    {
                        int x = i * 71;
                        //int ClientDataCheck = 
                        //Clients.IndexOf(Clients.FirstOrDefault(X => X.RemoteEndPoint == handler.RemoteEndPoint));
                        //CurrentMoveClients = ClientDataCheck;
                        OtherCount += 1;
                        if (buf.Length != 0)
                        {
                            Buffer.BlockCopy(buf, x, buf2, 0, 71);
                            ////Buffer.BlockCopy(BitConverter.GetBytes(ClientDataCheck), 0, buf2, 71, 4);

                            if (FlightList != null && ClientDataCheck != -1)
                            {
                                FlightDataConvert(handler, buf2);
                            }
                            //if (ClientDataCheck == XYZChart.SelClient) WaveDataConvert(ClientDataCheck, buf2);
                            //Console.WriteLine("URA " + mem.Length);
                        }
                    }
                    //Console.WriteLine("ThreadID " + Thread.CurrentThread.ManagedThreadId);

                    //Console.WriteLine("URB " + mem.Length + " URD " + buf.Length);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Length " + memStream.Length);
                    Console.WriteLine(ex);
                    LossCount += 1;
                }
            }
            //return Task.CompletedTask;
        }
        public static void RecvCountS(object sender, EventArgs e)
        {
            if (RecvCount > 0 || LossCount > 0 || OtherCount > 0)
            {
                Console.WriteLine("OK:{0} Loss:{1} Other:{2}", RecvCount, LossCount, OtherCount);
            }
            RecvCount = 0;
            LossCount = 0;
            OtherCount = 0;
        }

        public static void SendLog(string Log)
        {
            if (Directory.Exists(SysDir + @"\Log") == false)//不存在就建立
            {
                Directory.CreateDirectory(SysDir + @"\Log");
            }
            using (FileStream fs = new FileStream(LogPath + SysOpen + ".txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter StreamWriter = new StreamWriter(fs, Encoding.UTF8))
                {
                    StreamWriter.Write(Log);
                    StreamWriter.Close();
                    StreamWriter.Dispose();
                }
                fs.Close();
                fs.Dispose();
            }

        }
        #endregion
    }
}
