using DoroonNet.Command;
using DoroonNet.Views;
using Geolocation;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace DoroonNet.RouteView
{
    /// <summary>
    /// Route.xaml 的互動邏輯
    /// </summary>
    /// 
    public partial class RtView : Window
    {
        //private int t = 0;
        private byte[] NavPtData;
        private bool AltSet;
        private bool GoHome;
        private bool ComBoXIsLoad;
        private bool ForwardBtIsLoad = false;
        private bool isForward = true;
        private PointLatLng Pos;
        private RtVModel RtV = new RtVModel();        
        private List<NavData> NavPt = new List<NavData>();
        private DispatcherTimer Ref;

        public static bool isClickSend;
        public static List<PointLatLng> NavPtLatLng;

        public RtView(PointLatLng P)
        {
            InitializeComponent();
            HeightTxT.DataContext = RtV;
            WidthTxT.DataContext = RtV;
            AngleTxT.DataContext = RtV;
            SLTxT.DataContext = RtV;
            AltTxT.DataContext = RtV;
            NavPtDataGrid.ItemsSource = null;
            NavPtDataGrid.ItemsSource = NavPt;
            Pos = P;
            Ref = new DispatcherTimer();
            Ref.Interval = TimeSpan.FromMilliseconds(1);
            Ref.Tick += RefData;
        }

        #region Set
        private void ForwardBt_Checked(object sender, RoutedEventArgs e)
        {
            if (ForwardBtIsLoad)
            {
                ReverseBt.IsChecked = false;
                isForward = true;
            }
        }

        private void ReverseBt_Checked(object sender, RoutedEventArgs e)
        {
            ForwardBt.IsChecked = false;
            isForward = false;
        }

        private void ForwardBt_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ReverseBt.IsChecked == false)
            {
                ForwardBt.IsChecked = true;
            }
        }

        private void ReverseBt_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ForwardBt.IsChecked == false)
            {
                ReverseBt.IsChecked = true;
            }
        }

        private void ForwardBt_Loaded(object sender, RoutedEventArgs e)
        {
            ForwardBtIsLoad = true;
        }
        #endregion

        #region Send

        private void SendMsgBt_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SendMesgBt_Click(object sender, RoutedEventArgs e)
        {
            string LogSave = "";
            NavPtLatLng = new List<PointLatLng>();
            int pos = 0;
            int one = 0;
            int Mode = 0;
            GoHome = true;

            switch (ComBoBoX.SelectedIndex)
            {
                case 0:
                    NavPtData = new byte[(5 * 24) + 4 + 1 + 1];//5 int.Parse(LapTxTBox.Text)              
                    LogSave = $"R,Lap:[{LapTxTBox.Text}],isForward:[{isForward}],H:[{RtV.HeightTx}],W:[{RtV.WidthTx}],Angle:[{RtV.AngleTx}],Alt:[{RtV.AltTx}],Nav:";
                    break;
                case 1:
                    NavPtData = new byte[(4 * 24) + 4 + 1 + 1];//4 int.Parse(LapTxTBox.Text)
                    LogSave = $"T,Lap:[{LapTxTBox.Text}],isForward:[{isForward}],H:[{RtV.SLTx}],W:[0],Angle:[{RtV.AngleTx}],Alt:[{RtV.AltTx}],Nav:";
                    break;
                case 2:
                    NavPtData = new byte[(24) + 12 + 1 + 1];
                    LogSave = $"C,Lap:[{LapTxTBox.Text}],isForward:[{isForward}],H:[{RtV.HeightTx}],W:[{RtV.WidthTx}],Angle:[{RtV.AngleTx}],Alt:[{RtV.AltTx}],Nav:";
                    break;
            }

            if (MessageBox.Show("請於下方確認無人機選擇是否正確。\n共" + NavPt.Count + "個導航點高度是否設定完成。(1/2)", "傳送導航資料", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK && FlightInfoRight.SelSend != -1)
            //IsGoHomeSel
            {
                if (MessageBox.Show("將傳送 " + NavPt.Count + "個導航點，返回起始點:" + GoHome + "\n是否傳送導航資料(2/2)", "傳送導航資料", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {

                    if (ComBoBoX.SelectedIndex != 2)
                    {
                        foreach (var item in NavPt)
                        {
                            Buffer.BlockCopy(BitConverter.GetBytes(item.SPD), 0, NavPtData, pos, 4);
                            pos = pos + 4;
                            Buffer.BlockCopy(BitConverter.GetBytes(item.ALT), 0, NavPtData, pos, 4);
                            pos = pos + 4;
                            Buffer.BlockCopy(BitConverter.GetBytes(item.NavPt.Lat), 0, NavPtData, pos, 8);
                            pos = pos + 8;
                            Buffer.BlockCopy(BitConverter.GetBytes(item.NavPt.Lng), 0, NavPtData, pos, 8);
                            pos = pos + 8;
                            LogSave += $"[{item.SPD},{item.ALT},[{item.NavPt.Lat},{item.NavPt.Lng }]";
                            NavPtLatLng.Add(new PointLatLng(item.NavPt.Lat, item.NavPt.Lng));
                            //Console.WriteLine(item.SPD + "," + item.ALT + "," + item.NavPt.Lat + "," + item.NavPt.Lng);
                        }
                        Buffer.BlockCopy(BitConverter.GetBytes(int.Parse(LapTxTBox.Text)), 0, NavPtData, pos, 4);
                        pos = pos + 4;
                        Buffer.BlockCopy(BitConverter.GetBytes(isForward), 0, NavPtData, pos, 1);
                        pos = pos + 1;
                    }
                    else
                    {
                        foreach (var item in NavPt)
                        {
                            if (one < 1)
                            {
                                Buffer.BlockCopy(BitConverter.GetBytes(item.SPD), 0, NavPtData, pos, 4);
                                pos = pos + 4;
                                Buffer.BlockCopy(BitConverter.GetBytes(item.ALT), 0, NavPtData, pos, 4);
                                pos = pos + 4;
                                Buffer.BlockCopy(BitConverter.GetBytes(item.NavPt.Lat), 0, NavPtData, pos, 8);
                                pos = pos + 8;
                                Buffer.BlockCopy(BitConverter.GetBytes(item.NavPt.Lng), 0, NavPtData, pos, 8);
                                pos = pos + 8;
                                Buffer.BlockCopy(BitConverter.GetBytes((float)RtV.HeightTx / 2), 0, NavPtData, pos, 4);
                                pos = pos + 4;
                                Buffer.BlockCopy(BitConverter.GetBytes((float)RtV.WidthTx / 2), 0, NavPtData, pos, 4);
                                pos = pos + 4;
                                Buffer.BlockCopy(BitConverter.GetBytes(int.Parse(LapTxTBox.Text)), 0, NavPtData, pos, 4);
                                pos = pos + 4;
                                Buffer.BlockCopy(BitConverter.GetBytes(isForward), 0, NavPtData, pos, 1);
                                pos = pos + 1;
                            }

                            if (one >= 1)
                            {
                                LogSave += $"[{item.SPD},{item.ALT},[{item.NavPt.Lat},{item.NavPt.Lng }]";
                                NavPtLatLng.Add(new PointLatLng(item.NavPt.Lat, item.NavPt.Lng));
                            }
                            one++;
                            //Console.WriteLine(item.SPD + "," + item.ALT + "," + item.NavPt.Lat + "," + item.NavPt.Lng);
                        }
                        Mode = 1;
                    }
                    isClickSend = true;
                    Buffer.BlockCopy(BitConverter.GetBytes(GoHome), 0, NavPtData, pos, 1);         
                    TcpServer.SendNAV(FlightInfoRight.SelSend, NavPtData, LogSave, Mode);
                }
            }
            else
            {
                MessageBox.Show("請於下方選擇傳送資料的對象!!", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //Console.WriteLine(NavPtData.Length);
        }
        #endregion

        #region SetTakeoffBt
        private void TakeoffBt_Checked(object sender, RoutedEventArgs e)
        {
            AllTakeoffBt.IsChecked = false;
        }

        private void AllTakeoffBt_Checked(object sender, RoutedEventArgs e)
        {
            TakeoffBt.IsChecked = false;
        }

        private void TakeoffBt_Unchecked(object sender, RoutedEventArgs e)
        {
            if (AllTakeoffBt.IsChecked == false)
            {
                TakeoffBt.IsChecked = true;
            }
        }

        private void AllTakeoffBt_Unchecked(object sender, RoutedEventArgs e)
        {
            if (TakeoffBt.IsChecked == false)
            {
                AllTakeoffBt.IsChecked = true;
            }
        }
        #endregion

        private void RtMap_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            RtMap.MapProvider = GMapProviders.GoogleSatelliteMap;
            RtMap.Position = new PointLatLng(Pos.Lat, Pos.Lng);
            RtMap.MaxZoom = 30;
            RtMap.MinZoom = 10;
            RtMap.Zoom = 20;
            //Console.WriteLine(RtMap.PositionPixel);
            TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
        }
        private void RtMap_OnMapDrag()
        {
            switch (ComBoBoX.SelectedIndex)
            {
                case 0:
                    TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
                    break;
                case 1:
                    TE(RtV.SLTx, 0, RtV.AngleTx);
                    break;
                case 2:
                    TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
                    break;
            }
            //TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);//BUG
        }
        private void TE(double h, double w, int angle)
        {
            //Console.WriteLine(ComBoBoX.SelectedIndex);
            NavPt.Clear();
            switch (ComBoBoX.SelectedIndex)
            {
                case 0:
                    Rectangle(RtMap.Position, h, w, angle);
                    break;
                case 1:
                    Triangle(RtMap.Position, h, angle);
                    break;
                case 2:
                    Ellipse(RtMap.Position, h, w, angle);
                    break;
            }
            AltDGRef();
        }
        private void AltDGRef()
        {
            Ref.Start();
        }
        private void RefData(object sender, EventArgs e)
        {
            foreach (var data in NavPt)
            {
                data.ALT = RtV.AltTx;
            }
            AltSet = false;
            NavPtDataGrid.ItemsSource = null;
            NavPtDataGrid.ItemsSource = NavPt;

            NavPtDataGrid.Height = STP.ActualHeight / 2;
            Ref.Stop();
        }

        #region 正三角形
        private void Triangle(PointLatLng StartPosition, double SideLength, int Angle)
        {
            List<PointLatLng> TriPoint = new List<PointLatLng>(); 
            TriPoint.Add(StartPosition);
            var No1Pt = FindPointAtDistanceFrom(StartPosition, Angle, SideLength, 4);
            var No2Pt = FindPointAtDistanceFrom(StartPosition, Angle, SideLength, 5);
            TriPoint.Add(No1Pt);
            TriPoint.Add(No2Pt);
            TriPoint.Add(StartPosition);

            for (int i = 0; i < TriPoint.Count; i++)
            {
                if (i > 1)
                {
                    double CKdist =
GeoCalculator.GetDistance(TriPoint[i - 1].Lat, TriPoint[i - 1].Lng, TriPoint[i].Lat, TriPoint[i].Lng, 5, DistanceUnit.Meters);
                    Console.WriteLine("" + CKdist + " M " + i);
                }

                NavData data = new NavData
                {
                    ID = i,
                    SPD = 1.5f,
                    ALT = RtV.AltTx,
                    NavPt = new PointLatLng(TriPoint[i].Lat, TriPoint[i].Lng)
                };
                NavPt.Add(data);
            }
            NavPtDataGrid.ItemsSource = null;
            NavPtDataGrid.ItemsSource = NavPt;

            GMapRoute gmRoute = new GMapRoute(TriPoint);//
            gmRoute.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
            gmRoute.Tag = "Route";
            GMapMarker Route = RtMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Route");
            if (Route != null)
            {
                int x = RtMap.Markers.IndexOf(RtMap.Markers.FirstOrDefault(u => u.Tag.ToString() == "Route"));
                RtMap.Markers.Remove(Route);
                RtMap.Markers.Add(gmRoute);
                //Console.WriteLine(x);
                //Route = new GMapRoute(MoveHistory);
                //Route.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
                //Route.Tag = "Route";
            }
            else
            {
                RtMap.Markers.Add(gmRoute);
            }

        }
        #endregion

        #region 矩形
        private void Rectangle(PointLatLng StartPosition, double H, double W, int Angle)
        {
            List<PointLatLng> RecPoint = new List<PointLatLng>();
            RecPoint.Add(StartPosition);

            var NewNo1 = FindPointAtDistanceFrom(StartPosition, Angle, W, 1);
            var NewNo2 = FindPointAtDistanceFrom(NewNo1, Angle, H, 2);
            var NewNo3 = FindPointAtDistanceFrom(NewNo2, Angle, W, 3);

            RecPoint.Add(NewNo1);
            RecPoint.Add(NewNo2);
            RecPoint.Add(NewNo3);
            RecPoint.Add(StartPosition);

            for (int i = 0; i < RecPoint.Count; i++)
            {
                if (i > 1)
                {
                    double CKdist =
GeoCalculator.GetDistance(RecPoint[i - 1].Lat, RecPoint[i - 1].Lng, RecPoint[i].Lat, RecPoint[i].Lng, 5, DistanceUnit.Meters);
                    Console.WriteLine("轉換前 " + CKdist + " M " + i);
                }

                NavData data = new NavData
                {
                    ID = i,
                    SPD = 1.5f,
                    ALT = RtV.AltTx,
                    NavPt = new PointLatLng(RecPoint[i].Lat, RecPoint[i].Lng)
                };
                NavPt.Add(data);
            }
            //NavPtDataGrid.ItemsSource = null;
            NavPtDataGrid.ItemsSource = NavPt;
            GMapRoute gmRoute = new GMapRoute(RecPoint);//
            gmRoute.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
            gmRoute.Tag = "Route";
            GMapMarker Route = RtMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Route");
            if (Route != null)
            {
                int x = RtMap.Markers.IndexOf(RtMap.Markers.FirstOrDefault(u => u.Tag.ToString() == "Route"));
                RtMap.Markers.Remove(Route);
                RtMap.Markers.Add(gmRoute);
                //Console.WriteLine(x);
                //Route = new GMapRoute(MoveHistory);
                //Route.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
                //Route.Tag = "Route";
            }
            else
            {
                RtMap.Markers.Add(gmRoute);
            }
            //double dist =
            //    GeoCalculator.GetDistance(StartLat, StartLng, StartLat, StartLng, 5, DistanceUnit.Meters);

            //Console.WriteLine("IN ADJ " + LatOneMeterCoe + " " + (double)LatOneMeterCoe + " FINAL " + dist);
        }
        #endregion

        #region 橢圓形        
        double x0 = 0, y0 = 0;
        private void Ellipse(PointLatLng StartPosition, double H, double W, int Angle)
        {
            double a = 0;
            List<PointLatLng> ElliPoint = new List<PointLatLng>();
            var w = W / 2;
            var h = H / 2;
            var oneLat = oneMeterLat(StartPosition);
            var oneLng = oneMeterLng(StartPosition);
            //x X+w+(1-α) (X-w) α=0~1(0.1) 0.01111111111
            //y Y+1 sqrt(1-(((Z-0)/(w)))^(2))
            //Console.WriteLine(oneMeterLat(StartPosition));
            ElliPoint.Add(StartPosition);
            for (int i = 0; i < 90; i++)
            {                
                var x = x0 + w + (1 + a) * (x0  - w);
                var y = y0 + h * Math.Sqrt(1 - Math.Pow((x - x0) / w, 2));
                a += 0.01111111111;
                if (a > 1) a = 1;
                var l = Math.Sqrt(Math.Pow(Math.Abs(x) - x0, 2) + Math.Pow(y - y0, 2));
                var pto = new PointLatLng(StartPosition.Lat + (oneLat * y), StartPosition.Lng + (oneLng * Math.Abs(x)));
                //Console.WriteLine("l:" + l + " x:" + Math.Abs(x) + " y:" + y + " " + i);
                //var pt = FindPointAtDistanceFrom(StartPosition, i, l, 0);
                ElliPoint.Add(pto);
            }
            //var finalpt1 = new PointLatLng(StartPosition.Lat, StartPosition.Lng + (oneLng * w));
            //ElliPoint.Add(finalpt1);
            for (int i = 0; i < 90; i++)
            {
                var x = x0 + w + (1 + a) * (x0 - w);
                var y = y0 - h * Math.Sqrt(1 - Math.Pow((Math.Abs(x) - x0) / w, 2));
                a -= 0.01111111111;//0.0055
                if (a > 1) a = 1;
                var l = Math.Sqrt(Math.Pow(Math.Abs(x) - x0, 2) + Math.Pow(y - y0, 2));
                var pto = new PointLatLng(StartPosition.Lat + (oneLat * y), StartPosition.Lng - (oneLng * x));
                //Console.WriteLine("l:" + a + " x:" + Math.Abs(x) + " y:" + y + " " + i);
                //var pt = FindPointAtDistanceFrom(StartPosition, i, l, 0);
                ElliPoint.Add(pto);
            }
            a = 0;
            for (int i = 0; i < 90; i++)
            {
                var x = x0 + w + (1 - a) * (x0 - w);
                var y = y0 - h * Math.Sqrt(1 - Math.Pow((x - x0) / w, 2));
                a -= 0.01111111111;
                if (a > 1) a = 1;
                var l = Math.Sqrt(Math.Pow(Math.Abs(x) - x0, 2) + Math.Pow(y - y0, 2));
                var pto = new PointLatLng(StartPosition.Lat + (oneLat * y), StartPosition.Lng + (oneLng * x));
                //Console.WriteLine("l:" + a + " x:" + x + " y:" + y + " " + i);
                //var pt = FindPointAtDistanceFrom(StartPosition, i, l, 0);
                ElliPoint.Add(pto);
            }
            //var finalpt2 = new PointLatLng(StartPosition.Lat, StartPosition.Lng - (oneLng * w));
            //ElliPoint.Add(finalpt2);
            for (int i = 0; i < 89; i++)
            {
                var x = x0 + w + (1 + a) * (x0 - w);
                var y = y0 - h * Math.Sqrt(1 - Math.Pow((Math.Abs(x) - x0) / w, 2));
                a += 0.01111111111;
                if (a > 1) a = 1;
                var l = Math.Sqrt(Math.Pow(Math.Abs(x) - x0, 2) + Math.Pow(y - y0, 2));
                var pto = new PointLatLng(StartPosition.Lat - (oneLat * y), StartPosition.Lng - (oneLng * x));

                //Console.WriteLine("l:" + a + " x:" + Math.Abs(x) + " y:" + y + " " + i);
                //var pt = FindPointAtDistanceFrom(StartPosition, i, l, 0);
                ElliPoint.Add(pto);
            }
            ElliPoint.Add(new PointLatLng(StartPosition.Lat + (oneLat * h), StartPosition.Lng));
            Console.WriteLine(ElliPoint.Count);

            for (int i = 0; i < ElliPoint.Count; i++)
            {
                if (i > 1)
                {
                    double CKOdist =
                    GeoCalculator.GetDistance(StartPosition.Lat, StartPosition.Lng, ElliPoint[i].Lat, ElliPoint[i].Lng, 5, DistanceUnit.Meters);
                    double CKdist = GeoCalculator.GetDistance(ElliPoint[i - 1].Lat, ElliPoint[i - 1].Lng, ElliPoint[i].Lat, ElliPoint[i].Lng, 5, DistanceUnit.Meters);
                    Console.WriteLine("距離: {0} M 距離: {1} M {2}", CKdist, CKOdist, i);
                }
                NavData data = new NavData
                {
                    ID = i,
                    SPD = 1.5f,
                    ALT = RtV.AltTx,
                    NavPt = new PointLatLng(ElliPoint[i].Lat, ElliPoint[i].Lng)
                };
                NavPt.Add(data);

            }
            NavPtDataGrid.ItemsSource = null;
            NavPtDataGrid.ItemsSource = NavPt;
            //for (int i = -1; i > -91; i--)
            //{
            //    double CKdist =
            //    GeoCalculator.GetDistance(StartPosition.Lat, StartPosition.Lng, ElliPoint[91 + i].Lat, ElliPoint[91 + i].Lng, 5, DistanceUnit.Meters);
            //    var pt = FindPointAtDistanceFrom(StartPosition, 90 - i, CKdist, 0);
            //    ElliPoint.Add(pt);
            //    Console.WriteLine(CKdist + " M " + i);
            //}

            GMapRoute gmRoute = new GMapRoute(ElliPoint);//
            gmRoute.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
            gmRoute.Tag = "Route";
            GMapMarker Route = RtMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Route");
            if (Route != null)
            {
                int x = RtMap.Markers.IndexOf(RtMap.Markers.FirstOrDefault(u => u.Tag.ToString() == "Route"));
                RtMap.Markers.Remove(Route);
                RtMap.Markers.Add(gmRoute);
                //Console.WriteLine(x);
                //Route = new GMapRoute(MoveHistory);
                //Route.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
                //Route.Tag = "Route";
            }
            else
            {
                RtMap.Markers.Add(gmRoute);
            }


            #region
            //for (int i = 0; i <= 90; i++)
            //{
            //    //if (i == 90) h = w;
            //    var pt = FindPointAtDistanceFrom(StartPosition, i, h, 0);
            //    if (h > w)
            //    {
            //        ha = ha / 45;
            //        h = h - ha;
            //    }

            //    Console.WriteLine(h +" "+i);
            //    ElliPoint.Add(pt);

            //    GMapRoute gmRoute = new GMapRoute(ElliPoint);//
            //    gmRoute.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
            //    gmRoute.Tag = "Route";
            //    GMapMarker Route = RtMap.Markers.Where(u => u.Tag != null).FirstOrDefault(u => u.Tag.ToString() == "Route");
            //    if (Route != null)
            //    {
            //        int x = RtMap.Markers.IndexOf(RtMap.Markers.FirstOrDefault(u => u.Tag.ToString() == "Route"));
            //        RtMap.Markers.Remove(Route);
            //        RtMap.Markers.Add(gmRoute);
            //        //Console.WriteLine(x);
            //        //Route = new GMapRoute(MoveHistory);
            //        //Route.Shape = new System.Windows.Shapes.Path() { Stroke = new SolidColorBrush(Colors.Yellow), StrokeThickness = 2 };
            //        //Route.Tag = "Route";
            //    }
            //    else
            //    {
            //        RtMap.Markers.Add(gmRoute);
            //    }
            //}

            //for (int j = 1; j < ElliPoint.Count; j++)
            //{
            //    double CKdist =
            //    GeoCalculator.GetDistance(StartPosition.Lat, StartPosition.Lng, ElliPoint[j].Lat, ElliPoint[j].Lng, 5, DistanceUnit.Meters);
            //    //Console.WriteLine("" + CKdist + " M " + j);
            //}
            #endregion
        }
        private double oneMeterLat(PointLatLng Startpt)
        {            
            return 1 / GeoCalculator.GetDistance(Startpt.Lat, Startpt.Lng, Startpt.Lat + 1, Startpt.Lng, 12, DistanceUnit.Meters);
        }
        private double oneMeterLng(PointLatLng Startpt)
        {
            return 1 / GeoCalculator.GetDistance(Startpt.Lat, Startpt.Lng, Startpt.Lat, Startpt.Lng + 1, 12, DistanceUnit.Meters);
        }

        #endregion

        #region Tool
        public static PointLatLng FindPointAtDistanceFrom(PointLatLng startPoint, double Angle, double distancemetres, int pt)
        {
            const double radiusEarthKilometres = 6371.009;
            var distRatio = distancemetres / 1000 / radiusEarthKilometres;
            double initialBearingRadians = Angle * Math.PI / 180;
            switch (pt)
            {
                case 1:
                    initialBearingRadians = (Angle + 90) * Math.PI / 180;
                    break;
                case 2:
                    initialBearingRadians = (Angle + 180) * Math.PI / 180;
                    break;
                case 3:
                    initialBearingRadians = (Angle + 270) * Math.PI / 180;
                    break;
                case 4:
                    initialBearingRadians = (Angle + 150) * Math.PI / 180;
                    break;
                case 5:
                    initialBearingRadians = (Angle + 210) * Math.PI / 180;
                    break;
            }            
            //var distRatio = distancemetres / 1000 / radiusEarthKilometres;
            var distRatioSine = Math.Sin(distRatio);
            var distRatioCosine = Math.Cos(distRatio);

            var startLatRad = DegreesToRadians(startPoint.Lat);
            var startLonRad = DegreesToRadians(startPoint.Lng);

            var startLatCos = Math.Cos(startLatRad);
            var startLatSin = Math.Sin(startLatRad);

            var endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(initialBearingRadians)));
            var endLonRads = startLonRad + Math.Atan2(Math.Sin(initialBearingRadians) * distRatioSine * startLatCos, distRatioCosine - startLatSin * Math.Sin(endLatRads));

            return new PointLatLng(RadiansToDegrees(endLatRads), RadiansToDegrees(endLonRads));
        }     
        public static double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }
        public static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }
        #endregion

        #region Control
        private void RtMap_MouseMove(object sender, MouseEventArgs e)
        {
            //var lat = RtMap.FromLocalToLatLng(e.GetPosition e.Y).Lat;
            //var lng = RtMap.FromLocalToLatLng(e.X, e.Y).Lng;
            //label1.Text = "lat= " + Convert.ToString(lat) + "   lng= " + Convert.ToString(lng);
            //label1.BackColor = Color.Transparent;
            //Console.WriteLine("X:"+e.GetPosition(RtMap).X);
            //Console.WriteLine("Y:"+e.GetPosition(RtMap).Y);
            //mouseY = e.Y;
            //mouseX = e.Location.X;
            //label1.Location = new Point(mouseX, mouseY + 10);
        }

        #region Setting
        private void HPlusBt_Click(object sender, RoutedEventArgs e)
        {
            RtV.HeightTx += double.Parse(HeightTxTBox.Text);
            TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
        }
        private void HDimBt_Click(object sender, RoutedEventArgs e)
        {
            if (RtV.HeightTx > 0)
            {
                RtV.HeightTx -= double.Parse(HeightTxTBox.Text);
                TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
            }
        }
        private void WPlusBt_Click(object sender, RoutedEventArgs e)
        {
            RtV.WidthTx += double.Parse(WidthTxTBox.Text);
            TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
        }
        private void WDimBt_Click(object sender, RoutedEventArgs e)
        {
            if (RtV.WidthTx > 0)
            {
                RtV.WidthTx -= double.Parse(WidthTxTBox.Text);
                TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
            }
        }
        private void SLPlusBt_Click(object sender, RoutedEventArgs e)
        {
            RtV.SLTx += double.Parse(SLTxTBox.Text);
            TE(RtV.SLTx, 0, RtV.AngleTx);

        }
        private void SLWDimBt_Click(object sender, RoutedEventArgs e)
        {
            if (RtV.SLTx > 0)
            {
                RtV.SLTx -= double.Parse(SLTxTBox.Text);
                TE(RtV.SLTx, 0, RtV.AngleTx);
            }
        }
        private void AnglePlusBt_Click(object sender, RoutedEventArgs e)
        {
            if (RtV.AngleTx < 360)
            {
                RtV.AngleTx += int.Parse(AngleTxTBox.Text);
                TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
            }
        }
        private void AngleDimBt_Click(object sender, RoutedEventArgs e)
        {
            if (RtV.AngleTx > -360)
            {
                RtV.AngleTx -= int.Parse(AngleTxTBox.Text);
                TE(RtV.HeightTx, RtV.WidthTx, RtV.AngleTx);
            }
        }
        private void AltPlusBt_Click(object sender, RoutedEventArgs e)
        {
            RtV.AltTx += float.Parse(AltTxTBox.Text);
            AltSet = true;
            AltDGRef();

        }

        private void AltDimBt_Click(object sender, RoutedEventArgs e)
        {
            if (RtV.AltTx > 0)
            {
                RtV.AltTx -= float.Parse(AltTxTBox.Text);
                AltSet = true;
                AltDGRef();
            }
        }
        #endregion
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //EQtriangle
            if (ComBoXIsLoad)
            {
                if (ComBoBoX.SelectedIndex == 0 || ComBoBoX.SelectedIndex == 2)
                {
                    REct.Visibility = Visibility.Visible;
                    EQtriangle.Visibility = Visibility.Collapsed;
                    TE(RtV.HeightTx, RtV.WidthTx, 0);
                }
                else
                {
                    EQtriangle.Visibility = Visibility.Visible;
                    REct.Visibility = Visibility.Collapsed;
                    TE(RtV.SLTx, 0, RtV.AngleTx);
                }
            }
            //Console.WriteLine(ComBoBoX.SelectedIndex);
        }

        private void ComBoBoX_Loaded(object sender, RoutedEventArgs e)
        {
            ComBoXIsLoad = true;
        }
        #endregion
    }
}
