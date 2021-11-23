using DoroonNet.ViewModel;
using DoroonNet.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DoroonNet.Command
{
    public class ConnectObject  
    {
        public const int BufferSize = 102400;
        public byte[] buffer = new byte[BufferSize];
        public Socket workSocket = null;
    }

    public class TcpServer
    {
        
        static ConsolePrint ConSoPt = new ConsolePrint();
        static MemoryStream memStream = new MemoryStream();
        static ManualResetEvent allDone = new ManualResetEvent(false);
        public static List<Socket> Clients = new List<Socket>();
        static InfoViewModel ins = new InfoViewModel();
        static FlightInfoRightCommand firc;
        static List<int> Item = new List<int>();
        static int NextClint = 1;


        public void AsyncSocketListeners()
        {
            StartListening(); //啟動 StartListening
        }

        public static void StartListening()
        {
            int ip = 0;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            for (int index = 0; index < ipHostInfo.AddressList.Length; index++)
            {
                if (ipHostInfo.AddressList[index].ToString().Contains("192.168"))
                {
                    ip = index;
                    //Console.WriteLine(index);
                }
                //Console.WriteLine(ipHostInfo.AddressList[index]);
            }
            #region Set IP/Port
            IPAddress ipAddress = ipHostInfo.AddressList[ip];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 7414);
            Socket listener = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
            ConSoPt.ConsoPrint("TCP Using " + ipAddress.ToString() + " " + localEndPoint);
            #endregion
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                listener.IOControl(IOControlCode.KeepAliveValues, KeepAlive(1, 1000, 1000), null);
                while (true)
                {
                    allDone.Reset(); // Start an asynchronous socket to listen for connections.  
                    ConSoPt.ConsoPrint("[Debug] " + "Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback),listener);                    
                    allDone.WaitOne();// Wait until a connection is made before continuing.  
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
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

                    if (BitConverter.ToString(memStream.ToArray()).Replace("-", string.Empty).Contains("30"))
                    {
                        var ClientDataCheck = Clients.IndexOf(Clients.Where(X => X.RemoteEndPoint == handler.RemoteEndPoint).FirstOrDefault());
                        if(ClientDataCheck == XYZChart.SelClient) WaveDataConvert(memStream.ToArray());
                        FlightDataConvert(ClientDataCheck, memStream.ToArray());
                        memStream.SetLength(0);
                    }
                    //Console.WriteLine(((IPEndPoint)handler.RemoteEndPoint).Port.ToString());
                    memStream.Flush();
                }
                else
                {
                    XYZChart.IsDisconnect = true;
                    var DisClient = Clients.IndexOf(Clients.Where(X => X.RemoteEndPoint == handler.RemoteEndPoint).FirstOrDefault());
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        ins.CollectionListPartial.RemoveAt(DisClient);
                    });
                    
                    FlightInfoRightCommand FIRC = new FlightInfoRightCommand() { DataID = "" };                    
                    Clients.Remove(handler);
                    ConSoPt.ConsoPrint("#" + ((IPEndPoint)handler.RemoteEndPoint).Port.ToString() + " Disconnect");
                }

                if (bytesRead > 1)
                {
                    // All the data has been read from the
                    // client. Display it on the console.  
                    //Console.WriteLine("Read {0} bytes from socket.",
                    //    bytesRead);
                    // Echo the data back to the client.  
                    //Send(handler, content);
                    //Send(handler, "");
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
                XYZChart.IsDisconnect = true;
                Console.WriteLine(e.ToString()+" "+ ((IPEndPoint)handler.RemoteEndPoint).Port.ToString());
            }


        }

        private static byte[] KeepAlive(int onOff, int keepAliveTime, int keepAliveInterval)
        {
            byte[] buffer = new byte[12];
            BitConverter.GetBytes(onOff).CopyTo(buffer, 0);
            BitConverter.GetBytes(keepAliveTime).CopyTo(buffer, 4);
            BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);
            return buffer;
        }

        #region Send
        public static void SendC(int Client)
        {
            byte[] byteData = Encoding.ASCII.GetBytes("A23");
            try
            {
                if (Clients[Client] != null)
                {
                    var ClientHandelr = Clients[Client];
                    ClientHandelr.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), ClientHandelr);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {                
                Socket handler = (Socket)ar.AsyncState;// Retrieve the socket from the state object.                 
                int bytesSent = handler.EndSend(ar); // Complete sending the data to the remote device.  
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
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

        private static void WaveDataConvert(byte[] WaveByte)
        {
            XYZChart.IsDisconnect = false;
            XYZChart.X = BitConverter.ToSingle(WaveByte.Skip(1).Take(4).ToArray(), 0);
            XYZChart.Y = BitConverter.ToSingle(WaveByte.Skip(5).Take(4).ToArray(), 0);
            XYZChart.Z = BitConverter.ToSingle(WaveByte.Skip(9).Take(4).ToArray(), 0);
            XYZChart.X1 = BitConverter.ToSingle(WaveByte.Skip(13).Take(4).ToArray(), 0);
            XYZChart.Y1 = BitConverter.ToSingle(WaveByte.Skip(17).Take(4).ToArray(), 0);
            XYZChart.Z1 = BitConverter.ToSingle(WaveByte.Skip(21).Take(4).ToArray(), 0);
            XYZChart.X2 = BitConverter.ToSingle(WaveByte.Skip(25).Take(4).ToArray(), 0);
            XYZChart.Y2 = BitConverter.ToSingle(WaveByte.Skip(29).Take(4).ToArray(), 0);
            XYZChart.Z2 = BitConverter.ToSingle(WaveByte.Skip(33).Take(4).ToArray(), 0);
            WaveByte = null;
        }

        public static void FlightDataConvert(int selClient ,byte[] LatLngByte)
        {           
            XYZChart.IsDisconnect = false;
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                //XYZChart.X = BitConverter.ToSingle(LatLngByte.Skip(1).Take(4).ToArray(), 0);
                //XYZChart.Y = BitConverter.ToSingle(LatLngByte.Skip(5).Take(4).ToArray(), 0);
                //XYZChart.Z = BitConverter.ToSingle(LatLngByte.Skip(9).Take(4).ToArray(), 0);
                //XYZChart.X1 = BitConverter.ToSingle(LatLngByte.Skip(13).Take(4).ToArray(), 0);
                //XYZChart.Y1 = BitConverter.ToSingle(LatLngByte.Skip(17).Take(4).ToArray(), 0);
                //XYZChart.Z1 = BitConverter.ToSingle(LatLngByte.Skip(21).Take(4).ToArray(), 0);
                //XYZChart.X2 = BitConverter.ToSingle(LatLngByte.Skip(25).Take(4).ToArray(), 0);
                //XYZChart.Y2 = BitConverter.ToSingle(LatLngByte.Skip(29).Take(4).ToArray(), 0);
                //XYZChart.Z2 = BitConverter.ToSingle(LatLngByte.Skip(33).Take(4).ToArray(), 0);

                ins.CollectionListPartial[selClient].FlightHDG = BitConverter.ToInt32(LatLngByte.Skip(37).Take(4).ToArray(), 0);
                ins.CollectionListPartial[selClient].FlightSPD = MathF.Round(BitConverter.ToSingle(LatLngByte.Skip(41).Take(4).ToArray(), 0), 2);
                ins.CollectionListPartial[selClient].FlightALT = MathF.Round(BitConverter.ToSingle(LatLngByte.Skip(45).Take(4).ToArray(), 0), 2);
                ins.CollectionListPartial[selClient].FlightLAT = Math.Round(BitConverter.ToDouble(LatLngByte.Skip(49).Take(8).ToArray(), 0), 8);
                ins.CollectionListPartial[selClient].FlightLNG = Math.Round(BitConverter.ToDouble(LatLngByte.Skip(57).Take(8).ToArray(), 0), 8);

                //ins.CollectionListPartial[0] = new FlightData
                //{
                //    FlightHDG = BitConverter.ToInt32(LatLngByte.Skip(37).Take(4).ToArray(), 0),
                //    FlightSPD = MathF.Round(BitConverter.ToSingle(LatLngByte.Skip(41).Take(4).ToArray(), 0), 2),
                //    FlightALT = MathF.Round(BitConverter.ToSingle(LatLngByte.Skip(45).Take(4).ToArray(), 0), 2),
                //    FlightLAT = Math.Round(BitConverter.ToDouble(LatLngByte.Skip(49).Take(8).ToArray(), 0), 8),
                //    FlightLNG = Math.Round(BitConverter.ToDouble(LatLngByte.Skip(57).Take(8).ToArray(), 0), 8)
                //};
                
                LatLngByte = null;
            });

        }
              
        public static void ClientComp(Socket handle)
        {

            Clients.Add(handle);
            if (Clients.Count == ins.CollectionListPartial.Count)
            {
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    ins.CollectionListPartial[Clients.Count - 1].ID = Clients.Count;
                    ins.CollectionListPartial[Clients.Count - 1].FlightID = "#" + ((IPEndPoint)handle.RemoteEndPoint).Port.ToString();               
                });
                Console.WriteLine("A=b");
            }
            else if (Clients.Count > ins.CollectionListPartial.Count)
            {
                NextClint++;
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    ins.CollectionListPartial.Add(new FlightData() { ID = Clients.Count, FlightID = "#" + ((IPEndPoint)handle.RemoteEndPoint).Port.ToString() });
                });

            }
            //Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            //{
            //    ins.CollectionListPartial.Add(new FlightData() { ID = 5, FlightID = "#" + ((IPEndPoint)handle.RemoteEndPoint).Port.ToString() });
            //});
            //Clients.Add(handle);
            firc = new FlightInfoRightCommand();
            firc.DataID = ((IPEndPoint)handle.RemoteEndPoint).Port.ToString();
            //FlightInfoRightCommand FIRC = new FlightInfoRightCommand() 
            //{ DataID = ((IPEndPoint)handle.RemoteEndPoint).Port.ToString() };
            //Console.WriteLine(FIRC.DataID);         
            ConSoPt.ConsoPrint("#" + ((IPEndPoint)handle.RemoteEndPoint).Port.ToString() + " Connected "
                + ins.CollectionListPartial.Count.ToString() + " " + Clients.Count.ToString());
        }

        public static Task FlightData(byte[] FData)
        {
            XYZChart.IsDisconnect = false;
            XYZChart.X = BitConverter.ToSingle(FData.Skip(1).Take(4).ToArray(), 0);
            XYZChart.Y = BitConverter.ToSingle(FData.Skip(5).Take(4).ToArray(), 0);
            XYZChart.Z = BitConverter.ToSingle(FData.Skip(9).Take(4).ToArray(), 0);            
            //XYZChart.nextDataIndex += 1;            
            //ConSoPt.ConsoPrint(XYZChart.nextDataIndex.ToString() + XYZChart.X.ToString());
            return Task.CompletedTask;
        }

        

    }
}
