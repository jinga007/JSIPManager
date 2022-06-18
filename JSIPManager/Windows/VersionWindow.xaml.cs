using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// VersionWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VersionWindow : Window
    {
        public VersionWindow()
        {
            InitializeComponent();
            txtBTitle.Text = $" Title : {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).Comments.ToString()}";
            txtBVersion.Text = $" Version : {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString()}";
            txtCompany.Text = $" {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).CompanyName.ToString()}";
            txtBCL.Text = $" {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).LegalCopyright.ToString()}";

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
