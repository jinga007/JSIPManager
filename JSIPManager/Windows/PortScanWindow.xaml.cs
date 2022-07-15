using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

namespace JSIPManager.Windows
{
    /// <summary>
    /// PortScanWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PortScanWindow : Window
    {
        System.Threading.Thread td = null;
        bool bStop = false;
        public PortScanWindow()
        {
            InitializeComponent();
        }


        public void DoPortScan(object data)
        {
            string[] IPInfors = (data as string).Split('=');
            if (IPInfors.Length > 0 && (data as string).Contains("="))
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    txtPortScanStatus.Document.Blocks.Add(new Paragraph(new Run($"Port scan start..... ")));

                }));

                string[] IPAddresses = IPInfors[0].Split(',');
                string[] Ports = IPInfors[1].Split(',');
                TcpClient Scan = new TcpClient();
                foreach (var ip in IPAddresses)
                {
                    foreach(var port in Ports)
                    {
                        if (bStop)
                            break;

                        int iPort = -1;
                        int.TryParse(port, out iPort);

                        try
                        {
                            Scan.Connect(ip, iPort);
                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                            {
                                txtPortScanStatus.Document.Blocks.Add(new Paragraph(new Run($"IP ADDRESS [{ip.ToString()}], Port [{iPort.ToString()}] is opened")));

                            }));
                        }
                        catch
                        {
                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                            {
                                txtPortScanStatus.Document.Blocks.Add(new Paragraph(new Run($"IP ADDRESS [{ip.ToString()}], Port [{iPort.ToString()}] is not opened")));

                            }));

                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    txtPortScanStatus.Document.Blocks.Add(new Paragraph(new Run($"Port scan end..... ")));

                }));
            }
        }

        private void btnPortScanStart_Click(object sender, RoutedEventArgs e)
        {

            string strIpInfors = $"{txtIPAddress.Text}={txtPort.Text}";
            bStop = false;
            txtPortScanStatus.Document.Blocks.Clear();
            td = new System.Threading.Thread(DoPortScan);
            td.Start(strIpInfors);
        }

        private void btnPortScanStop_Click(object sender, RoutedEventArgs e)
        {
            bStop = true;
        }
    }
}
