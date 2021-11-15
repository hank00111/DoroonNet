using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using ScottPlot.Plottable;

namespace DoroonNet.Views
{
    /// <summary>
    /// HomeView.xaml 的互動邏輯
    /// </summary>
    public partial class LiveChart : MetroWindow
    {
        public LiveChart()
        {
            InitializeComponent();
            Console.WriteLine("Start Chart");
            
        }

        #region Chart
        private DispatcherTimer _updateDataTimer;
        private DispatcherTimer _renderTimer;

        int nextDataIndex = 1;

        double[] data = new double[1000];
        double[] data2 = new double[1000];
        double[] data3 = new double[1000];

        SignalPlot signalPlot;
        SignalPlot signalPlot2;
        SignalPlot signalPlot3;
        Random rand = new Random(0);

        private void ChartGraphs()
        {
            Random rd = new Random();

            signalPlot = WpfPlot1.Plot.AddSignal(data, label: "x");
            signalPlot.MarkerSize = 2;
            signalPlot.LineWidth = 1.5;

            signalPlot2 = WpfPlot1.Plot.AddSignal(data2, label: "y");
            signalPlot2.MarkerSize = 2;
            signalPlot2.LineWidth = 1.5;

            signalPlot3 = WpfPlot1.Plot.AddSignal(data3, label: "z");
            signalPlot3.MarkerSize = 2 ;
            signalPlot3.LineWidth = 1.5;

            WpfPlot1.Plot.Palette = ScottPlot.Drawing.Palette.Category10;
            WpfPlot1.Plot.Title("Unknow");
            //WpfPlot1.Plot.XLabel("Time (seconds)");
            //WpfPlot1.Plot.XLabel("Sample Number");
            WpfPlot1.Plot.SetAxisLimits(xMin: 0, xMax: 60, yMin: -540, yMax: 540);
            //WpfPlot1.Configuration.LockVerticalAxis = true;
            //WpfPlot1.Configuration.MiddleClickAutoAxisMarginX = 0;
            WpfPlot1.Plot.Legend(location: ScottPlot.Alignment.UpperRight);
            WpfPlot1.Render();


            _updateDataTimer = new DispatcherTimer();
            _updateDataTimer.Interval = TimeSpan.FromMilliseconds(100);
            _updateDataTimer.Tick += UpdateData;
            _updateDataTimer.Start();

            // create a timer to update the GUI
            _renderTimer = new DispatcherTimer();
            _renderTimer.Interval = TimeSpan.FromMilliseconds(20);
            _renderTimer.Tick += Render;
            _renderTimer.Start();

            Closed += (sender, args) =>
            {
                _updateDataTimer?.Stop();
                _renderTimer?.Stop();
            };

        }

        int x;
        bool plus = true;

        private void UpdateData(object sender, EventArgs e)
        {
            if (nextDataIndex >= data.Length)
            {
                nextDataIndex = 0;

                //   1. clear the plot
                //   2. create a new larger array
                //   3. copy the old data into the start of the larger array
                //   4. plot the new (larger) array
                //   5. continue to update the new array
                //Array.Copy(data, 0, data, 0, data.Length - 1);
                //Array.Copy(data, 0, data, 0, data.Length - 1);
                //Array.Copy(data2, 0, data2, 0, data2.Length - 1);

            }

            if (x > 89) plus = false;
            if (x < -89) plus = true;

            if (plus)
            {
                x++;
            }
            else
            {
                x--;
            }



            double randomValue = Math.Sin(x) * 100;
            double randomValue2 = Math.Cos(x * 4) * 180;
            double randomValue3 = Math.Sin(x) * 50;//Math.Round((double)rand.Next(0, 360), 1)
            data[nextDataIndex] = randomValue;
            data2[nextDataIndex] = randomValue2;
            data3[nextDataIndex] = randomValue3;

            signalPlot.MaxRenderIndex = nextDataIndex;
            signalPlot2.MaxRenderIndex = nextDataIndex;
            signalPlot3.MaxRenderIndex = nextDataIndex;

            nextDataIndex += 1;


        }

        private void Render(object sender, EventArgs e)
        {

            //if (nextDataIndex >= 60)
            //{
            //    //x1++;
            //    WpfPlot1.Plot.SetAxisLimits(xMin: nextDataIndex - 60, xMax: nextDataIndex, yMin: -220, yMax: 380);
            //}
            //else
            //{
            //    WpfPlot1.Plot.SetAxisLimits(xMin: 0, xMax: 60, yMin: -220, yMax: 380);
            //}


            WpfPlot1.Refresh();

        }
        #endregion

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ChartGraphs();
        }
    }
}
