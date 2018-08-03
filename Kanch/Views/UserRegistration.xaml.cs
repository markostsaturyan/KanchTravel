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
using Kanch.ViewModel;

namespace Kanch.Views
{
    /// <summary>
    /// Interaction logic for UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : UserControl
    {
        public UserRegistration()
        {
            this.DataContext = new UserViewModel();
            InitializeComponent();
        }


        public void LoginClick(object sender, EventArgs e)
        {
            var login = new Login();
            var myWindow = Window.GetWindow(this);
            Application.Current.MainWindow = login;
            login.Show();
            myWindow.Close();
        }

        public void HomeClick(object sender, EventArgs e)
        {
            var home = new Home();
            var myWindow = Window.GetWindow(this);
            Application.Current.MainWindow = home;
            home.Show();
            myWindow.Close();
        }

        public void RegistrationClick(object sender, EventArgs e)
        {
            var window = Application.Current.MainWindow;
            var presenter = window.FindName("RegistrationPresent") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("MainRegistrationPage") as DataTemplate;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)

        {

            // converting sender to Password box

            var passwordBox = (PasswordBox)sender;
            var dataContext = DataContext as UserViewModel;


            // setting to view model fields

            if (passwordBox == this.password)

                dataContext.Password = passwordBox.Password;

            else dataContext.ConfirmPassword = passwordBox.Password;

        }




    }
}
