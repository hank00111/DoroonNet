   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.27.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aaW SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType �� Property �� Value �� 
TargetType � ColumnDefinitions � RowDefinitions J� SnapsToDevicePixels �� Columns �� Path	 �� RelativeSource!
 �� ColumnHeaderContainerStyle �� ColumnHeaderTemplate# �� ColumnHeaderTemplateSelector �� AllowsColumnReorder �� ColumnHeaderContextMenu �� ColumnHeaderToolTip-    'System.Windows.Input.KeyboardNavigation    DirectionalNavigation �� CanContentScroll �� CanHorizontallyScroll �� CanVerticallyScroll �� Orientation �� ScrollableWidth � Maximum �� ViewportWidth �� ViewportSize �� HorizontalOffset � Value, �� %ComputedHorizontalScrollBarVisibility �� ScrollableHeight �� ViewportHeight �� VerticalOffset* �� #ComputedVerticalScrollBarVisibility   GridViewColumnHeaderGripper  �� 
StartPoint! �� EndPoint  BorderLightColor" � Color# � Offset  BorderDarkColor$ �� Padding'   !System.Windows.VisualStateManager%   VisualStateGroups%   System.Windows.VisualStateGroup&   States    System.Windows.VisualState'   
Storyboard( �� TargetProperty) �� 
TargetName8  2System.Windows.Media.Animation.EasingColorKeyFrame* �� KeyTime  ControlMouseOverColor+ �� Value, ��Name  ControlLightColor  ControlMediumColor- ��Name. n�Name/ �� Triggers0 P� Property1 P� Value2 � ActualWidth3 � ActualHeight4 �� Color  BorderMediumColor5 �� Triggers6 �� 
TargetName  DisabledBorderLightColor  SelectedBackgroundColor 	 SelectedUnfocusedColor�� 5      J 9http://schemas.microsoft.com/winfx/2006/xaml/presentation        :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %_  (��       5      *'%GridView.GridViewScrollViewerStyleKey+)&	  1    '�� �    7  7   '�� X    7  7  '�� �    7  7 	 �� 6   "  ��5      .�5   
   ��  � �� 5      �� 5      " ��6"   .R�5	      � #��z��6    5
      �� 5      $��* �6/   �� 5      $
��Auto �6/    5      �� 5      $��* �6,   �� 5      $
��Auto �6,   .+�5      ]� #��z��6$   .+�5      �� $	��Top=�6+   $��Hidden=�5      $��Hidden=�5      $|�false��5      .��5   "   �� $��2,0,2,0q�6=   # z 5-   +    5   +   �� $" TemplatedParent.View.Columns�	 �� *TemplatedParent+
 5   +   �� $5 /TemplatedParent.View.ColumnHeaderContainerStyle�	 �� *TemplatedParent+ 5   +   �� $/ )TemplatedParent.View.ColumnHeaderTemplate�	 �� *TemplatedParent+ 5!   +   �� $7 1TemplatedParent.View.ColumnHeaderTemplateSelector�	 �� *TemplatedParent+ 5$   +   �� $. (TemplatedParent.View.AllowsColumnReorder�	 �� *TemplatedParent+ 5'   +   �� $2 ,TemplatedParent.View.ColumnHeaderContextMenu�	 �� *TemplatedParent+ 5*   +   �� $. (TemplatedParent.View.ColumnHeaderToolTip�	 �� *TemplatedParent+5-   +   5/      �� 51      $!��PART_ScrollContentPresenter��65   $ Local=�52   %   $
 True��53   %   $ False��54   %   $ False��55   %   56      �� 58      $��PART_HorizontalScrollBar��6$   $ 
Horizontal=�59      $��1��5:      # z 5;      # z 5<      # z 5=      #y�z 5>      �� 5@      $��PART_VerticalScrollBar��6$   $��1��5A      # z 5B      # z 5C      # z 5D      #y�z 5E      5G      5H      5J      5K      �� 5M      "  n�5N      .�5O   
   ��  � ��$ 18��5P      �� 5Q   
    � �� 5R      �� 5S      $	  0,0&�6&   $	! 0,1&�5T      ��5V      � � 5W      #" �  6+   $	# 0.0Q�5X      � 5Y      #" �  6+   $	# 1.0Q�5Z      5[      5]      5_      �� 5`   
    � �� 5a      �� 5b      " n�6"   .R�5c      �� #$ z��6   
������ ._�5e      �� $��Center=�6$   $��1��5f      #��z��5g      5h      5i      5k      �� 5l   
    � �� 5m      �� 5n      $! 0.5,1&�6&   $  0.5,0&�5o      .��5p      � $" Black��6#   $# 0Q�5q      � 5r      $" White��6#   $# 1Q�5s      5t      5v      5w      �� 5y      "  ��5z      .�5{   
   ��  � ��$ Center=�5|      �� 5}   
    � ��$ Center=�5~      �� 5   
    � ��# � ��5�      �� 5�   
    � �� 5�      �� 5�      " ��6"   .R�5�      � % 5�        5�      CommonStates��.& 5�   "     
Normal��6.     5�   "   	MouseOver��.' 5�   &   �� .��5�   *   �� $c( ](Panel.Background).                     (GradientBrush.GradientStops)[1].(GradientStop.Color)�6G   $) HeaderBorder��5�   1   .g�5�   .     $* 0��6B   8+   5�   *   5�   +   5�   '   5�   #     5�   "   Pressed��6.     5�   "   Disabled��6.   5�      .+�5�      �� $, HeaderBorder��6!   $��0,1,0,1q�5�      $$ 2,0,2,0q�5�      ��5�      �� 5�   "   $	  0,0&�66   $	! 0,1&�5�   $   ��5�   *   � � 5�   .   #" �  6;   $	# 0.0Q�5�   #   � 5�   .   #" �  6;   $	# 1.0Q�5�   #   5�   +   5�   #   ��5�      �� 5�   "   $	  0,0&�66   $	! 0,1&�5�   $   ��5�   *   � � 5�   .   #" �  6;   $	# 0.0Q�5�   #   � 5�   .   #" �  6;   $	# 1.0Q�5�   #   5�   +   5�   #   ._�5�      �� $- HeaderContent��6/   $��0,0,0,1q�5�      $
��True��5�      #��z��5�      #��z��5�      # z 5�      5�      n� 5�      $. PART_HeaderGripper��6    $��Right=�5�      $��0,0,-9,0q�5�      8�� 5�      5�      5�      5�      / 5�   
   P� 5�      0 �@��Role$1 Floating=�5�      . �5�      ��  �@J�Opacity$	 0.7Q�5�      �� 5�       � �� 5�      �� 5�      " ��6*   .R�5�      �� $��PART_FloatingHeaderCanvas��6%   .+�5�   "   �� ���	#60000000#��z2 5�      #��z3 5�      5�      5�      5�      5�      P� 5�      0 �@��Role$1 Padding=�5�      . �5�      ��  � �� 5�      �� 5�      " ��6*   .R�5�      �� $��HeaderBorder��6%   $��0,1,0,1q�5�      ��5�   "   �� 5�   &   #4 �  66   ��5�   "   �� 5�   &   $	  0,0&�6:   $	! 0,1&�5�   &   ��5�   .   � � 5�   2   #" �  6?   $	# 0.0Q�5�   %   � 5�   2   #" �  6?   $	# 1.0Q�5�   %   5�   /   5�   '   5�      5�      5�      5�      5�      �� 5�      "  ��5�      .�5�   
   ��  �@J�SnapsToDevicePixels$
 true��5�      �� 5�   
    �@�OverridesDefaultStyle$
 true��5�      �� 5�   
    � ��$
 Auto=�5�      �� 5�   
    � ��$
 Auto=�5�      �� 5�   
    � ��$
 true��5      �� 5  
    � ��$ Center=�5     �� 5  
    � �� 5     �� 5     " ��6"   .R�5     �� $��Border��6   $��1q�5     ��5     �� 5	     84   6.   ��5     �� 5     84  6.   ._�5     �� #��� $�6'   .��5     �� 5     5     5 5     P� 5     0 �@��
IsGrouping$
1 true��5     . �5     ��  � ��$ false��5     5     P� 5     0 � {�$1 false��5     . �5     �� $6 Border��6%    � �� 5  "   �� 5  &   #4 �  66   5      5!     5#     5%     5&     �� 5(     "  ��5)     .�5*  
   ��  �@J�SnapsToDevicePixels$
 true��5+     �� 5,  
    �@�OverridesDefaultStyle$
 true��5-     �� 5.  
    � �� 5/     �� 50     " ��6"   .R�51     �� $, Border��6   $$ 0q�52     $
 true��53     
������ % 55       56     CommonStates��.& 57  "     
Normal��6.     58  "   	MouseOver��6.     59  "   Disabled��6.   5:       5;     SelectionStates��.& 5<  "     
Unselected��6.     5=  "   Selected��.' 5>  &   �� .��5?  *   �� $) Border��6G   $E( ?(Panel.Background).                     (SolidColorBrush.Color)�5@  1   .g�5B  .     $* 0��6B   8+   5C  *   5D  +   5E  '   5F  #     5G  "   SelectedUnfocused��.' 5H  &   �� .��5I  *   �� $) Border��6G   $E( ?(Panel.Background).                     (SolidColorBrush.Color)�5J  1   .g�5L  .     $* 0��6B   8+  5M  *   5N  +   5O  '   5P  #   5Q     ._�5S     �� #��z��6/   5T     5U     5W     5X     5Y     