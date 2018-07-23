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

        public void Reset()
        {
            this.textBoxBrandOfCar.Text = "";
            this.textBoxFuelType.Text = "";
            this.textBoxNumberOfSeates.Text = "";
            this.checkBoxHasWiFi.IsChecked = false;
            this.checkBoxHasMicrophone.IsChecked = false;
            this.checkBoxHasAirConditioner.IsChecked = false;
            this.checkBoxHasKitchen.IsChecked = false;
            this.checkBoxHasToilet.IsChecked = false;
        }



    }
}
