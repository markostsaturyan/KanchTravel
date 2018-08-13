using Kanch.ProfileComponents;
using Kanch.ProfileComponents.ViewModels;
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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public Profile()
        {

            InitializeComponent();
            var role = ConfigurationSettings.AppSettings["role"];
            switch (role)
            {
                case "Admin":
                    {
                        userMainInfoPresenter.ContentTemplate = this.FindResource("AdminMain") as DataTemplate;
                        mainPage.ContentTemplate = this.FindResource("CampingTripRegistrationForAdmin") as DataTemplate;
                        break;
                    }
                case "Driver":
                    {
                        userMainInfoPresenter.ContentTemplate = this.FindResource("DriverMain") as DataTemplate;
                        mainPage.ContentTemplate = this.FindResource("CampingTripsRegistration") as DataTemplate;
                        break;
                    }
                case "Guide":
                    {
                        userMainInfoPresenter.ContentTemplate = this.FindResource("GuideMain") as DataTemplate;
                        mainPage.ContentTemplate = this.FindResource("CampingTripsRegistration") as DataTemplate;
                        break;
                    }
                case "Photographer":
                    {
                        userMainInfoPresenter.ContentTemplate = this.FindResource("PhotographerMain") as DataTemplate;
                        mainPage.ContentTemplate = this.FindResource("CampingTripsRegistration") as DataTemplate;
                        break;
                    }
                default:
                    {
                        userMainInfoPresenter.ContentTemplate = this.FindResource("UserMain") as DataTemplate;
                        mainPage.ContentTemplate = this.FindResource("CampingTripsRegistration") as DataTemplate;
                        break;
                    }

            }

            logout.ContentTemplate = this.FindResource("LogOut") as DataTemplate;
            Uri iconUri = new Uri("pack://application:,,,/Images/KanchLogo.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);


        }
    }
}
