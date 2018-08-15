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
using System.Windows.Shapes;

namespace Kanch
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            campingTrips.ContentTemplate = this.FindResource("InProgressCampingTrips") as DataTemplate;
        }

        public void AboutUsClick(object sender, EventArgs e)
        {
            var aboutUs = new AboutUs();
            var myWindow = Window.GetWindow(this);
            Application.Current.MainWindow = aboutUs;
            aboutUs.Show();
            myWindow.Close();
        }

        public void LoginClick(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["refreshToken"] == "")
            {
                var login = new Login();
                Application.Current.MainWindow = login;
                login.Show();
                this.Close();
            }
            else
            {
                var profile = new Profile();
                Application.Current.MainWindow = profile;
                profile.Show();
                this.Close();
            }
        }

        public void RegisterClick(object sender, EventArgs e)
        {
            var register = new MainWindow();
            Application.Current.MainWindow = register;
            register.Show();
            this.Close();
        }
    }
}
