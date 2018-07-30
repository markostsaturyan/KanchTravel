using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {


        DriverRegistration driverRegistration;
        GuideRegistration guideRegistration;
        PhotographerRegistration photographerRegistration;
        RegistrationViewModel registrationViewModel;

        public Registration()
        {
            this.registrationViewModel = new RegistrationViewModel();
            this.DataContext = this.registrationViewModel;
            InitializeComponent();
            /*driverRegistration = new DriverRegistration();
            guideRegistration = new GuideRegistration();
            photographerRegistration = new PhotographerRegistration();**/
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();

            login.Show();
        }




        private void Submitbutto(object sender, RoutedEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var dataContext = (DataContext as RegistrationViewModel);
            if (comboBox.SelectedItem == this.driver)
            {
                dataContext.PhotographerViewModel.PhotographerVisible = Visibility.Collapsed;
                dataContext.GuideViewModel.GuideVisible = Visibility.Collapsed;
                dataContext.DriverViewModel.DriverVisible = Visibility.Visible;
            }
            else if (comboBox.SelectedItem == this.guide)
            {
                dataContext.DriverViewModel.DriverVisible = Visibility.Collapsed;
                dataContext.PhotographerViewModel.PhotographerVisible = Visibility.Collapsed;
                dataContext.GuideViewModel.GuideVisible = Visibility.Visible;

            }
            else if (comboBox.SelectedItem == this.photographer)
            {
                dataContext.GuideViewModel.GuideVisible = Visibility.Collapsed;
                dataContext.DriverViewModel.DriverVisible = Visibility.Collapsed;
                dataContext.PhotographerViewModel.PhotographerVisible = Visibility.Visible;
            }
            else
            {
                dataContext.PhotographerViewModel.PhotographerVisible = Visibility.Collapsed;
                dataContext.DriverViewModel.DriverVisible = Visibility.Collapsed;
                dataContext.GuideViewModel.GuideVisible = Visibility.Collapsed;
            }

        }

       
    }
}
