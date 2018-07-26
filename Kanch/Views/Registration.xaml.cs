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

        public Registration()
        {
            this.DataContext = new RegistrationViewModel();
            InitializeComponent();
            driverRegistration = new DriverRegistration();
            guideRegistration = new GuideRegistration();
            photographerRegistration = new PhotographerRegistration();
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
        }



        private void button2_Click(object sender, RoutedEventArgs e)

        {

            Reset();

        }

        /*private void HandleCheck(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Name == "checkBoxHasWiFi")
            {
                text1.Text = "2 state CheckBox is checked.";
            }
            else
            {
                text2.Text = "3 state CheckBox is checked.";
            }
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Name == "checkBoxHasMicrophone")
            {
                text1.Text = "2 state CheckBox is unchecked.";
            }
            else
            {
                text2.Text = "3 state CheckBox is unchecked.";
            }
        }*/



        public void Reset()

        {

            this.textBoxFirstName.Text = "";

            this.textBoxLastName.Text = "";

            this.textBoxUserName.Text = "";

            this.textBoxDateOfBirth.Text = "";

            this.textBoxPhoneNumber.Text = "";

            this.textBoxEmail.Text = "";

            this.male.IsChecked = false;

            this.female.IsChecked = false;

            this.passwordBox1.Password = "";

            this.passwordBoxConfirm.Password = "";

            if (this.driverRegistration != null)
            {
                driverRegistration.Reset();
            }
            if (this.guideRegistration != null)
            {
                guideRegistration.Reset();
            }
            if (this.photographerRegistration != null)
            {
                photographerRegistration.Reset();
            }

        }

        private void Submit_Click(object sender, RoutedEventArgs e)

        {

            if (textBoxEmail.Text.Length == 0)

            {

                errormessage.Text = "Enter an email.";

                textBoxEmail.Focus();

            }

            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))

            {

                errormessage.Text = "Enter a valid email.";

                textBoxEmail.Select(0, textBoxEmail.Text.Length);

                textBoxEmail.Focus();

            }

            else

            {

                string firstname = textBoxFirstName.Text;

                string lastname = textBoxLastName.Text;

                string email = textBoxEmail.Text;

                string password = passwordBox1.Password;

                if (passwordBox1.Password.Length == 0)

                {

                    errormessage.Text = "Enter password.";

                    passwordBox1.Focus();

                }

                else if (passwordBoxConfirm.Password.Length == 0)

                {

                    errormessage.Text = "Enter Confirm password.";

                    passwordBoxConfirm.Focus();

                }

                else if (passwordBox1.Password != passwordBoxConfirm.Password)

                {

                    errormessage.Text = "Confirm password must be same as password.";

                    passwordBoxConfirm.Focus();

                }

                else

                {

                    errormessage.Text = "";

                    SqlConnection con = new SqlConnection("Data Source=TESTPURU;Initial Catalog=Data;User ID=sa;Password=wintellect");

                    con.Open();

                    SqlCommand cmd = new SqlCommand("Insert into Registration (FirstName,LastName,Email,Password,Address) values('" + firstname + "','" + lastname + "')", con);

                    cmd.CommandType = CommandType.Text;

                    cmd.ExecuteNonQuery();

                    con.Close();

                    errormessage.Text = "You have Registered successfully.";

                    Reset();

                }

            }
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
