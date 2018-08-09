using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kanch.ProfileComponents.Views
{
    /// <summary>
    /// Interaction logic for LogOutView.xaml
    /// </summary>
    public partial class LogOutView : UserControl
    {
        public LogOutView()
        {
            InitializeComponent();
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            ConfigurationSettings.AppSettings["refreshToken"] = "";

            ConfigurationSettings.AppSettings["role"] = "";

            ConfigurationSettings.AppSettings["userId"] = "";

            ConfigurationSettings.AppSettings["userName"] = "";

            var login = new Login();

            login.Show();

            Application.Current.MainWindow.Close();

            Application.Current.MainWindow = login;
        }
    }
}
