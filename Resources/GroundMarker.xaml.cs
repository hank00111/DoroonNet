using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace DoroonNet.Resources
{
    /// <summary>
    /// GroundMarker.xaml 的互動邏輯
    /// </summary>
    public partial class GroundMarker : UserControl
    {
        public MainWindow MainWindow { get; }

        GMapMarker Marker;
        public GroundMarker(MainWindow window, GMapMarker marker)
        {
            InitializeComponent();
            this.MainWindow = window;
            this.Marker = marker;
            this.Loaded += new RoutedEventHandler(GroundMarkerr_Loaded);
            this.Unloaded += new RoutedEventHandler(GroundMarkerr_Unloaded);
            this.SizeChanged += new SizeChangedEventHandler(GroundMarkerr_SizeChanged);
            //ThreadPool.QueueUserWorkItem(Base_light);
        }

        private void GroundMarkerr_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void GroundMarkerr_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(GroundMarkerr_Loaded);
            this.Unloaded -= new RoutedEventHandler(GroundMarkerr_Unloaded);
            this.SizeChanged -= new SizeChangedEventHandler(GroundMarkerr_SizeChanged);

            Marker.Shape = null;
            icon.Source = null;
            icon = null;
        }

        void GroundMarkerr_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height / 2);
        }

        private void Base_light(Object stateInfo)
        {
            double Opac;
            while (true)
            {
                for (Opac = 0.0; Opac < 1.0; Opac += 1)
                {
                    SpinWait.SpinUntil(() => false, 800);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        icon.Opacity = Opac;
                    }));

                }
                SpinWait.SpinUntil(() => false, 2000);
                for (Opac = 1.0; Opac > 0.1; Opac -= 0.5)
                {
                    SpinWait.SpinUntil(() => false, 20);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        icon.Opacity = Opac;
                    }));
                }
            }
        }

    }
}
