using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JSIPManager.Windows;

namespace JSIPManager
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Func
        private void SetStaticIP()
        {
            bool bRest = false;
            try
            {
                bRest = new CNetworkAdptManager().SetStaticIP(
                        (this.cmbNWDescs.SelectedItem as NIC_ITEM).NIC_NAME.ToString(),
                        (dgNWList.SelectedItem as CNICIP_INFOR).IPADDRESS,
                        (dgNWList.SelectedItem as CNICIP_INFOR).SUBNET_MASK,
                        (dgNWList.SelectedItem as CNICIP_INFOR).GATEWAY,
                        (dgNWList.SelectedItem as CNICIP_INFOR).DEFAULT_DNS,
                        (dgNWList.SelectedItem as CNICIP_INFOR).SUB_DNS
                        );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (bRest)
                {
                    System.Windows.MessageBox.Show(this, $"OK, The IP Changed");
                    AutoIP.IsChecked = false;
                    AutoIP.IsEnabled = true;

                    LoadNicInfor();
                    ShowIPInfor();
                }
                else
                {
                    System.Windows.MessageBox.Show(this, $"NO, The IP not Changed");
                }
            }
        }

        private void SetDynamicIP()
        {
            bool bRest = false;
            try
            {
                bRest = new CNetworkAdptManager().setDynamicIP((this.cmbNWDescs.SelectedItem as NIC_ITEM).NIC_NAME.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (bRest)
                {
                    System.Windows.MessageBox.Show(this, $"OK, The IP Changed");
                    LoadNicInfor();
                    ShowIPInfor();
                    AutoIP.IsEnabled=false;
                }
                else
                {
                    System.Windows.MessageBox.Show(this, $"NO, The IP not Changed");
                }
            }
        }

        private void LoadIPListData(bool _option = false)
        {
            try
            {
                if(!_option)
                    if (Properties.Settings.Default.LOAD_IPLIST != YES_NO.YES)
                        return;

                dgNWList.DataContext = new CXMLManagement().GetIPHistXML("");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void SaveIPListData(bool _option = false,bool _closed = false)
        {
            bool bRest = false;
            try
            {
                if(!_option)
                    if (Properties.Settings.Default.SAVE_IPLIST != YES_NO.YES)
                        return;


                bRest = new CXMLManagement().SaveIPHistXML(dgNWList.DataContext as ObservableCollection<CNICIP_INFOR>);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (!_closed)
                {
                    if (bRest)
                    {
                        System.Windows.MessageBox.Show(this, $"OK, The History saved");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(this, $"NO, The History not saved");
                    }
                }
            }
        }
        
        private void LoadNicInfor(bool _option = false)
        {
            try
            {
                if(!_option)
                    if (Properties.Settings.Default.LOAD_NIC != YES_NO.YES)
                        return;

                ObservableCollection<NIC_ITEM>  _dicAll = new CNetworkAdptManager().GetNICDescs();
                cmbNWDescs.ItemsSource = _dicAll;
                foreach (var item in _dicAll.Where(x => Regex.IsMatch(x.NIC_NAME.ToString().ToUpper(), @"WIFI|WIRELESS|WI-FI")).ToDictionary(r => r.IDX, r => r.NIC_NAME).Keys)
                    cmbNWDescs.SelectedIndex = item;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void InitialControls()
        {
            try
            {
                if (new CNetworkAdptManager().GetDhcp((this.cmbNWDescs.SelectedItem as NIC_ITEM).NIC_NAME.ToString()))
                {
                    AutoIP.IsChecked = true;
                    AutoIP.IsEnabled = false;
                }
                else
                {
                    AutoIP.IsChecked = false;
                    AutoIP.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
            }
        }

        private void ShowIPInfor()
        {
            string IPv4_IP = string.Empty;
            string IPv4_DNS = string.Empty;
            string IPv4Gateway = string.Empty;
            string IPv4SubnetMask = string.Empty;
            try
            {
                if (cmbNWDescs.SelectedItem is null) 
                    return;

                string strNicName = (cmbNWDescs.SelectedItem as NIC_ITEM).NIC_NAME.ToString();
                IPv4_IP = new CNetworkAdptManager().NIC_DETIL_INFORS.Where(x => x.NIC_NAME == strNicName).ToList()[0].UNICASTADDRESSES.Last();
                IPv4_DNS = new CNetworkAdptManager().NIC_DETIL_INFORS.Where(x => x.NIC_NAME == strNicName).ToList()[0].DNSADDRESSES.First();
                IPv4Gateway = new CNetworkAdptManager().NIC_DETIL_INFORS.Where(x => x.NIC_NAME == strNicName).ToList()[0].GATEWAYADDRESSES.Last();
                IPv4SubnetMask = new CNetworkAdptManager().NIC_DETIL_INFORS.Where(x => x.NIC_NAME == strNicName).ToList()[0].SUBNETMASKADDRESSES.Last();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                lblNWCurrentInfor.Content = $"IP [{IPv4_IP}], SUBNETMASK [{IPv4SubnetMask}], GATEWAY [{IPv4Gateway}], DNS [{ IPv4_DNS}]";
            }
        }

        private void Initial()
        {
            try
            {
                ObservableCollection<OPTION> _Options = new CXMLManagement().GetConfigXML();
                foreach (OPTION option in _Options)
                {
                    switch (option.NAME)
                    {
                        case MENU_ITEM.GENERAL:
                            {
                                Properties.Settings.Default.LOAD_NIC = option.LOAD_NIC;
                                Properties.Settings.Default.LOAD_IPLIST = option.LOAD_IPLIST;
                                Properties.Settings.Default.SAVE_IPLIST = option.SAVE_IPLIST;
                                Properties.Settings.Default.Save();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {

            }
        }
        #endregion Func

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Initial();
            LoadNicInfor();
            LoadIPListData();
            InitialControls();
        }

       

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            SaveIPListData(false,true);
        }

        private void cmbNWDescs_SelectionChanged(object sender, SelectionChangedEventArgs e) => ShowIPInfor();

        private void DGMenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch((sender as System.Windows.Controls.HeaderedItemsControl).Header)
            {
                case MENU_ITEM.PING:
                    {
                        System.Windows.MessageBox.Show(this, new CNetworkAdptManager().Ping((dgNWList.SelectedValue as CNICIP_INFOR).IPADDRESS));
                    }
                    break;
                case MENU_ITEM.SET_STATIC:
                    {
                        SetStaticIP();
                    }
                    break;
                default:
                    break;
            }
        }

        private void MainMenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.HeaderedItemsControl).Header)
            {
                case MENU_ITEM.LOAD_NIC:
                    {
                        LoadNicInfor(true);
                    }
                    break;
                case MENU_ITEM.PING_EX:
                    {
                        PingTestWindow pingWin = new PingTestWindow(new CNetworkAdptManager().NIC_DETIL_INFORS.Where(x => x.NIC_NAME == (cmbNWDescs.SelectedItem as NIC_ITEM).NIC_NAME.ToString()).ToList()[0].UNICASTADDRESSES.Last());
                        pingWin.Show();
                    }
                    break;
                case MENU_ITEM.LOAD_IPS:
                    {
                        LoadIPListData(true);
                    }
                    break;
                case MENU_ITEM.SAVE_IPS:
                    {
                        SaveIPListData(true);
                    }
                    break;
                case MENU_ITEM.MINIMUM:
                    {
                        this.WindowState = WindowState.Minimized;
                    }
                    break;
                case MENU_ITEM.EXIT:
                    {
                        this.Close();
                    }
                    break;
                case MENU_ITEM.SET_STATIC:
                    {
                        if (System.Windows.MessageBox.Show(this,"Could you change to Static IP ?", "Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            SetStaticIP();
                    }
                    break;
                case MENU_ITEM.OPTION_EX:
                    {
                        OptionWindow optionWin = new OptionWindow();
                        optionWin.ShowDialog();
                    }
                    break;
                case MENU_ITEM.PORT_SCAN_EX:
                    {
                        PortScanWindow portScanWin = new PortScanWindow();
                        portScanWin.Show();
                    }
                    break;
                case MENU_ITEM.VERSION:
                    {
                        VersionWindow versionWin = new VersionWindow();
                        versionWin.ShowDialog();
                    }
                    break;
                case MENU_ITEM.HELP:
                    {
                        System.Windows.MessageBox.Show(this, "준비중...");
                    }
                    break;
                default:
                    break;
            }
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            switch ((sender as System.Windows.Controls.HeaderedItemsControl).Header)
            {
                case MENU_ITEM.SET_DYNAMIC:
                    {
                        if (!new CNetworkAdptManager().GetDhcp((this.cmbNWDescs.SelectedItem as NIC_ITEM).NIC_NAME.ToString()))
                        {
                            if (System.Windows.MessageBox.Show("Could you change to Dynamic IP ?", "Confirm", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {
                                SetDynamicIP();
                            }
                            else
                            {
                                (sender as MenuItem).IsChecked = false;
                                (sender as MenuItem).IsEnabled = true;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
