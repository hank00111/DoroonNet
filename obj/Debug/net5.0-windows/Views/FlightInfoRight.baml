   M S B A M L   `   `   `  ����D  @DoroonNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DoroonNet.ViewsDoroonNet.Views  5      5clr-namespace:DoroonNet.CommandDoroonNet.Command  5      %    DoroonNet.Views.FlightInfoRight   5      \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.27.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aaW SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089J 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:DoroonNet.Views     5      0Dataclr-namespace:DoroonNet.Command     5      T PMahApps.Metro, Version=2.0.0.0, Culture=neutral, PublicKeyToken=51482d6f650b2b3f6mah,http://metro.mahapps.com/winfx/xaml/controls  6;   .��5	      � $��1��6     � ColumnDefinitions  5
   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $
��AUTO �6   �� 5      $
��AUTO �6   �� 5      �� 5      .+�5   
   �� -   5         ��Name$ ImageShowID��5      $��0��6�   $��14(�6�   $��Center=�6�   $��Google Sans)�6�   ��6)   �� *	ImageID+ �� Mode$ OneWay=� �� NotifyOnTargetUpdated$
 True�� �� NotifyOnSourceUpdated$
 True�� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5   
   -   5        $ 
DataShowID��5      $��14(�6�   $��1��6�   $��Center=�6�   $��Google Sans)�6�   ��6(   �� *DataID+$ OneWay=�$
 True��$
 True��$ Explicit=�6�   �� 5   
   -   5        $��2��5      $��	Send_Test��6   $��58��62   $��30��6@   $
��Bold"�6d   +  %MahApps.Metro.Controls.ControlsHelper   ContentCharacterCasing$ Normal=�6v   $y�Hidden=�6�   5      5      