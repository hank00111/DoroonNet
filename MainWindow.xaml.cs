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
using System.Collections.ObjectModel;

namespace DoroonNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private System.Timers.Timer MemoryClearTimer, FPSFunc;
        private DispatcherTimer DroneLoc_loop,IMGControl;
        ConsolePrint ConSoPt = new ConsolePrint();     
        int LossCount = 0;
        int te = 0;
        //private DispatcherTimer _renderTimer;

        public MainWindow()
        {
            InitializeComponent();
            ConSoPt.ConsoPrint("Start..."); 
            //fun_MemoryClear();
            ImageFCreate();
            ThreadPool.QueueUserWorkItem(StarUcp);//starUcpserver
            ThreadPool.QueueUserWorkItem(StarTcp);


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

        }

        public void StarUcp(Object stateInfo)
        {
            UDPServer Udplink = new UDPServer();
            Udplink.UDPStart();
            //UdpServer.StartUdp();
        }

        public void StarTcp(Object stateInfo)
        {
            TcpServer Tcplink = new TcpServer();
            Tcplink.AsyncSocketListeners();
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
                //File.Delete(imgPath);
            }
            catch (Exception ex)
            {
                LossCount += 1;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Loss"+ LossCount+" "+ ex);
                Console.ResetColor();
            }
            //File.Delete(imgPath);

            return Task.CompletedTask;
        }

        #region Map
        
        GMapMarker CurrentElement;
        MarkerControl mkControl = new MarkerControl();
        VariableRes VAR = new VariableRes();
        private bool isMouseDown;
        private int baseMarkerCount = 2;
        private int i = 0;
        private List<PointLatLng> MoveHistory = new List<PointLatLng>();
        private List<PointLatLng> pointLatlngs = new List<PointLatLng>();

        private void MainMap_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose provider here
            //MainMap.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            MainMap.MapProvider = GMapProviders.GoogleSatelliteMap;
            MainMap.Position = new PointLatLng(25.083007, 121.456940);
            MainMap.MaxZoom = 25;
            MainMap.MinZoom = 5;
            MainMap.Zoom = 18;
            // lets the map use the mousewheel to zoom
            MainMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            MainMap.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            MainMap.DragButton = MouseButton.Right;

            GroundMarker();
            DroneLocation();
            //ThreadPool.QueueUserWorkItem(DroneLocation_loop);
            VariableRes.Lat = 25.083007;
            VariableRes.Lng = 121.456940;
            //if (i == 0) ThreadPool.QueueUserWorkItem(DroneLocation_loop);
            if (i == 0)
            {
                DroneLoc_loop = new DispatcherTimer();
                DroneLoc_loop.Interval = TimeSpan.FromMilliseconds(200);
                DroneLoc_loop.Tick += DroneLocation_loop;
                DroneLoc_loop.Start();
            }
            i++;
        }

        private void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
            int X = Convert.ToInt32(e.GetPosition(MainMap).X);
            int Y = Convert.ToInt32(e.GetPosition(MainMap).Y);
            PointLatLng point = MainMap.FromLocalToLatLng(X, Y);
            LatLngDisplay(point.Lat, point.Lng);
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
            if (isMouseDown && (Keyboard.Modifiers == ModifierKeys.Control))
            {

                System.Windows.Point clickPoint = e.GetPosition(MainMap);
                PointLatLng point = MainMap.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);
                AddMaker(point);
                //BottomMsg(point.ToString());
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {    
            if (e.Key == Key.Delete && (Keyboard.Modifiers == ModifierKeys.Control))
            {
                mkControl.Popup.IsOpen = false;
                var clear = MainMap.Markers.Where(p => p != null && p != CurrentElement);//&& p != CurrentElement

                if (clear != null)
                {
                    for (baseMarkerCount = 2; baseMarkerCount < clear.Count(); baseMarkerCount++)
                    {
                        MainMap.Markers.Remove(clear.ElementAt(baseMarkerCount));
                        baseMarkerCount--;
                    }
                }
                //this.MainMap.Markers.Clear();
                //pointLatlngs.Clear();
                //_currentElement.Tag = flyId;

                ConSoPt.ConsoPrint("Clear Marker " + clear.Count());
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
            this.MainMap.Markers.Add(marker);
        }

        private void DroneLocation()
        {
            GMapMarker marker = new GMapMarker(new PointLatLng(25.083057, 121.456940));
            marker.Shape = new M_DroneMarker(this, marker);
            marker.ZIndex = 55;
            MainMap.Markers.Add(marker);
        }

        private void DroneLocation_Move(double loc_x, double loc_y, int hdg)
        {
            M_DroneMarker_Func p = new M_DroneMarker_Func();
            var revolve = MainMap.Markers.ElementAt(1);
            revolve.Position = new PointLatLng(loc_x, loc_y);
            p.M_DroneMarker_Angle(hdg);
            DroneControl.Label.Content = $"HDG {hdg} LAT {Math.Round(loc_x, 5)} LNG {Math.Round(loc_y, 5)}";
            //Lab_lat.Content = ($"緯度:{Math.Round(loc_x, 6)}");
            //Lab_lng.Content = ($"經度:{Math.Round(loc_y, 6)}");
            //Lab_hdg.Content = ($"航向:{hdg}");
            //ConSoPt.ConsoPrint($"緯度:{Math.Round(loc_x, 6)}");
            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    var revolve = MainMap.Markers.ElementAt(1);
            //    revolve.Position = new PointLatLng(loc_x, loc_y);
            //    p.M_DroneMarker_Angle(hdg);
            //    DroneControl.Label.Content = $"HDG {hdg} Lat {Math.Round(loc_x, 6)} Lng {Math.Round(loc_y, 6)}";
            //    //Lab_lat.Content = ($"緯度:{Math.Round(loc_x, 6)}");
            //    //Lab_lng.Content = ($"經度:{Math.Round(loc_y, 6)}");
            //    //Lab_hdg.Content = ($"航向:{hdg}");
            //    //ConSoPt.ConsoPrint($"緯度:{Math.Round(loc_x, 6)}");

            //}));
        }

        static FlightData FData = new FlightData();
        private void DroneLocation_loop(object sender, EventArgs e)
        {
            PointLatLng point = new PointLatLng();
            double lat_naxt = 0;
            //VariableRes.Lat += 0.0001;  
            if (FData.FlightLAT != lat_naxt && FData.FlightLNG != 0)
            {
                //point.Lat = FData.FlightLAT;//VariableRes.Lat
                //point.Lng = FData.FlightLNG;
                point.Lat = VariableRes.Lat;//VariableRes.Lat
                point.Lng = VariableRes.Lng;
                MoveHistory.Add(point);
                DroneLocation_Move(VariableRes.Lat, VariableRes.Lng, FData.FlightHDG);
                if (MoveHistory.Count > 0)
                {
                    MoveRoute();
                }
            }
        }
        private void AddMaker(PointLatLng pt)
        {
            GMapMarker marker = new GMapMarker(pt);
            marker.Shape = new CustomMarker(this, marker, pt.ToString());
            marker.ZIndex = 55;
            pointLatlngs.Add(pt);
            this.MainMap.Markers.Add(marker);
        }

        private void MoveRoute()//PointLatLng Star, PointLatLng End
        {
            GMapRoute gmRoute = new GMapRoute(MoveHistory);//       new List<PointLatLng>() { MoveHistory[1] } 
            gmRoute.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
            //gmRoute.Shape. = System.Windows.Media.Brushes.LightSteelBlue;
            //gmRoute.ZIndex = -1;
            if (MainMap.Markers.Count > 3)
            {
                MainMap.Markers.RemoveAt(3);
            }
            MainMap.Markers.Add(gmRoute);
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
                ConSoPt.ConsoPrint("Create");
                Directory.CreateDirectory(Func_exe_path() + @"\ImageCache\");
            }
        }

        #endregion

        #region Chart
        //private DispatcherTimer _updateDataTimer;
        //private DispatcherTimer _renderTimer;

        //int nextDataIndex = 1;

        //double[] data = new double[1000];
        //double[] data2 = new double[1000];
        //double[] data3 = new double[1000];

        //SignalPlot signalPlot;
        //SignalPlot signalPlot2;
        //SignalPlot signalPlot3;
        //Random rand = new Random(0);       

        //private void StyleGraphs()
        //{
        //    Random rd = new Random();

        //    signalPlot = WpfPlot1.Plot.AddSignal(data, label:"x");        
        //    signalPlot.MarkerSize = 0;
        //    signalPlot.LineWidth = 1.5;

        //    signalPlot2 = WpfPlot1.Plot.AddSignal(data2, label:"y");            
        //    signalPlot2.MarkerSize = 0;
        //    signalPlot2.LineWidth = 1.5;

        //    signalPlot3 = WpfPlot1.Plot.AddSignal(data3, label: "z");
        //    signalPlot3.MarkerSize = 0;
        //    signalPlot3.LineWidth = 1.5;

        //    WpfPlot1.Plot.Palette = ScottPlot.Drawing.Palette.Category10;
        //    WpfPlot1.Plot.Title("Unknow");
        //    //WpfPlot1.Plot.XLabel("Time (seconds)");
        //    //WpfPlot1.Plot.XLabel("Sample Number");
        //    WpfPlot1.Plot.SetAxisLimits(xMin: 0, xMax: 60, yMin: -540, yMax: 540);
        //    WpfPlot1.Configuration.LockVerticalAxis = true;
        //    WpfPlot1.Configuration.MiddleClickAutoAxisMarginX = 0;
        //    WpfPlot1.Plot.Legend(location: ScottPlot.Alignment.UpperRight);
        //    WpfPlot1.Render();
            

        //    _updateDataTimer = new DispatcherTimer();
        //    _updateDataTimer.Interval = TimeSpan.FromMilliseconds(100);
        //    _updateDataTimer.Tick += UpdateData;
        //    _updateDataTimer.Start();

        //    // create a timer to update the GUI
        //    _renderTimer = new DispatcherTimer();
        //    _renderTimer.Interval = TimeSpan.FromMilliseconds(20);
        //    _renderTimer.Tick += Render;
        //    _renderTimer.Start();

        //    Closed += (sender, args) =>
        //    {
        //        _updateDataTimer?.Stop();
        //        _renderTimer?.Stop();
        //    };

        //}

        //int x ;
        //bool plus = true;

        //private void UpdateData(object sender, EventArgs e)
        //{
        //    if (nextDataIndex >= data.Length)
        //    {
        //        nextDataIndex = 0;
     
        //        //   1. clear the plot
        //        //   2. create a new larger array
        //        //   3. copy the old data into the start of the larger array
        //        //   4. plot the new (larger) array
        //        //   5. continue to update the new array
        //        //Array.Copy(data, 0, data, 0, data.Length - 1);
        //        //Array.Copy(data, 0, data, 0, data.Length - 1);
        //        //Array.Copy(data2, 0, data2, 0, data2.Length - 1);

        //    }

        //    if (x > 89) plus = false;
        //    if (x < -89) plus = true;

        //    if (plus)
        //    {
        //        x++;
        //    }
        //    else
        //    {
        //        x--;
        //    }



        //    double randomValue = Math.Sin(x) * 100;
        //    double randomValue2 = Math.Cos(x*4) * 180;
        //    double randomValue3 = Math.Sin(x) * 50;//Math.Round((double)rand.Next(0, 360), 1)
        //    data[nextDataIndex] = randomValue;
        //    data2[nextDataIndex] = randomValue2;
        //    data3[nextDataIndex] = randomValue3;

        //    signalPlot.MaxRenderIndex = nextDataIndex;
        //    signalPlot2.MaxRenderIndex = nextDataIndex;
        //    signalPlot3.MaxRenderIndex = nextDataIndex;

        //    nextDataIndex += 1;
         

        //}

        //private void Render(object sender, EventArgs e)
        //{

        //    if (nextDataIndex >= 60)
        //    {
        //        //x1++;
        //        WpfPlot1.Plot.SetAxisLimits(xMin: nextDataIndex - 60, xMax: nextDataIndex, yMin: -220, yMax: 380);
        //    }
        //    else
        //    {
        //        WpfPlot1.Plot.SetAxisLimits(xMin: 0, xMax: 60, yMin: -220, yMax: 380);
        //    }


        //    WpfPlot1.Refresh();
                    
        //}
        #endregion

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

        //InfoViewModel ins = new InfoViewModel();
        XYZChart Ch = new XYZChart();

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    //Dispatcher.BeginInvoke(new Action(() =>
        //    //{
        //    //    XYZChart Ch = new XYZChart();
        //    //    Ch.ChartGraphs_a();
        //    //}));
        //    //XYZChart Ch = new XYZChart();
        //    //XYZChart.OnChart();
        //    ConSoPt.ConsoPrint("Button_Click ");

        //}
    }
}
