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

        #region DataArry
        double[] a_data1 = new double[4000];
        double[] a_data2 = new double[4000];
        double[] a_data3 = new double[4000];

        double[] b_data1 = new double[4000];
        double[] b_data2 = new double[4000];
        double[] b_data3 = new double[4000];

        double[] c_data1 = new double[4000];
        double[] c_data2 = new double[4000];
        double[] c_data3 = new double[4000];
        #endregion

        double[] xPos = new double[60];
        double[] yPos = {-180, -90, 0, 90, 180, 270, 360 };
        string[] yTick = {"-180", "-90", "0", "90", "180","270", "360" };
        string[] xTick = new string[60];

        //readonly ScottPlot.Plottable.VLine VerticalLine;
        public DispatcherTimer _updateDataTimer, _updateDataTimer1, _updateDataTimer2;
        private DispatcherTimer _renderTimer, _renderTimer1, _renderTimer2;
        //private System.Timers.Timer test;

        public static float X = 0, Y = 0, Z = 0, X1 = 0, Y1 = 0, Z1 = 0, X2 = 0, Y2 = 0, Z2 = 0;//#1

        public static bool IsDisconnect = true;

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

            _updateDataTimer = new DispatcherTimer();
            _updateDataTimer.Interval = TimeSpan.FromMilliseconds(5);
            _updateDataTimer.Tick += UpdateData;
            //_updateDataTimer.Start();

            // create a timer to update the GUI
            _renderTimer = new DispatcherTimer();
            _renderTimer.Interval = TimeSpan.FromMilliseconds(20);
            _renderTimer.Tick += Render;
            _renderTimer.Start();
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

            _updateDataTimer1 = new DispatcherTimer();
            _updateDataTimer1.Interval = TimeSpan.FromMilliseconds(5);
            _updateDataTimer.Tick += UpdateData1;

            _renderTimer1 = new DispatcherTimer();
            _renderTimer1.Interval = TimeSpan.FromMilliseconds(20);
            _renderTimer1.Tick += Render1;
            //_renderTimer1.Start();
        }

        public void ChartGraphs_c()
        {
            cPlot1 = C_Plot.Plot.AddSignal(c_data1, sampleRate: 1, label: "x");
            cPlot1.LineWidth = 2.8;
            cPlot1.MaxRenderIndex = nextDataIndex;

            cPlot2 = C_Plot.Plot.AddSignal(c_data2, sampleRate: 1, label: "y");
            cPlot2.LineWidth = 2.8;
            cPlot2.MaxRenderIndex = nextDataIndex;

            cPlot3 = C_Plot.Plot.AddSignal(c_data3, sampleRate: 1, label: "z");
            cPlot3.LineWidth = 2.8;
            cPlot3.MaxRenderIndex = nextDataIndex;

            C_Plot.Plot.Title("加速度");
            C_Plot.Plot.Palette = ScottPlot.Palette.OneHalfDark;
            C_Plot.Render();

            _updateDataTimer2 = new DispatcherTimer();
            _updateDataTimer2.Interval = TimeSpan.FromMilliseconds(5);
            _updateDataTimer2.Tick += UpdateData2;

            //// create a timer to update the GUI
            _renderTimer2 = new DispatcherTimer();
            _renderTimer2.Interval = TimeSpan.FromMilliseconds(20);
            _renderTimer2.Tick += Render2;
            //_renderTimer2.Start();
        }
        #endregion

        #region Data
        private void UpdateData(object sender, EventArgs e)
        {
            if (nextDataIndex >= a_data2.Length)
            {
                nextDataIndex = 0;
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
            A_Plot.Plot.Style(figureBackground: Color.FromArgb(17, 49, 66), dataBackground: ColorTranslator.FromHtml("#2e3440"), 
                grid: ColorTranslator.FromHtml("#777B7E"), tick: Color.White, titleLabel: Color.White);
            B_Plot.Plot.Style(figureBackground: Color.FromArgb(17, 49, 66), dataBackground: ColorTranslator.FromHtml("#2e3440"), 
                grid: ColorTranslator.FromHtml("#777B7E"), tick: Color.White, titleLabel: Color.White);
            C_Plot.Plot.Style(figureBackground: Color.FromArgb(17, 49, 66), dataBackground: ColorTranslator.FromHtml("#2e3440"), 
                grid: ColorTranslator.FromHtml("#777B7E"), tick: Color.White, titleLabel: Color.White);

            A_Plot.Plot.SetAxisLimits(xMin: 0, xMax: 4500, yMin: -185, yMax: 185);
            B_Plot.Plot.SetAxisLimits(xMin: 0, xMax: 4500, yMin: -185, yMax: 185);
            C_Plot.Plot.SetAxisLimits(xMin: 0, xMax: 4500, yMin: -185, yMax: 185);

            A_Plot.Plot.XAxis.TickLabelStyle(fontSize: 14);
            A_Plot.Plot.YAxis.TickLabelStyle(fontSize: 14);

            B_Plot.Plot.XAxis.TickLabelStyle(fontSize: 14);
            B_Plot.Plot.YAxis.TickLabelStyle(fontSize: 14);

            C_Plot.Plot.XAxis.TickLabelStyle(fontSize: 14);
            C_Plot.Plot.YAxis.TickLabelStyle(fontSize: 14);

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
            B_Plot.Plot.YTicks(yPos, yTick);
            C_Plot.Plot.YTicks(yPos, yTick);

            A_Plot.Plot.SetOuterViewLimits(0, 4500, -185, 185);
            B_Plot.Plot.SetOuterViewLimits(0, 4500, -185, 185);
            C_Plot.Plot.SetOuterViewLimits(0, 4500, -185, 185);

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
            _renderTimer1?.Start();
            _renderTimer2?.Start();
            //if (SelectList.SelectedIndex == -1)
            //{
            //    SelectList.SelectedIndex = 0;
            //}
            //Console.WriteLine(SelectList.SelectedIndex);
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            _updateDataTimer?.Stop();
            _updateDataTimer1?.Stop();
            _updateDataTimer2?.Stop();
            _renderTimer?.Stop();
            _renderTimer1?.Stop();
            _renderTimer2?.Stop();
        }

        private void A_Plot_Loaded(object sender, RoutedEventArgs e)
        {
            if (i == 0)
            {
                ChartGraphs_a(); 
                ChartGraphs_b();
                ChartGraphs_c();
                
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
            var sel = SelectList.SelectedIndex;
            //DoubleAnimation animeUP = new DoubleAnimation(1, TimeSpan.FromSeconds(0.3));
            //DoubleAnimation animeDown = new DoubleAnimation(0, TimeSpan.FromSeconds(0.3));
            switch (sel)
            {
                case 0:
                    if (A_Plot != null)
                    {
                    }
                    break;

                case 1:

                    break;

                case 2:

                    break;

                case 3:

                    break;
            }
            
        }

        #endregion

        private void xTickcount()
        {
            int j = 0;
            for (int i = 0; i <= 60; i+=5)
            {
                xPos[j] = i * 66;
                xTick[j] = i.ToString();
                j++;
            }
        }
    }
}
