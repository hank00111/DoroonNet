using DoroonNet.Command;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// M_DroneMarker.xaml 的互動邏輯
    /// </summary>
    /// 
    public class DroneControl
    {
        public static Label Label;
    }

    public partial class M_DroneMarker : UserControl
    {
        Popup Poup;
        //Label Label;
        GMapMarker Marker;
        DroneControl droneControl = new DroneControl();
        public MainWindow MainWindow { get; }
        public static Image iaC { get; set; }        

        public M_DroneMarker(MainWindow window, GMapMarker marker)
        {
            InitializeComponent();

            Poup = new Popup();
            DroneControl.Label = new Label();

            iaC = icon;
            this.MainWindow = window;
            this.Marker = marker;
            this.Loaded += new RoutedEventHandler(M_DroneMarker_Loaded);
            this.Unloaded += new RoutedEventHandler(M_DroneMarker_Unloaded);
            this.SizeChanged += new SizeChangedEventHandler(M_DroneMarker_SizeChanged);

            SolidColorBrush Background = new SolidColorBrush(Colors.Gray);//Color.FromArgb(100, 14, 255, 0)
            Background.Opacity = 0.7;

            Poup.Placement = PlacementMode.Mouse;
            {
                DroneControl.Label.Background = Background;
                DroneControl.Label.Foreground = new SolidColorBrush(Color.FromArgb(255, 14, 255, 0));
                DroneControl.Label.Padding = new Thickness(0,0,-100,100);
                DroneControl.Label.FontSize = 16;
                DroneControl.Label.FontWeight = FontWeights.Bold;
                DroneControl.Label.Content = $"HDG {VariableRes.HDG} Lat {VariableRes.Lat} Lng {VariableRes.Lat}"; 
            }
            Poup.AllowsTransparency = true;
            Poup.Child = DroneControl.Label;
        }

        private void M_DroneMarker_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        private void M_DroneMarker_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(M_DroneMarker_Loaded);
            this.Unloaded -= new RoutedEventHandler(M_DroneMarker_Unloaded);
            this.SizeChanged -= new SizeChangedEventHandler(M_DroneMarker_SizeChanged);

            Marker.Shape = null;
            icon.Source = null;
            icon = null;
        }

        private void M_DroneMarker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height / 2);
            //P_DroneMarker_Angle();
        }

        private void M_DroneMarker_MouseEnter(object sender, MouseEventArgs e)
        {
            Poup.IsOpen = true;
        }

        private void M_DroneMarker_MouseLeave(object sender, MouseEventArgs e)
        {
            Poup.IsOpen = false;
        }
    }

    public class M_DroneMarker_Func
    {
        public void M_DroneMarker_Angle(int Angle)
        {
            //int i = 0;
            RotateTransform rotateTransform = new RotateTransform(Angle);
            M_DroneMarker.iaC.RenderTransformOrigin = new Point(0.5, 0.5); //Set Center Rotate
            M_DroneMarker.iaC.RenderTransform = rotateTransform;
            //return 123;
        }


    }
}
