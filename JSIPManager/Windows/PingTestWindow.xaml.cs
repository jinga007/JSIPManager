using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace JSIPManager.Windows
{
    /// <summary>
    /// PingTestWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PingTestWindow : Window
    {
        //Thread PingThread = null;
        bool bStop = false;
        private void InitialControls()
        {
            try
            {
                this.txtbIPAdd01.Visibility = Visibility.Visible;
                this.txtbIPAdd02.Visibility = Visibility.Visible;
                this.txtbIPAdd03.Visibility = Visibility.Visible;
                this.txtbIPAdd04.Visibility = Visibility.Visible;
                this.lblIpAdd01.Visibility = Visibility.Visible;
                this.lblIpAdd02.Visibility = Visibility.Visible;
                this.lblIpAdd03.Visibility = Visibility.Visible;

                this.txtbDomain.Visibility = Visibility.Collapsed;


                ObservableCollection<PING_OPTION> _POptions = new ObservableCollection<PING_OPTION>();
                _POptions.Add(new PING_OPTION());
                cmbOption.ItemsSource = _POptions;
                cmbOption.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
            }
        }

        private void PingProc(object obj)
        {
            while(true)
            {
                if (bStop)
                    break;
                
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    rTxtStatus.Document.Blocks.Add(new Paragraph(new Run(new CNetworkAdptManager().Ping(obj.ToString(), 1))));

                }));
                Thread.Sleep(1000);
            }
        }

        public PingTestWindow()
        {
            InitializeComponent();
        }

        public PingTestWindow(string strIPAddress)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(strIPAddress) && strIPAddress.Contains('.'))
            {
                this.txtbIPAdd01.Text = strIPAddress.Split('.')[0].ToString().Trim();
                this.txtbIPAdd02.Text = strIPAddress.Split('.')[1].ToString().Trim();
                this.txtbIPAdd03.Text = strIPAddress.Split('.')[2].ToString().Trim();
                this.txtbIPAdd04.Text = strIPAddress.Split('.')[3].ToString().Trim();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            InitialControls();
        }

        private void txtbIPAdd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnPingStart_Click(object sender, RoutedEventArgs e)
        {
            bStop = false;
            Thread PingThread = new Thread(PingProc);
            rTxtStatus.Document.Blocks.Clear();
            PingThread.Start($"{txtbIPAdd01.Text.Trim()}.{txtbIPAdd02.Text.Trim()}.{txtbIPAdd03.Text.Trim()}.{txtbIPAdd04.Text.Trim()}");
        }

        private void btnPingEnd_Click(object sender, RoutedEventArgs e)
        {
            bStop = true;
        }

        private void txtbIPAdd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && Keyboard.Modifiers  == ModifierKeys.Control)
            {
                string strCopiedText = Clipboard.GetText();
                if(!string.IsNullOrEmpty(strCopiedText) && strCopiedText.Contains('.'))
                {
                    this.txtbIPAdd01.Text = strCopiedText.Split('.')[0].ToString().Trim();
                    this.txtbIPAdd02.Text = strCopiedText.Split('.')[1].ToString().Trim();
                    this.txtbIPAdd03.Text = strCopiedText.Split('.')[2].ToString().Trim();
                    this.txtbIPAdd04.Text = strCopiedText.Split('.')[3].ToString().Trim();
                }

            }
        }
    }
}
