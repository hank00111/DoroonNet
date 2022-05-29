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
    /// CustomMarker.xaml 的互動邏輯
    /// </summary>
    /// 
    public class MarkerControl
    {
        public Popup Popup;
    }

    public partial class CustomMarker : UserControl
    {

        Label Label;
        GMapMarker Marker;
        MainWindow MainWindow;
        MarkerControl markerControl = new MarkerControl();
        //Popup Popupp;

        public CustomMarker(MainWindow window, GMapMarker marker, string NavPt, int NavCount)
        {
            InitializeComponent();
            
            Label = new Label();
            markerControl.Popup = new Popup();

            this.MainWindow = window;
            this.Marker = marker;

            this.Loaded += new RoutedEventHandler(CustomMarker_Loaded);
            this.Unloaded += new RoutedEventHandler(CustomMarker_Unloaded);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarker_SizeChanged);
            this.MouseEnter += new MouseEventHandler(CustomMarker_MouseEnter);
            this.MouseLeave += new MouseEventHandler(CustomMarker_MouseLeave);
            this.MouseMove += new MouseEventHandler(CustomMarker_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarker_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarker_MouseLeftButtonDown);
            Txt.Text = NavCount.ToString();

            markerControl.Popup.Placement = PlacementMode.Mouse;
            {
                Label.Background = Brushes.Black;
                Label.Foreground = Brushes.White;                
                Label.Padding = new Thickness(5);
                Label.FontSize = 12;
                Label.Content = NavPt;
            }
            markerControl.Popup.Child = Label;

        }
        private void CustomMarker_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        private void CustomMarker_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(CustomMarker_Loaded);
            this.Unloaded -= new RoutedEventHandler(CustomMarker_Unloaded);
            this.SizeChanged -= new SizeChangedEventHandler(CustomMarker_SizeChanged);
            this.MouseEnter -= new MouseEventHandler(CustomMarker_MouseEnter);
            this.MouseLeave -= new MouseEventHandler(CustomMarker_MouseLeave);
            this.MouseMove -= new MouseEventHandler(CustomMarker_MouseMove);
            this.MouseLeftButtonUp -= new MouseButtonEventHandler(CustomMarker_MouseLeftButtonUp);
            this.MouseLeftButtonDown -= new MouseButtonEventHandler(CustomMarker_MouseLeftButtonDown);

            Marker.Shape = null;
            icon.Source = null;
            icon = null;
            markerControl.Popup = null;
            Label = null;
        }

        private void CustomMarker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        void CustomMarker_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
            {
                Point p = e.GetPosition(MainWindow.MainMap);
                Marker.Position = MainWindow.MainMap.FromLocalToLatLng((int)(p.X), (int)(p.Y));
                Label.Content = MainWindow.MainMap.FromLocalToLatLng((int)(p.X), (int)(p.Y));
            }
        }
        private void CustomMarker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!IsMouseCaptured)
            {
                Mouse.Capture(this);
            }

        }

        private void CustomMarker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
            }
        }

        private void CustomMarker_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 10000;
            markerControl.Popup.IsOpen = false;
        }

        private void CustomMarker_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 10000;

            markerControl.Popup.IsOpen = true;
            //Popupp.IsOpen = true;
            //Console.WriteLine(VariableRes.ifDel);

        }


    }


}
