   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   D @DoroonNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:DoroonNet.CommandDoroonNet.Command 5   	   1clr-namespace:DoroonNet.ViewsDoroonNet.Views 5	   	   c _GMap.NET.WindowsPresentation, Version=2.0.2.0, Culture=neutral, PublicKeyToken=b85b9027b614afefqPclr-namespace:GMap.NET.WindowsPresentation;assembly=GMap.NET.WindowsPresentationGMap.NET.WindowsPresentation 5
   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Y>clr-namespace:System.Windows.Interop;assembly=PresentationCoreSystem.Windows.Interop 5   	      DoroonNet.MainWindow   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.27.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aaR NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089J 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       R	 NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml  	   5   	   _
 [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3j+http://schemas.microsoft.com/xaml/behaviors 
 5   	   :d2http://schemas.microsoft.com/expression/blend/2008 
 5   	   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006 
 5   	   Emah;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro 
 5   	   /	behaviorsclr-namespace:DoroonNet.Command 
 5   	   &ucclr-namespace:DoroonNet.Views 
 5	   	   \gmapsPclr-namespace:GMap.NET.WindowsPresentation;assembly=GMap.NET.WindowsPresentation 
 5
   	   Fi>clr-namespace:System.Windows.Interop;assembly=PresentationCore 
 5   	   ^ ZCaliburn.Micro.Platform, Version=4.0.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f!cmhttp://caliburnmicro.com  5   	   -   5        $	��720��5   	   $
��1600��6   +   %MahApps.Metro.Controls.ControlsHelper    ContentCharacterCasing$  Normal=�6#   $	��800��6V   $	��480��6e    � Title$ Doroon��6u    � WindowStartupLocation$ CenterScreen=�5   	    � WindowState$ 	Maximized=�6/   .��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��1* �6   .+�5   
   � $��0��6   ���	#FF878080$��Stretch=�64   � 5   
   -   5         �Name$ 	main_Grid��5      $��1��6   $��Stretch=�6N    � ColumnDefinitions 5      �� 5      $��1* �6#   �� 5      $	��640 �6#   .+�5"      � $��0��6    5#      �� 5$      $
��1.9* �6$   �� 5%      .+�5.      .  (GMap.NET.WindowsPresentation.GMapControl  -   5          Name$ MainMap��5.   1   $��0��6$    K� ZIndex$ 0��6B   6�   � 51      $��0��6   $��30��6$   $	��320��61   $	��Top=�6=   $
��Left=�6U   $��4,2,0,0q�6p   .+�52      �� $
��FPS:��6    ���	#FF0EFF00$
��Bold"�6C   $��Google Sans)�6U   $��24(�6n   �� 53      .F�54      �� -   5        	 ��Name$	 FPS_text��54   (   ���	#FF0EFF00$
��Bold"�6�   $��Google Sans)�6�   $��24(�6   ��6:   �� *
FpsCount+
 �� Mode$
 OneWay=� �� NotifyOnTargetUpdated$
 True�� �� NotifyOnSourceUpdated$
 True�� �� UpdateSourceTrigger$ PropertyChanged=�6   55      56      57      �� 5:      $��1��6   ���	#FF21272F���	#FF7A7070$��0,0,1,0q�6U   ._�5;      � .+�5<      +   %MahApps.Metro.Controls.DateTimePicker  $��Right=�6.   $	��Top=�6J   $��38��6b   
�������+   %MahApps.Metro.Controls.TimePickerBase   Culture$ zh-TW��6�   $��13(�6�      IsClockVisible$ False��6�   ��6�   Z� ��6�   Z� * 
 $Microsoft.Xaml.Behaviors.Interaction   	Behaviors 5=      # DoroonNet.Command.DateTimeNow  5>   "   5@      �� 5B      ���	#FF21272F-   'MahApps.Metro.Controls.TabControlHelper   
Underlined$ TabPanel=�6<   2   ,MahApps.Metro.Controls.HeaderedControlHelper   HeaderFontSize$ 24Q�6g   .@�5C      �� $��資訊��6&    �� 
IsSelected$
 True��63      HeaderFontStretch$ Normal&�6E      HeaderFontWeight$ Normal"�6z      HeaderFontFamily$ Microsoft YaHei UI)�6�   .��5D   "   �  5E   &   �� 5F   *   $
��2.5* �6;   �� 5G   *   .+�5J   &    	  DoroonNet.Views.FlightInfo	  $��0��64   %
  DoroonNet.Views.FlightInfoRight
  5K   &   $��1��69   5L   #   5M      �� 5N      $��設定��6&   $ Normal&�62   $  Normal=�6g   $ Normal"�6�   $ Microsoft YaHei UI)�6�   5O      5P      5Q      5R      � 5V      $��1��6   $��Right=�6#   $��Stretch=�6?   $	��680��6\   $	��640��6l   ���	#FF21272F 5W      �� 5X      $	��360 �6$   �� 5Y      .+�5_      � -   5        $ 
Image_grid��5_      $��0��6+   ���	#FF212020$	��360��6O   $	��Top=�6\   .+�5`      �� -   5         ��Name$
 icon��5`      $��28��6*   $��28��66   ��5a      H� 5b       H� Drawing 5c   "   � 5d   &    � Brush
 ����� � Geometry� ����^ �hI ӄ	Y XsC 2`O XsC �jI �hI �8uC H^O 8uC �Y �jI �^ �X�� �� ��jI 8�tuC l-tuC ���jI @�$�n`O ��*�	Y ��*8�^ @�$ѐ� Ԯ� ��9@�$�`/��*����*4�$@�$Ә�*����*l-4�$8�Ȱ� �� �4�$�^ ���*�Y ��*H^O 4�$�hI ���XsC �/XsC �9�hI ѐ� d�� ���^ �hI 	6D   5f      5h      �� 5i      -   5         ��Name$ Image_border��5i      
�������$��1,1,1,1q�6H   ._�5j      �� -   5        $ Imgavdio��5j       $	��640��62   $	��360��6>   5k      5l      � 5n      -	   5        $ Chart��5n      $��1��6&   $��Stretch=�63   .+�5o        DoroonNet.Views.XYZChart  5p      5q      5s      5t      5u      