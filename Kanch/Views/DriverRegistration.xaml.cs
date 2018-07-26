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
            InitializeComponent();
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
