   M S B A M L   `   `   `  ����H  DSundasProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SundasProject.viewSundasProject.view  5      5    /SundasProject.view.QuantumComputerConfiguration   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      2local clr-namespace:SundasProject.view     5        N� Title$  Quantum Optics Simulator��5	      $	��510��6(   $
��1260��65    � DataContext 5      ��  �� RelativeSource �� *Self+.��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6   �� 5      $��3* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ PropertiesDataGrid��5      $��0��6/      AutoGenerateColumns$ False��6?      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Wavelength��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *
Wavelength+  5      $
 Path��6%    63   �� *Path+  5      $ Phase��6%    64   �� *Phase+  5      $ 	Intensity��6%    68   �� *	Intensity+  5      $ Duration��6%    67   �� *
Duration+  5      $ Center��6%    65   �� *Center+  5      $ 	Bandwidth��6%    68   �� *	Bandwidth+5      �� 5    
   $��1��6   	 �� Orientation$	 Vertical=�6%   $��10q�6<   .+�5#      �� $��0,0,0,5q�6   .+�5$      �� $��
Wavelength��6   �� 5%      -   5        
 ��Name$
 WavelengthTextBox��5%      $��0,0,0,10q�65   6r   �� 5&      $
��Path��6   �� 5'      -   5        $
 PathTextBox��5'      $��0,0,0,10q�6/   6l   �� 5(      $��Phase��6   �� 5)      -   5        $
 PhaseTextBox��5)      $��0,0,0,10q�60   6m   �� 5*      $��	Intensity��6   �� 5+      -   5        $
 IntensityTextBox��5+      $��0,0,0,10q�64   6q   �� 5,      $��Duration��6   �� 5-      -   5        $
 DurationTextBox��5-      $��0,0,0,10q�63   6p   �� 5.      $��Center��6   �� 5/      -   5        $
 CenterTextBox��5/      $��0,0,0,10q�61   6n   �� 50      $��	Bandwidth��6   �� 51      -   5        $
 BandwidthTextBox��51      $��0,0,0,10q�64   6q   52      �� 55      $	 
Horizontal=�6   $��Center=�62   .+�56      �� -	   5         ��Name$ CreateButton��56      $��Create��6/   $��5q�6@   $��60��6K   $	��100��6W   $��1q�6c   
��� � �$��16(�6�   6�   �� 57      -
   5        $ UpdateButton��57      $��Update��6/   $��5q�6@   $��60��6K   $	��100��6W   $��1q�6c   
����+��$��16(�6�   6�   �� 58      -   5        $ DeleteButton��58      $��Delete��6/   $��5q�6@   $��60��6K   $	��100��6W   $��1q�6c   
���  ��$��16(�6�   6�   �� 59      -   5        $ SimulateButton��59      $��Run ▶��61   $��5q�6A   $��60��6L   $��65��6X   $��1q�6c   
����  �$��12(�6�   6�   5:      5;      �� 5>   
   -   5         ��Name$ 	PlotImage��5?      $��2��5@      $��Uniform=�5A      5B      5C      