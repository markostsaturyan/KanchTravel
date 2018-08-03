using System;
using System.Collections.Generic;
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

namespace Kanch.Views
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class ChooseRegistrationType : UserControl
    {
        
        public ChooseRegistrationType()
        {
            InitializeComponent();
        }

        public void UserClick(object sender,EventArgs e)
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("RegistrationPresent") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("UserRegistration") as DataTemplate;
        }

        public void DriverClick(object sender, EventArgs e)
        {
            var window = Application.Current.MainWindow;
            var presenter = window.FindName("RegistrationPresent") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("DriverRegistration") as DataTemplate;
        }

        public void GuideClick(object sender, EventArgs e)
        {
            var window = Application.Current.MainWindow;
            var presenter = window.FindName("RegistrationPresent") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("GuideRegistration") as DataTemplate;
        }

        public void PhotographerClick(object sender, EventArgs e)
        {
            var window = Application.Current.MainWindow;
            var presenter = window.FindName("RegistrationPresent") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("PhotographerRegistration") as DataTemplate;
        }

        public void LoginClick(object sender, EventArgs e)
        {
            var login = new Login();
            var myWindow = Window.GetWindow(this);
            login.Show();
            myWindow.Close();
        }

        public void HomeClick(object sender, EventArgs e)
        {
            var home = new Home();
            var myWindow = Window.GetWindow(this);
            //home.Show();
            myWindow.Close();
        }
    }
}
