using System;
using System.Collections.Generic;
using System.IO;
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

namespace DoroonNet.Views
{
    /// <summary>
    /// SYS.xaml 的互動邏輯
    /// </summary>
    public partial class SYS : Window
    {
        double[] xx = new double[729];//894//827
        double[] yy = new double[729];
        int ix = 0;
        int iy = 0;
        //private readonly ScottPlot.Plottable.ScatterPlot MyScatterPlot;
        //private readonly ScottPlot.Plottable.ScatterPlot HighlightedPoint;
        //private int LastHighlightedIndex = -1;

        public SYS()
        {
            InitializeComponent();
            //FF();
        }

        private void FF()
        {

            //var plt = new ScottPlot.Plot(600, 400);

            // sample data
            string path = @"C:\Users\001\Desktop\X.txt";
            string path2 = @"C:\Users\001\Desktop\Y.txt";
            //lat 1/101.77545*100000
            //lng 1/110.9362*100000
            // This text is added only once to the file.

            // This text is always added, making the file longer over time
            // if it is not deleted.
            //string appendText = "This is extra text" + Environment.NewLine;
            //File.AppendAllText(path, appendText);

            // Open the file to read from.
            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                double.TryParse(s, out double x);
                xx[ix] = x * (1 / 110.9362 * 100000);
                //Console.WriteLine(x);
                ix += 1;
            }
            string[] yyyy = File.ReadAllLines(path2);
            foreach (string s in yyyy)
            {
                double.TryParse(s, out double y);
                yy[iy] = y * (1 / 101.77545 * 100000);
                iy += 1;
            }


            // plot the data
            FK.Plot.AddScatter(xx, yy);//lineWidth: 0
                                       //FK.Plot.AddScatter(xs, cos);


            // customize the axis labels
            //FK.Plot.Title("ScottPlot Quickstart");
            //FK.Plot.XLabel("Horizontal Axis");
            //FK.Plot.YLabel("Vertical Axis");
            FK.Render();
            FK.Refresh();
        }


        private void FK_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
