   M S B A M L   `   `   `  ����D  @DoroonNet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DoroonNet.ViewsDoroonNet.Views  5           DoroonNet.Views.FlightInfo   5      \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.27.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aaW SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089J 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:DoroonNet.Views     5      ��6:   Z� .��5      �� .��5	   
   �� -   5          ��Name$  
FlightList��5	         ListView#���   6�   ��6'   �� *CollectionListPartial+ �� Mode$ OneWay=� �� NotifyOnTargetUpdated$
 True�� �� NotifyOnSourceUpdated$
 True�� �� UpdateSourceTrigger$ PropertyChanged=���6�   Z� ��6�   Z� ��5
      �� 5       �� 
TargetType" ��6    0 ,MahApps.Styles.ListBoxItem.HamburgerMenuItem �� BasedOn# [ 6:   .�5      ��  �� Property � �� �� Value
 ���� 63   5      ��5      �� 5      .R�5      �� ���	#FF171717$��1q�65   	 �� Padding$	 10q�6I   ���	#7F747474
 �� CornerRadius$
 5,5,5,5��6m   $��53��6�   ._�5      � $��Stretch=�6   $��Center=�6=    � 	Resources 5      ��%   '��       �� 5   "   " ��6(   .�5   &   ��  � ��$ Google Sans)�6C   �� 5   &    � ��$ 17(�6B   5   #   5       � ColumnDefinitions 5      �� 5   "   $��22 �63   �� 5   "   $��25 �63   �� 5   "   $��58 �63   �� 5   "   $��75 �63   �� 5   "   $��48 �63   �� 5    "   $��40 �63   �� 5!   "   $��40 �63   �� 5"   "   $��58 �63   �� 5#   "   $��40 �63   �� 5$   "   $��35 �63   �� 5%   "   $��68 �63   �� 5&   "   $��20 �63   �� 5'   "   $��48 �63   �� 5(   "   $	��110 �63   �� 5)   "   $��40 �63   �� 5*   "   .+�5-      �� $��0��6(   $	��ID:��68   
�������$
��Left=�6V   $��Bottom=�6q   �� 5.      $��1��6(   $
��Left=�6�   $��Bottom=�6�   
���������68   �� *ID+$ OneWay=�$
 True��$
 True��$ PropertyChanged=�6�   �� 5/      $��2��6(   $��LinkID:��68   $
��Left=�6G   $��Bottom=�6b   
�������6}   �� 50      $��3��6(   $
��Left=�6�   $��Bottom=�6�   ���#56cc56��68   �� *
FlightID+$ OneWay=�$
 True��$
 True��$ PropertyChanged=�6�   �� 51      $��4��6(   $
��HDG:��68   $
��Left=�6D   $��Bottom=�6_   
�������6z   �� 52      $��5��6(   $
��Left=�6�   $��Bottom=�6�   ���#56cc56��68   �� *	FlightHDG+$ OneWay=�$
 True��$
 True��$ PropertyChanged=�6�   �� 53      $��6��6(   $
��SPD:��68   $
��Left=�6D   $��Bottom=�6_   
�������6z   �� 54      $��7��6(   $��Right=�6�   $��Bottom=�6�   ���#56cc56��68   �� *	FlightSPD+$ OneWay=�$
 True��$
 True��$ PropertyChanged=�6�   �� 55      $��8��6(   $	��m/s��68   $
��Left=�6D   $��Bottom=�6_   ���#56cc566z   �� 56      $��9��6(   $
��ALT:��68   $
��Left=�6D   $��Bottom=�6_   
�������6z   �� 57      $��10��6(   $��Right=�6�   $��Bottom=�6�   ���#56cc56��69   �� *	FlightALT+$ OneWay=�$
 True��$
 True��$ PropertyChanged=�6�   �� 58      $��11��6(   $��m��69   $
��Left=�6B   $��Bottom=�6]   ���#56cc566x   �� 59      $��12��6(   $�� 緯度:��69   $
��Left=�6E   $��Bottom=�6`   
�������6{   �� 5:      $��13��6(   $
��Left=�6�   $��Bottom=�6�   ���#56cc56��69   �� *	FlightLAT+$ OneWay=�$
 True��$
 True��$ PropertyChanged=�6�   �� 5;      $��14��6(   $��經度:��69   $
��Left=�6D   $��Bottom=�6_   
�������6z   �� 5<      $��15��6(   $
��Left=�6�   $��Bottom=�6�   ���#56cc56��69   �� *	FlightLNG+$ OneWay=�$
 True��$
 True��$ PropertyChanged=�6�   5=      5>      5?      5B      5D      5E      