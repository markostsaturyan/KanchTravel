using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Kanch.DataModel;
using Kanch.ViewModel;

namespace Kanch.Views
{
    /// <summary>
    /// Interaction logic for Driver.xaml
    /// </summary>
    public partial class DriverRegistration : UserControl
    {

        public DriverRegistration()
        {
            this.DataContext = new DriverViewModel();
            InitializeComponent();
            
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
            var dataContext = DataContext as DriverViewModel;


            // setting to view model fields
            if (passwordBox == this.password)
                dataContext.Password = passwordBox.Password;
            else dataContext.ConfirmPassword = passwordBox.Password;
        }


        /* public Driver GetDriverInfo(User user)
         {
             var driver = new Driver()
             {

                 Car = new Car()
                 {
                     Brand = textBoxBrandOfCar.Text,
                     FuelType = textBoxFuelType.Text,
                     NumberOfSeats = Convert.ToInt32(textBoxNumberOfSeates.Text),
                     LicensePlate = textBlockLicensePlate.Text,
                     HasAirConditioner = checkBoxHasAirConditioner.IsChecked == true ? true : false,
                     HasKitchen = checkBoxHasAirConditioner.IsChecked == true ? true : false,
                     HasMicrophone = checkBoxHasMicrophone.IsChecked == true ? true : false,
                     HasToilet = checkBoxHasToilet.IsChecked == true ? true : false,
                     HasWiFi = checkBoxHasToilet.IsChecked == true ? true : false
                 }
             };

             driver.KnowledgeOfLanguages = "";
             if(checkBoxArmenian.IsChecked == true)
             {
                 driver.KnowledgeOfLanguages += "Armenian,";
             }
             if(checkBoxRussian.IsChecked == true)
             {
                 driver.KnowledgeOfLanguages += "Russian,";
             }
             if(checkBoxEnglish.IsChecked == true)
             {
                 driver.KnowledgeOfLanguages += "English,";
             }
             if(checkBoxGerman.IsChecked == true)
             {
                 driver.KnowledgeOfLanguages += "German,";
             }
             if(checkBoxFrench.IsChecked == true)
             {
                 driver.KnowledgeOfLanguages += "French,";
             }
             if(checkBoxItalian.IsChecked== true)
             {
                 driver.KnowledgeOfLanguages += "Italian";
             }

             return driver;
         }

     */

    }
}
