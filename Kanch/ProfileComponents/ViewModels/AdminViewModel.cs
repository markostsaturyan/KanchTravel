using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;

namespace Kanch.ProfileComponents.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand GetDriverRequestsCommand { get; set; }
        public ICommand GetAllTripsCommand { get; set; }
        public ICommand GetMyCurrentTripsCommand { get; set; }
        public ICommand GetMyPreviousTripsCommand { get; set; }
        public ICommand GetUnconfirmedCampingTripsCommand { get; set; }
        public ICommand RegistrationOfTheTripCommand { get; set; }
        public ICommand GetRequestsResponsesCommand { get; set; }

        private HttpClient httpClient;
        private TokenClient tokenClient;

        private ImageSource male;
        private ImageSource female;

        public UserInfo User { get; set; }
        public AdminViewModel()
        {
            this.male = new BitmapImage(new Uri(String.Format("Images/male.jpg"), UriKind.Relative));
            this.male.Freeze();
            this.female = new BitmapImage(new Uri(String.Format("Images/female.jpg"), UriKind.Relative));
            this.female.Freeze();

            this.GetDriverRequestsCommand = new Command(o => GetDriverRequests());
            this.GetAllTripsCommand = new Command(o => GetAllTrips());
            this.GetMyCurrentTripsCommand = new Command(o => GetMyCurrentTrips());
            this.GetMyPreviousTripsCommand = new Command(o => GetMyPreviousTris());
            this.GetUnconfirmedCampingTripsCommand = new Command(o => GetUnconfirmedCampingTrips());
            this.GetRequestsResponsesCommand = new Command(o => GetRequestsResponses());
            this.RegistrationOfTheTripCommand = new Command(o => RegistrationOfTheTrip());
            ConnectToServer();
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["userManagementBaseUri"]);
            GetUserInfo();
        }

        private void GetRequestsResponses()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("TripsManagementOfAdminView") as DataTemplate;
        }

        private void RegistrationOfTheTrip()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("CampingTripRegistrationForAdmin") as DataTemplate;
        }
        private void GetMyPreviousTris()
        {
            throw new NotImplementedException();
        }

        private void GetMyCurrentTrips()
        {
            throw new NotImplementedException();
        }

        private void GetAllTrips()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("InProgressCampingTrips") as DataTemplate;
        }

        public void GetDriverRequests()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("DriverRequestsForAdmin") as DataTemplate;
        }

        public void GetUnconfirmedCampingTrips()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("ConfirmationOfTrips") as DataTemplate;
        }
         public void GetUserInfo()
        {

            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            this.httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync("api/User/" + ConfigurationSettings.AppSettings["userId"]).Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var user = JsonConvert.DeserializeObject<User>(jsonContent);

            var userinfo = new UserInfo
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,

            };
            if (user.Image != null)
            {
                userinfo.Image = ImageConverter.ConvertImageToImageSource(user.Image);
            }
            else
            {
                if (userinfo.Gender == "Female")
                {
                    userinfo.Image = this.female;
                }
                else
                {
                    userinfo.Image = this.male;
                }
            }
            this.User = userinfo;
        }
        private async void ConnectToServer()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationSettings.AppSettings["authenticationService"]).Result;

            if (disco.IsError)
            {
                //ErrorCode = 404;
                //
                //ErrorMessage = disco.Error;

                return;
            }
            else
            {
                tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }

    }
}
