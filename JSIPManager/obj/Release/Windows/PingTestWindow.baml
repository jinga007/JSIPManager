   M S B A M L   `   `   `  ���� F  BJSIPManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:JSIPManager.WindowsJSIPManager.Windows  (    "JSIPManager.Windows.PingTestWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     3local!clr-namespace:JSIPManager.Windows       � WindowStyle$  
ToolWindow=� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�$��나눔고딕)� � Title$ 	PING TEST��$	��300��$	��500��.���  � ColumnDefinitions �� $��5 ��� $��60 ��� $��13 ��� $��60 ��� $��13 ��� $��60 ��� $��13 ��� $��60 ��� $��5 ��� $��* ��� $��5 ��� $��50 ��� $��5 ��� $��50 ��� $��5 � � RowDefinitions �� $��5 ��� $��30 ��� $��5 ��� $��* ��� $��5 �.+��� -    ��Name$ txtbIPAdd01��$��1��$��1��$��Center=�$��Bottom=�$��20(� �� 	MaxLength$ 3���� -    ��Name$ 
lblIpAdd01��$��2��$��1��$��.��$
��Left=�$��Bottom=�$��20(��� -   $ txtbIPAdd02��$��3��$��1��$��Center=�$��Bottom=�$��20(�$ 3���� -   $ 
lblIpAdd02��$��4��$��1��$��.��$
��Left=�$��Bottom=�$��20(��� -   $ txtbIPAdd03��$��5��$��1��$��Center=�$��Bottom=�$��20(�$ 3���� -   $ 
lblIpAdd03��$��6��$��1��$��.��$
��Left=�$��Bottom=�$��20(��� -   $ txtbIPAdd04��$��7��$��1��$��Center=�$��Bottom=�$��20(�$ 3���� -   $ 
txtbDomain��$��1��$��1��$��8���� -	   	 ��Name$	 	cmbOption��$��9��$��1������ .R��� 
 �� Orientation$
 
Horizontal=�.+��� ���� *_T+�� -
    ��Name$ btnPingStart��$��Start��$��11��$��1���� -   $ 
btnPingEnd��$	��End��$��13��$��1���� -    ��Name$ 
rTxtStatus��$��1��$��3��$��13��" �� VerticalScrollBarVisibility$
 Auto=�
���   �
���2͚� � 	Resources  �� 
TargetType �� Property �� Value��%3   'J�       �� " J�.���  �@��Margin$ 0q�