using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
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
using System.Timers;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using ScottPlot;
using ScottPlot.Plottable;
using System.Diagnostics;
using DoroonNet.Views;
using DoroonNet.Command;
using DoroonNet.Resources;
using DoroonNet.ViewModel;
using DoroonNet.DoroonDB;
using DoroonNet.RouteView;
using DoroonNet.NavView;

namespace DoroonNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private System.Timers.Timer MemoryClearTimer, FPSFunc, RecvFunc;
        private DispatcherTimer DroneLoc_loop,IMGControl;
        ConsolePrint ConSoPt = new ConsolePrint();
        DoroonSQLLiteDB DoroonDB;
        static CancellationTokenSource cts;
        int LossCount = 0;
        int te = 0;
        Nav nav;

        //private DispatcherTimer _renderTimer;
        public MainWindow()
        {

            InitializeComponent();
            ConSoPt.ConsoPrint("Start...");
            DoroonDB = new DoroonSQLLiteDB();
            DoroonDB.CreateTables();
            //fun_MemoryClear();
            ImageFCreate();         
            //starUcpserver
            //ThreadPool.QueueUserWorkItem(StarTcp, cts.Token);
       
            MemoryClearTimer = new System.Timers.Timer();            
            MemoryClearTimer.Elapsed += fun_MemoryClear;
            MemoryClearTimer.Interval = 60*1000;
            MemoryClearTimer.Start();           
            
            IMGControl = new DispatcherTimer();
            IMGControl.Interval = TimeSpan.FromMilliseconds(32);
            IMGControl.Tick += IMgControl;
            IMGControl.Start();

            FPSFunc = new System.Timers.Timer();
            FPSFunc.Elapsed += ImageConverterV2.FPS_Func;
            FPSFunc.Interval = 1000;
            FPSFunc.Start();

            //Imgavdio.Source = new BitmapImage(new Uri(@"G:\相片\新增資料夾\lna4K.PNG"));

            //VariableRes tt = new VariableRes();
            //tt.ImageID = "1234";

            ThreadPool.QueueUserWorkItem(StarUcp);
            ThreadPool.QueueUserWorkItem(StarTcp);

            RecvFunc = new System.Timers.Timer();
            RecvFunc.Elapsed += TcpServer.RecvCountS;
            RecvFunc.Interval = 1000;
            RecvFunc.Start();
        }

        public void StarUcp(Object stateInfo)
        {
            UDPServer Udplink = new UDPServer();
            Udplink.UDPStart();
        }

        public void StarTcp(Object stateInfo)
        {
            TcpServer Tcplink = new TcpServer();
            Tcplink.AsyncSocketListeners();
        } 

        public static void CloseT()
        {
            cts.Cancel();
            Console.WriteLine("Cancellation set in token source...");
            // Cancellation should have happened, so call Dispose.
            cts.Dispose();
        }

        private void IMgControl(object sender, EventArgs e)
        {
            //VideoChunk.Content = ($"Video Chunk:{AsynchronousSocketListener.imgDate.Count} Load:{te} Chunk:{chunk}");
            VariableRes VarR = new VariableRes();
            FPS_text.DataContext = VarR;

            if (ImageConverterV2.imgJpgDate.Count > te)
            {
                try
                {
                    if (te > 1000)
                    {
                        ImageConverterV2.imgJpgDate.RemoveRange(0, 1000);
                        //chunk += 2;
                        //chunksleep += 1;
                        te = 0;
                    }

                    if (ImageConverterV2.imgJpgDate.Count == te)
                    {
                        te -= 1;
                    }
                    else
                    {
                        ImgGenerate(ImageConverterV2.ImgPath + ImageConverterV2.imgJpgDate[te]);
                        //Console.ForegroundColor = ConsoleColor.Magenta;
                        //Console.WriteLine(ImageConverterV2.imgJpgDate.Count + " " + ImageConverterV2.ImgPath + ImageConverterV2.imgJpgDate[te]);
                        //Console.ResetColor();
                        te += 1;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ImageConverterV2.imgJpgDate.Count + " OUT " + te + " " + ex);
                    //Console.WriteLine(e.Message + "!!");
                    //te -= 1;
                }

            }

        }

        #region old IMGControl
        private void ImgControl(Object stateInfo)
        {
            int te = 0;
            //int Refresh = 1000;
            //int chunk = 40;
            //int chunksleep = 35;

            while (true)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    //VideoChunk.Content = ($"Video Chunk:{AsynchronousSocketListener.imgDate.Count} Load:{te} Chunk:{chunk}");
                    if (ImageConverterV2.imgJpgDate.Count > te)
                    {

                        try
                        {
                            if (te > 1000)
                            {
                                ImageConverterV2.imgJpgDate.RemoveRange(0, 1000);
                                //chunk += 2;
                                //chunksleep += 1;
                                te = 0;
                            }

                            if (ImageConverterV2.imgJpgDate.Count == te)
                            {
                                te -= 1;
                            }
                            else
                            {
                                ImgGenerate(ImageConverterV2.ImgPath + ImageConverterV2.imgJpgDate[te]);
                                //Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine(ImageConverterV2.imgJpgDate.Count +" "+ ImageConverterV2.ImgPath +ImageConverterV2.imgJpgDate[te]);
                                //Console.ResetColor();
                                te += 1;
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(ImageConverterV2.imgJpgDate.Count + " OUT " + te +" "+ e);
                            //Console.WriteLine(e.Message + "!!");
                            //te -= 1;
                        }
                    }
                    //if (AsynchronousSocketListener.MessageShow != null)
                    //{
                    //    listBox1.Items.Add(DateTime.Now.ToString("tt HH:mm:ss") + " [" + listBox1.Items.Count + "] " + AsynchronousSocketListener.MessageShow);
                    //    UpdateScrollBar(listBox1);
                    //    AsynchronousSocketListener.MessageShow = null;
                    //}

                }));
                Thread.Sleep(1000);
            }

        }
        #endregion

        private Task ImgGenerate(string imgPath)
        {
            try
            {
                //Imgavdio.Source = null;
                //Console.WriteLine(imgPath + " Check");
                //Imgavdio.Source = new BitmapImage(new Uri(imgPath));
                using (var stream = File.OpenRead(imgPath))
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.StreamSource = stream;
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.EndInit();
                    WriteableBitmap writeableBitmap = new WriteableBitmap(bmp);
                    Imgavdio.Source = writeableBitmap;
                }
                File.Delete(imgPath);
                File.Delete(imgPath.Replace(".jpg", ".txt"));
            }
            catch 
            {
                LossCount += 1;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Loss"+ LossCount+" "+ imgPath + " ");
                Console.ResetColor();
                File.Delete(imgPath);
                File.Delete(imgPath.Replace(".jpg", ".txt"));
            }
            //File.Delete(imgPath);

            return Task.CompletedTask;
        }

        #region Map
        
        GMapMarker CurrentElement;
        //MarkerControl mkControl = new MarkerControl();
        //VariableRes VAR = new VariableRes();
        public delegate void DelegateAddAircraft(int AircraftID, int ID, int DBID);
        public static DelegateAddAircraft DelegateAddAircraftObj;
        public delegate void DelegateDelAircraft(int AircraftID, int DBID);
        public static DelegateDelAircraft DelegateDelAircraftObj;
        public delegate void DelegateRouteChange(int MarkerID, int DBID);
        public static DelegateRouteChange DelegateRouteChangeObj;
        public delegate void DelegateRouteMove();
        public static DelegateRouteMove DelegateRouteMoveObj;

        private bool mapRender = false;
        private bool isMouseDown;
        // private bool MovePositionBool = false;
        private int MovePathPosition = 0;
        private int baseMarkerCount = 2;
        private int i = 0;
        private int DroneMarkerCount = 0;
        private int Cmr = 0; //ChangeMoveRoute

        private List<PointLatLng> MoveHistory = new List<PointLatLng>();
        private List<PointLatLng> NavList = new List<PointLatLng>();

        private void MainMap_Loaded(object sender, RoutedEventArgs e)
        {

            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose provider here
            //MainMap.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            MainMap.MapProvider = GMapProviders.BingSatelliteMap;
            MainMap.Position = new PointLatLng(25.083007, 121.456940);
            MainMap.MaxZoom = 30;
            MainMap.MinZoom = 2;
            MainMap.Zoom = 12;
            // lets the map use the mousewheel to zoom
            MainMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            MainMap.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            MainMap.DragButton = MouseButton.Right;

            GroundMarker();
            //DroneLocation();
            //ThreadPool.QueueUserWorkItem(DroneLocation_loop);
            VariableRes.Lat = 25.083007;
            VariableRes.Lng = 121.456940;
            //if (i == 0) ThreadPool.QueueUserWorkItem(DroneLocation_loop);
            if (i == 0)
            {
                DroneLoc_loop = new DispatcherTimer();
                DroneLoc_loop.Interval = TimeSpan.FromMilliseconds(250);
                DroneLoc_loop.Tick += DroneLocation_loop;
                DroneLoc_loop.Start();
            }
            i++;
            nav = new Nav();
            DelegateModeMapObj = ColorChg;
            DelegateDelAircraftObj = AircraftDel;
            DelegateAddAircraftObj = AircraftAdd;
            DelegateRouteChangeObj = MoveRouteChange;
            DelegateRouteMoveObj = MultiRoute;
        }

        private void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
            int X = Convert.ToInt32(e.GetPosition(MainMap).X);
            int Y = Convert.ToInt32(e.GetPosition(MainMap).Y);
            PointLatLng point = MainMap.FromLocalToLatLng(X, Y);
            //LatLngDisplay(point.Lat, point.Lng);
        }
        
        private void MainMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            isMouseDown = true;
            //if (_currentElement == null)
            //{
            //    System.Windows.Point pt = e.GetPosition(MainMap);
            //    PointLatLng point = MainMap.FromLocalToLatLng((int)pt.X, (int)pt.Y);

            //    PointHitTestParameters parameters = new PointHitTestParameters(pt);
            //    VisualTreeHelper.HitTest(MainMap, null, HitTestCallback, parameters);
            //    Console.WriteLine(pointLatlngs.Count.ToString());
            //    MarkerMoveflag = true;
            //}
            //CreateDynamicBorder(6,3);            
            if (IsEdit && isMouseDown)//&& (Keyboard.Modifiers == ModifierKeys.Control)
            {
                System.Windows.Point clickPoint = e.GetPosition(MainMap);
                PointLatLng point = MainMap.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);
                //NavData datal = new NavData();

                AddNavMarker(point);
                nav.NavDGRef();
                //Console.WriteLine(point);
                //AddNavMarker(point);
                //BottomMsg(point.ToString());
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {    
            if (e.Key == Key.Delete && (Keyboard.Modifiers == ModifierKeys.Control))
            {
                //mkControl.Popup.IsOpen = false;
                var clear = MainMap.Markers.Where(p => p != null && p != CurrentElement);//&& p != CurrentElement

                if (clear != null)
                {
                    for (baseMarkerCount = 1; baseMarkerCount < clear.Count(); baseMarkerCount++)
                    {
                        //MainMap.Markers.Remove(clear.ElementAt(baseMarkerCount));
                        //baseMarkerCount--;
                    }
                }
                //this.MainMap.Markers.Clear();
                //pointLatlngs.Clear();
                //_currentElement.Tag = flyId;

                //ConSoPt.ConsoPrint("Clear Marker " + clear.Count());
            }
        }

        private void Window_Keyup(object sender, KeyEventArgs e)
        {
         
        }

        private void GroundMarker()
        {
            GMapMarker marker = new GMapMarker(new PointLatLng(25.083007, 121.456940));
            marker.Shape = new GroundMarker(this, marker);
            marker.ZIndex = 100;
            marker.Tag = "Ground";
            this.MainMap.Markers.Add(marker);
        }

        private void AddNavMarker(PointLatLng pt)
        {

            GMapMarker marker = new GMapMarker(pt);
            marker.Shape = new CustomMarker(this, marker, pt.ToString(), NavData.NavList.Count + 1);
            marker.ZIndex = 55;
            marker.Tag = "NAVPT";

            NavData data = new NavData
            {
                ID = NavData.NavList.Count + 1,
                SPD = 1.5f,
                NavPt = pt
            };
            NavData.NavList.Add(data);
            NavList.Clear();
            foreach (var PT in NavData.NavList)
            {
                //Console.WriteLine(data.ALT);                
                NavList.Add(PT.NavPt);
            }         

            GMapMarker NavRt = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Nav");
            if (NavRt != null)
            {
                MainMap.Markers.Remove(NavRt);
            }

            GMapRoute NavRoute = new GMapRoute(NavList);
            NavRoute.Shape = new System.Windows.Shapes.Path() 
            { Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 173, 0, 204)), StrokeThickness = 3 };
            NavRoute.Tag = "Nav";
            MainMap.Markers.Add(NavRoute);
            
            MainMap.Markers.Add(marker);
        }

        public void AircraftAdd(int AircraftID, int ID, int DBID)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GMapMarker marker = new GMapMarker(new PointLatLng(25.089057, 121.456940));
                marker.Shape = new M_DroneMarker(this, marker, AircraftID, ID, DBID);
                marker.ZIndex = 55;
                marker.Tag = AircraftID;
                MainMap.Markers.Add(marker);
                DroneMarkerCount += 1;
                //MovePositionBool = true;
                Console.WriteLine("ADD " + ID);
            }));

        }

        public void AircraftDel(int AircraftID, int DBID)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                DroneMarkerCount -= 1;
                MovePathPosition -= 1;
                GMapMarker DelFlight = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == AircraftID.ToString());
                //Console.WriteLine(DelFlight.Tag + "Clear");
                MainMap.Markers.Remove(DelFlight);           
                MoveRouteChange(0, DBID);

            }));
        }

        private Task DroneLocation_Move(double loc_x, double loc_y, int hdg, int currentMoveClients)
        {
            M_DroneMarker_Func p = new M_DroneMarker_Func();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (currentMoveClients != 0)
                    {
                        mapRender = true;
                        GMapMarker revolve = MainMap.Markers.ElementAt(currentMoveClients);//currentMoveClients
                        //Console.WriteLine(MainMap.Markers.IndexOf(MainMap.Markers.FirstOrDefault(u => u.Tag.ToString() == currentMoveClients)));
                        revolve.Position = new PointLatLng(loc_x, loc_y);
                        //ins.CollectionListPartial[currentMoveClients].FlightLAT ins.CollectionListPartial[currentMoveClients].FlightLNG
                        p.M_DroneMarker_Angle(hdg);
                        //MultiRoute();
                        //MoveRoute();
                        mapRender = false;
                    }
                }
                catch(Exception ex) 
                { Console.WriteLine(ex); }   
 
            }));

            return Task.CompletedTask;
        }

        int DisClient = -1;
        //string RouteTag = "Route" + TcpServer.FirstClients;
        private void MoveRoute()//PointLatLng Star, PointLatLng End
        {            
            //foreach(var ss in MainMap.Markers)
            //{
            //    //Console.WriteLine(ss);
            //    //Console.WriteLine("--");
            //}
            GMapRoute gmRoute = new GMapRoute(MoveHistory);//
            gmRoute.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
            gmRoute.Tag = "Route";
            GMapMarker Route = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Route");

            if (Route != null)
            {
                int x = MainMap.Markers.IndexOf(MainMap.Markers.FirstOrDefault(u => u.Tag.ToString() == "Route"));
                MainMap.Markers.Remove(Route);
                MainMap.Markers.Add(gmRoute);
                //Console.WriteLine(x);
                //Route = new GMapRoute(MoveHistory);
                //Route.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
                //Route.Tag = "Route";
            }
            else
            {
                MainMap.Markers.Add(gmRoute);
            }

            if (DroneMarkerCount > 0)
            {
                int RoutePosition = DroneMarkerCount + 1;
                MovePathPosition = RoutePosition;                
                
                DisClient = MainMap.Markers.IndexOf(MainMap.Markers.FirstOrDefault(X => X.Shape == gmRoute.Shape));
                //Console.WriteLine(DisClient);
            }

        }
        private void MoveRouteChange(int chg, int DBID)
        {
            List<PointLatLng> FlightDataGPS = DoroonSQLLiteDB.DelegateFlightDataGPSObj.Invoke(DBID);
            Cmr = chg;
            GMapMarker ClearRT = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Route");

            if (ClearRT != null)
            {
                MainMap.Markers.Remove(ClearRT);
                MoveHistory.Clear();
                MoveHistory = FlightDataGPS;
            }
        }

        private void MultiRoute()
        {
            int pos = 0;
            foreach (var s in TcpServer.CLP)
            {
                int a = s.ID;
                //GMapMarker ClearRT = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Route");
                List<PointLatLng> FlightDataGPS = DoroonSQLLiteDB.DelegateFlightDataGPSObj.Invoke(a);
                GMapRoute DoroonRoute = new GMapRoute(FlightDataGPS);
                DoroonRoute.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(colorsSW(pos)), StrokeThickness = 2 };
                DoroonRoute.Tag = "Route" + pos;

                GMapMarker RouteMk = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Route" + pos);

                if (RouteMk != null)
                {
                    //int x = MainMap.Markers.IndexOf(MainMap.Markers.FirstOrDefault(u => u.Tag.ToString() == "Route" + pos));
                    MainMap.Markers.Remove(RouteMk);
                    MainMap.Markers.Add(DoroonRoute);
                    //Console.WriteLine(x);
                    //Route = new GMapRoute(MoveHistory);
                    //Route.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
                    //Route.Tag = "Route";
                }
                else
                {
                    MainMap.Markers.Add(DoroonRoute);
                }

                pos++;
                //Console.WriteLine(a);
            }
        }

        private System.Windows.Media.Color colorsSW(int c)
        {
            var co = Colors.Yellow;
            switch (c)
            {
                case 0:
                    co = Colors.Yellow;
                    break;
                case 1:
                    co = Colors.Cyan;
                    break;
                case 2:
                    co = Colors.Blue;
                    break;
            }

            return co;
        }


        private void NavPlanning()
        {
            GMapMarker NavMarkers = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "NavRoute");

            GMapRoute NavRoute = new GMapRoute(RtView.NavPtLatLng);
            NavRoute.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Orange), StrokeThickness = 1.8 };
            NavRoute.ZIndex = -1;
            NavRoute.Tag = "NavRoute";
            while (true)
            {
                GMapMarker FindNavMarkers = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Navmarker");
                if (FindNavMarkers != null)
                {
                    MainMap.Markers.Remove(FindNavMarkers);
                }
                else break;
            }

            for(int i = 1; i< RtView.NavPtLatLng.Count; i++)
            {
                GMapMarker Navmarker = new GMapMarker(RtView.NavPtLatLng[i - 1]);
                Navmarker.Shape = new NavMarker(this, Navmarker, i - 1);
                Navmarker.ZIndex = 0;
                Navmarker.Tag = "Navmarker";
                MainMap.Markers.Add(Navmarker);
            }

            if (NavMarkers != null)
            {
                MainMap.Markers.Remove(NavMarkers);
                MainMap.Markers.Add(NavRoute);
            }
            else
            {
                MainMap.Markers.Add(NavRoute);
            }
            RtView.isClickSend = false;
        }

        static FlightData FData = new FlightData();
        static InfoViewModel ins = new InfoViewModel();
        private void DroneLocation_loop(object sender, EventArgs e)
        {
            PointLatLng point = new PointLatLng();
            double lat_naxt = 0;
            if (RtView.isClickSend)
            {
                NavPlanning();
            }
            GMapMarker NAVPt = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "NAVPT");
            if (NAVPt != null && Nav.ReqDel)
            {
                GMapMarker NavRt = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Nav");
                if (NavRt != null)
                {
                    MainMap.Markers.Remove(NavRt);
                }

                MainMap.Markers.Remove(NAVPt);
                //Console.WriteLine("IN LOOP");
            }
            else
            {
                Nav.ReqDel = false;
            }

            if (TcpServer.MapClients > DroneMarkerCount)//TcpServer.MapClients
            {
                //Console.WriteLine("Marker 1 " + TcpServer.MapClients + " " + DroneMarkerCount + " " + MainMap.Markers.Count());
                //MulitDroneLocationAdd();//Add
            }
            else if (DroneMarkerCount > TcpServer.MapClients || DroneMarkerCount == 0)
            {
                //Console.WriteLine("Marker 2 " + TcpServer.MapClients + " " + DroneMarkerCount + " " + MainMap.Markers.Count());
                if (DroneMarkerCount != 0)
                {

                    //MulitDroneLocationDel(TcpServer.CurrentMapClients);//Del//Need FIX
                }
                else
                {
                    GMapMarker ClearRT = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString().StartsWith("Route"));
                    GMapMarker ClearDrone = MainMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() != "Ground");
                    if (ClearRT != null)
                    {
                        MainMap.Markers.Remove(ClearRT);
                        MoveHistory.Clear();
                    }
                    if (ClearDrone != null)
                    {
                        if (ClearDrone.Tag is not "NavRoute" and not "Navmarker" and not "NAVPT" and not "Nav")
                        {
                            MainMap.Markers.Remove(ClearDrone);
                        }
                        
                    }
                }
            }

            if (FData.FlightLAT != lat_naxt && FData.FlightLNG != 0)
            {
                try
                {
                    //MultiRoute();
                    if (TcpServer.MapClients > 0 && MainMap.Markers.Count > 0 && TcpServer.CurrentMoveClients != -1)
                    {
                        foreach (var s in TcpServer.CLP)
                        {                           
                            //Console.WriteLine(s.FlightID.Replace("#", string.Empty));
                            int a = MainMap.Markers.IndexOf(MainMap.Markers.FirstOrDefault(u => u.Tag.ToString() == s.FlightID.Replace("#", string.Empty)));
                            Task.Run(() => DroneLocation_Move(s.FlightLAT, s.FlightLNG, s.FlightHDG, a));
                            //Console.WriteLine("ID:{0}:{1}{2}{3}",a, s.FlightLAT, s.FlightLNG,s.FlightID);
                        }
                        if (TcpServer.CLP[0].FlightLAT != 0)
                        {
                            point.Lat = TcpServer.CLP[Cmr].FlightLAT;//VariableRes.Lat
                            point.Lng = TcpServer.CLP[Cmr].FlightLNG;
                            MoveHistory.Add(point);
                        }

                        //if (TcpServer.CLP[TcpServer.CurrentMoveClients].FlightLAT != 0 && TcpServer.CLP[TcpServer.CurrentMoveClients].FlightLNG != 0)
                        //{                           
                        //   foreach(var s in TcpServer.CLP)
                        //    {
                        //        //Console.WriteLine(s.FlightID.Replace("#", string.Empty));
                        //        Task.Run(() => DroneLocation_Move(s.FlightLAT, s.FlightLNG, s.FlightHDG, s.FlightID.Replace("#", string.Empty)));
                        //    }
                        //point.Lat = TcpServer.CLP[0].FlightLAT;//VariableRes.Lat
                        //point.Lng = TcpServer.CLP[0].FlightLNG;
                        //    int hdg = TcpServer.CLP[TcpServer.CurrentMoveClients].FlightHDG;
                        //    int Client = TcpServer.NewCurrentMoveClients;
                        //MoveHistory.Add(point);
                        //    //Task.Run(() => DroneLocation_Move(point.Lat, point.Lng, hdg, Client));
                        //    //DroneLocation_Move(point.Lat, point.Lng, TcpServer.CLP[TcpServer.CurrentMoveClients].FlightHDG, TcpServer.NewCurrentMoveClients);
                        //    //Console.WriteLine(MoveHistory.Count);
                        //}
                    }
                    else
                    {
                        MoveHistory.Clear();
                    }
                }
                catch (Exception EX)
                {
                    Console.WriteLine(EX);
                }

            }

        }      

        private void MapLocationLoop()
        {
            PointLatLng point = new PointLatLng();
            try
            {
                if (TcpServer.MapClients > 0 && MainMap.Markers.Count > 0 && TcpServer.CurrentMoveClients != -1)
                {
                    if (TcpServer.CLP[TcpServer.CurrentMoveClients].FlightLAT != 0 && TcpServer.CLP[TcpServer.CurrentMoveClients].FlightLNG != 0)
                    {
                        //point.Lat = TcpServer.CLP[TcpServer.CurrentMoveClients].FlightLAT;//VariableRes.Lat
                        //point.Lng = TcpServer.CLP[TcpServer.CurrentMoveClients].FlightLNG;
                        //int hdg = TcpServer.CLP[TcpServer.CurrentMoveClients].FlightHDG;
                        //int Client = TcpServer.NewCurrentMoveClients;
                        //MoveHistory.Add(point);
                        //Task.Run(() => DroneLocation_Move(point.Lat, point.Lng, hdg, Client));
                    }
                }
                else
                {
                    MoveHistory.Clear();
                }
            }
            catch (Exception EX)
            {
                Console.WriteLine(EX);
            }
        }

        public void LatLngDisplay(double lat, double lng)//, int alt
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //MainMap.Position = new PointLatLng(x, y);
                //txt_lat.Content = lat.ToString();
                //txt_lon.Content = lng.ToString();
                //Console.WriteLine("AA lat " + lat.ToString() + " lng " + lng.ToString());
            }));
        }

        #endregion

        #region Tool
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        public void fun_MemoryClear(object sender, ElapsedEventArgs e)
        {
            //Task tClear = new Task(() =>
            //{
            //    while (true)
            //    {
            //        //Thread.Sleep(15000);
            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //        {
            //            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            //        }
            //        ConSoPt.ConsoPrint("GC Collect");
            //    }
            //});
            //tClear.Start();
            TcpServer.SendKeep();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
            ConSoPt.ConsoPrint("GC Collect");
        }
        public static string Func_exe_path()
        {
            return System.Windows.Forms.Application.StartupPath;
        }
        private void ImageFCreate()
        {
            if (Directory.Exists(Func_exe_path() + @"\ImageCache\") == false)//不存在就建立
            {
                //ConSoPt.ConsoPrint("Create");
                Directory.CreateDirectory(Func_exe_path() + @"\ImageCache\");
            }
        }

        #endregion


        private void DatabasMenu_Click(object sender, RoutedEventArgs e)
        {
            DBHome DBView = new DBHome();
            DBView.Show();
        }

        private void MenuView_Click(object sender, RoutedEventArgs e)
        {
            RtView rtview = new RtView(MainMap.Position);
            rtview.Show();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((TabInfo.SelectedItem as TabItem).Header.ToString() == "快速設定")
            {
                QuickSettings quickSettings = new QuickSettings();
                //  quickSettings.Init();
            }
            //Console.WriteLine((TabInfo.SelectedItem as TabItem).Header);
            //QuickSettings();
        }

        #region ModeSel
        public static bool IsEdit;
        public delegate void DelegateModeMap(bool isedit);
        public static DelegateModeMap DelegateModeMapObj;

        private void ModeMap_Click(object sender, RoutedEventArgs e)
        {
            IsEdit = false;
            ColorChg(IsEdit);

        }

        private void ModeEdit_Click(object sender, RoutedEventArgs e)
        {
            IsEdit = true;
            Nav navView = new Nav();
            navView.Show();
            ColorChg(IsEdit);

        }
        public void ColorChg(bool isEdit)
        {
            if (isEdit)
            {
                ModeMap.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(110, 107, 68, 35));
                ModeEdit.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 107, 68, 35));

                ModeMap.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 189, 189, 189));
                ModeEdit.Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                ModeMap.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 107, 68, 35));
                ModeEdit.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(110, 107, 68, 35));

                ModeMap.Foreground = new SolidColorBrush(Colors.White);
                ModeEdit.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 189, 189, 189));
            }
        }

        private void SystemView_Click(object sender, RoutedEventArgs e)
        {
            SYS navView = new SYS();
            navView.Show();
        }
        #endregion

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ConSoPt.ConsoPrint(e.NewSize.Width.ToString() +" "+e.NewSize.Height.ToString());
            if (e.WidthChanged)
            {
                //ConSoPt.ConsoPrint(e.WidthChanged.ToString());
                var w = ((e.NewSize.Width+2) - 480);
                var h = ((e.NewSize.Height + 30) - 300);

                //Image_grid.Width = w;
                //Image_grid.Height = h;

                if (w % 16 < 3 || h % 9 < 3)
                {
                    //ConSoPt.ConsoPrint("new img width " + w.ToString());
                    //Image_grid.Width = w;
                    //Image_grid.Height = h;
                }
                //Image_border.Width = 640;
                //Image_border.Height = 360;
            }

        }
    }

}
