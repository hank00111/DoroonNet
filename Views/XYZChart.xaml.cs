using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using DoroonNet.Command;
using DoroonNet.ViewModel;
using ScottPlot;
using ScottPlot.Control;
using ScottPlot.Plottable;

namespace DoroonNet.Views
{
    /// <summary>
    /// XYZChart.xaml 的互動邏輯
    /// </summary>
    public partial class XYZChart : UserControl
    {

        SignalPlot aPlot1, aPlot2, aPlot3, 
            bPlot1, bPlot2, bPlot3, 
            cPlot1, cPlot2, cPlot3;
        public static int nextDataIndex = 0;
        int i = 0;
        //bool running = false;

        #region DataArry
        const int DataCount = 3040;//
        double[] a_data1 = new double[DataCount];
        double[] a_data2 = new double[DataCount];
        double[] a_data3 = new double[DataCount];

        double[] b_data1 = new double[DataCount];
        double[] b_data2 = new double[DataCount];
        double[] b_data3 = new double[DataCount];

        double[] c_data1 = new double[DataCount];
        double[] c_data2 = new double[DataCount];
        double[] c_data3 = new double[DataCount];
        #endregion

        double[] xPos = new double[60];
        double[] yPos = {-360, -180, -90, 0, 90, 180, 360 };
        string[] yTick = {"-360", "-180", "-90", "0", "90", "180", "360" };
        string[] xTick = new string[60];

        //readonly ScottPlot.Plottable.VLine VerticalLine;
        public DispatcherTimer _updateDataTimer, _updateDataTimer1, _updateDataTimer2;
        private DispatcherTimer _renderTimer;//_renderTimer, _renderTimer1, _renderTimer2
        //private System.Timers.Timer RenderTime;

        public static float X = 0, Y = 0, Z = 0, X1 = 0, Y1 = 0, Z1 = 0, X2 = 0, Y2 = 0, Z2 = 0;//#1
        bool running;
        public static bool IsDisconnect = true;        

        public static int SelClient = 0;
        private static InfoViewModel ins = new InfoViewModel();
        //private Thread MissionA_Thread;
        private static AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);
        GData data = new GData();

        public XYZChart()
        {
            InitializeComponent();
            xTickcount();
            PlotStyle();
            this.DataContext = new InfoViewModel();
            SelectList.Items.Clear();    
            
        }

        int connnt = 0;

        private void Testfunc(object sender, EventArgs e)
        {
            connnt += 1;
            if (connnt == 60) connnt = 0;
            Console.WriteLine(nextDataIndex+" "+connnt);
        }

        #region ChartInit
        public void ChartGraphs_a()
        {           
            aPlot1 = A_Plot.Plot.AddSignal(a_data1, sampleRate: 1, label: "x");
            aPlot1.LineWidth = 2.8;
            aPlot1.MaxRenderIndex = nextDataIndex;

            aPlot2 = A_Plot.Plot.AddSignal(a_data2, sampleRate: 1, label: "y");
            aPlot2.LineWidth = 2.8;
            aPlot2.MaxRenderIndex = nextDataIndex;

            aPlot3 = A_Plot.Plot.AddSignal(a_data3, sampleRate: 1, label: "z");
            aPlot3.LineWidth = 2.8;
            aPlot3.MaxRenderIndex = nextDataIndex;

            A_Plot.Plot.Title("歐拉角");
            A_Plot.Plot.Palette = ScottPlot.Palette.OneHalfDark;
            A_Plot.Render();

            _updateDataTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };
            _updateDataTimer.Tick += UpdateData;
            //_updateDataTimer.Start();

            // create a timer to update the GUI
            _renderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            _renderTimer.Tick += Render;
            _renderTimer.Tick += Render1;
            _renderTimer.Tick += Render2;
            //_renderTimer.Start();
            //Closed += (sender, args) =>
            //{
            //    _updateDataTimer?.Stop();
            //    _renderTimer?.Stop();
            //};
        }

        public void ChartGraphs_b()
        {
            bPlot1 = B_Plot.Plot.AddSignal(b_data1, sampleRate: 1, label: "x");
            bPlot1.LineWidth = 2.8;
            bPlot1.MaxRenderIndex = nextDataIndex;

            bPlot2 = B_Plot.Plot.AddSignal(b_data2, sampleRate: 1, label: "y");
            bPlot2.LineWidth = 2.8;
            bPlot2.MaxRenderIndex = nextDataIndex;

            bPlot3 = B_Plot.Plot.AddSignal(b_data3, sampleRate: 1, label: "z");
            bPlot3.LineWidth = 2.8;
            bPlot3.MaxRenderIndex = nextDataIndex;

            B_Plot.Plot.Title("角速度");
            B_Plot.Plot.Palette = ScottPlot.Palette.OneHalfDark;
            B_Plot.Render();

            _updateDataTimer1 = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(5)
            };
            _updateDataTimer.Tick += UpdateData1;

            //_renderTimer1 = new DispatcherTimer
            //{
            //    Interval = TimeSpan.FromMilliseconds(20)
            //};
            //_renderTimer1.Tick += Render1;
            //_renderTimer1.Start();
        }

        public void ChartGraphs_c()
        {
            cPlot1 = C_Plot.Plot.AddSignal(c_data1, sampleRate: 1, label: "x");
            cPlot1.LineWidth = 2.5;
            cPlot1.MaxRenderIndex = nextDataIndex;

            cPlot2 = C_Plot.Plot.AddSignal(c_data2, sampleRate: 1, label: "y");
            cPlot2.LineWidth = 2.5;
            cPlot2.MaxRenderIndex = nextDataIndex;

            cPlot3 = C_Plot.Plot.AddSignal(c_data3, sampleRate: 1, label: "z");
            cPlot3.LineWidth = 2.5;
            cPlot3.MaxRenderIndex = nextDataIndex;

            C_Plot.Plot.Title("加速度");
            C_Plot.Plot.Palette = ScottPlot.Palette.OneHalfDark;
            C_Plot.Render();

            _updateDataTimer2 = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(5)
            };
            _updateDataTimer2.Tick += UpdateData2;

            //// create a timer to update the GUI
            //_renderTimer2 = new DispatcherTimer
            //{
            //    Interval = TimeSpan.FromMilliseconds(20)
            //};
            //_renderTimer2.Tick += Render2;
            //_renderTimer2.Start();
        }
        #endregion
        double te = 1;
        #region Data
        private void UpdateData(object sender, EventArgs e)
        {
            te+=0.05;
            if (nextDataIndex >= a_data2.Length)
            {
                nextDataIndex = 0;
                te = 0;
            }
            if (IsDisconnect)
            {                
                if (X != 0)
                {
                    X = 0;
                    Y = 0;
                    Z = 0;
                }
            }
            //data.Gdata1[data.X,nextDataIndex] = te;

            a_data1[nextDataIndex] = X;
            a_data2[nextDataIndex] = Y;
            a_data3[nextDataIndex] = Z;

            aPlot1.MaxRenderIndex = nextDataIndex;
            aPlot2.MaxRenderIndex = nextDataIndex;
            aPlot3.MaxRenderIndex = nextDataIndex;

            nextDataIndex += 1;
        }

        private void UpdateData1(object sender, EventArgs e)
        {
            if (nextDataIndex >= b_data2.Length)
            {
                nextDataIndex = 0;
                te = 0;
            }

            if (IsDisconnect)
            {
                if (X1 != 0)
                {
                    X1 = 0;
                    Y1 = 0;
                    Z1 = 0;
                }
            }

            b_data1[nextDataIndex] = X1;//1
            b_data2[nextDataIndex] = Y1;//1
            b_data3[nextDataIndex] = Z1;//1

            bPlot1.MaxRenderIndex = nextDataIndex;
            bPlot2.MaxRenderIndex = nextDataIndex;
            bPlot3.MaxRenderIndex = nextDataIndex;

        }

        private void UpdateData2(object sender, EventArgs e)
        {
            if (nextDataIndex >= c_data2.Length)
            {
                nextDataIndex = 0;
                te = 0;
            }

            if (IsDisconnect)
            {
                if (X2 != 0)
                {
                    X2 = 0;
                    Y2 = 0;
                    Z2 = 0;
                }
            }

            c_data1[nextDataIndex] = X2;//1
            c_data2[nextDataIndex] = Y2;//1
            c_data3[nextDataIndex] = Z2;//1

            cPlot1.MaxRenderIndex = nextDataIndex;
            cPlot2.MaxRenderIndex = nextDataIndex;
            cPlot3.MaxRenderIndex = nextDataIndex;

        }
        #endregion

        private void PlotStyle()
        {
            double Xmax = 3550;
            A_Plot.Plot.Style(
                figureBackground: ColorTranslator.FromHtml("#00131a"), 
                dataBackground: ColorTranslator.FromHtml("#2e3440"), 
                grid: ColorTranslator.FromHtml("#777B7E"), tick: Color.White, titleLabel: Color.White);
            B_Plot.Plot.Style(
                figureBackground: ColorTranslator.FromHtml("#00131a"), 
                dataBackground: ColorTranslator.FromHtml("#2e3440"), 
                grid: ColorTranslator.FromHtml("#777B7E"), tick: Color.White, titleLabel: Color.White);
            C_Plot.Plot.Style(
                figureBackground: ColorTranslator.FromHtml("#00131a"), 
                dataBackground: ColorTranslator.FromHtml("#2e3440"), 
                grid: ColorTranslator.FromHtml("#777B7E"), tick: Color.White, titleLabel: Color.White);
            //15202B
            A_Plot.Plot.SetAxisLimits(xMin: 0, xMax: Xmax, yMin: -180, yMax: 180);
            B_Plot.Plot.SetAxisLimits(xMin: 0, xMax: Xmax, yMin: -200, yMax: 200);
            C_Plot.Plot.SetAxisLimits(xMin: 0, xMax: Xmax, yMin: -20, yMax: 20);

            A_Plot.Plot.XAxis.TickLabelStyle(fontSize: 14, fontName: "Roboto");
            A_Plot.Plot.YAxis.TickLabelStyle(fontSize: 14, fontName: "Roboto");

            B_Plot.Plot.XAxis.TickLabelStyle(fontSize: 14, fontName: "Roboto");
            B_Plot.Plot.YAxis.TickLabelStyle(fontSize: 14, fontName: "Roboto");

            C_Plot.Plot.XAxis.TickLabelStyle(fontSize: 14, fontName: "Roboto");
            C_Plot.Plot.YAxis.TickLabelStyle(fontSize: 14, fontName: "Roboto");

            A_Plot.Plot.YAxis.Label("deg");
            B_Plot.Plot.YAxis.Label("deg/s");
            C_Plot.Plot.YAxis.Label("m/s^2");

            A_Plot.Plot.YAxis.Color(Color.White);
            B_Plot.Plot.YAxis.Color(Color.White);
            C_Plot.Plot.YAxis.Color(Color.White);

            A_Plot.Plot.Legend();
            B_Plot.Plot.Legend();
            C_Plot.Plot.Legend();

            A_Plot.Plot.XTicks(xPos, xTick);
            B_Plot.Plot.XTicks(xPos, xTick);
            C_Plot.Plot.XTicks(xPos, xTick);

            A_Plot.Plot.YTicks(yPos, yTick);
            //B_Plot.Plot.YTicks(yPos, yTick);
            //C_Plot.Plot.YTicks(yPos, yTick);

            A_Plot.Plot.SetOuterViewLimits(0, Xmax, -180, 180);
            B_Plot.Plot.SetOuterViewLimits(0, Xmax, -200, 200);
            C_Plot.Plot.SetOuterViewLimits(0, Xmax, -20, 20);

            A_Plot.Configuration.Quality = QualityMode.LowWhileDragging;
            B_Plot.Configuration.Quality = QualityMode.LowWhileDragging;
            C_Plot.Configuration.Quality = QualityMode.LowWhileDragging;
        }

        #region Render
        private void Render(object sender, EventArgs e)
        {
            A_Plot.Refresh();
        }

        private void Render1(object sender, EventArgs e)
        {
            B_Plot.Refresh();
        }

        private void Render2(object sender, EventArgs e)
        {
            C_Plot.Refresh();
        }
        #endregion

        #region Control
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ChartGraphs_a();
            //Expan.IsExpanded = true;       
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            _updateDataTimer?.Start();
            _updateDataTimer1?.Start();
            _updateDataTimer2?.Start();
            _renderTimer?.Start();
            //_renderTimer1?.Start();
            //_renderTimer2?.Start();
            //RenderTime.Start();
            //_AutoResetEvent.Set();
            running = true;
            Console.WriteLine("Run Chart");
            if (SelectList.SelectedIndex == -1)
            {
                SelectList.SelectedIndex = 0;
            }
            Console.WriteLine(SelectList.SelectedIndex);
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            //_updateDataTimer?.Stop();
            //_updateDataTimer1?.Stop();
            //_updateDataTimer2?.Stop();
            //_renderTimer?.Stop();
            //_renderTimer1?.Stop();
            //_renderTimer2?.Stop();
            //RenderTime.Stop();
            ////_AutoResetEvent.WaitOne();
            //running = false;
            //Console.WriteLine("Stop");
        }

        private void A_Plot_Loaded(object sender, RoutedEventArgs e)
        {
            if (i == 0)
            {
                ChartGraphs_a();
                ChartGraphs_b();
                ChartGraphs_c();
                ////RendeTime2();
                ////RenderTime = new System.Timers.Timer();
                ////RenderTime.Elapsed += RendeTime;
                ////RenderTime.Interval = 60 * 1000;
                //ThreadStart MissionA_Tg = new ThreadStart(RendeTime2);
                //MissionA_Thread = new Thread(MissionA_Tg);
                //MissionA_Thread.IsBackground = true;
                //MissionA_Thread.Start();
                ////_AutoResetEvent.WaitOne();
            }
            i++;
            //Expan.IsExpanded = true;
        }

        private void A_Plot_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //ChartGraphs_a();
            var x = A_Plot.Plot.XAxis;
            var y = A_Plot.Plot.YAxis;
            //Console.WriteLine(x+" "+y);
        }

        private void SelectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sel = SelectList.SelectedIndex;
            SelClient = sel;
            //var aa = ins.CollectionListPartial.IndexOf(ins.CollectionListPartial.Where(X => X.ID == sel).FirstOrDefault());
            if (sel != -1)
            {
                int aa = ins.CollectionListPartial.IndexOf(ins.CollectionListPartial.FirstOrDefault(X => X.ID == sel));
                nextDataIndex = 0;
                //nextDataIndex
                //Console.WriteLine(aa.ToString()+" " + sel);
                //for (int i = 0; i <= sel; i++)
                //{
                //    var aa = ins.CollectionListPartial.IndexOf(ins.CollectionListPartial.Where(X => X.ID == sel).FirstOrDefault());
                //    var bb = ins.CollectionListPartial[i];
                //    Console.WriteLine( " " + sel);
                //}
                //Console.WriteLine("AA" + sel);
            }
            //Console.WriteLine(SelClient);
            //var cc = ins.CollectionListPartial.First(X => X.ID == sel);
            //DoubleAnimation animeUP = new DoubleAnimation(1, TimeSpan.FromSeconds(0.3));
            //DoubleAnimation animeDown = new DoubleAnimation(0, TimeSpan.FromSeconds(0.3));        
        }
    
        #endregion

        private void RendeTime(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("60s " + nextDataIndex);
            //nextDataIndex = 0;
            //te = 0;
            //Console.WriteLine("60s "+ nextDataIndex);
        }

        private void RendeTime2()
        {
            while (true)
            {
                if (running)
                {
                    SpinWait.SpinUntil(() => false, 60 * 1000);
                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "[Info] " + "60s " + nextDataIndex);

                    nextDataIndex = 0;
                }                
                SpinWait.SpinUntil(() => false, 1);
            }
        }

        private void xTickcount()
        {
            int j = 0;
            for (int i = 0; i <= 60; i+=5)
            {
                xPos[j] = i * 50;
                xTick[j] = i.ToString();
                j++;
            }
        }
    }
}
