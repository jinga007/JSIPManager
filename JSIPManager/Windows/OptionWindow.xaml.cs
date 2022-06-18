using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// OptionWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ObservableCollection < OPTION > _Options = new CXMLManagement().GetConfigXML();
            foreach (OPTION option in _Options)
            {
                switch(option.NAME)
                {
                    case MENU_ITEM.GENERAL:
                        {
                            chkLoadNic.IsChecked = option.LOAD_NIC == YES_NO.YES ? true : false;
                            chkLoadIPList.IsChecked = option.LOAD_IPLIST == YES_NO.YES ? true : false;
                            chkSaveIPList.IsChecked = option.SAVE_IPLIST == YES_NO.YES ? true : false;
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

        private void btnSaveSet_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<OPTION> _Options = new ObservableCollection<OPTION>();
            OPTION _option = new OPTION();

            _option.NAME = MENU_ITEM.GENERAL;
            _option.LOAD_NIC = chkLoadNic.IsChecked == true ? YES_NO.YES : YES_NO.NO;
            _option.LOAD_IPLIST = chkLoadIPList.IsChecked == true ? YES_NO.YES : YES_NO.NO;
            _option.SAVE_IPLIST = chkSaveIPList.IsChecked == true ? YES_NO.YES : YES_NO.NO;

            Properties.Settings.Default.LOAD_NIC = _option.LOAD_NIC;
            Properties.Settings.Default.LOAD_IPLIST = _option.LOAD_IPLIST;
            Properties.Settings.Default.SAVE_IPLIST = _option.SAVE_IPLIST;
            Properties.Settings.Default.Save();

            _Options.Add(_option);
            if(new CXMLManagement().SaveOptionXML(_Options))
            {
                if(System.Windows.MessageBox.Show(this, $" OK, The option saved.\n Could you close this form?", "Confrim", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }
            else
            {
                System.Windows.MessageBox.Show(this, $"NO, The option not saved");
            }
        }
    }
}
