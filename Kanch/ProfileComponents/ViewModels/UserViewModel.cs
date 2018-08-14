using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace Kanch.ProfileComponents.ViewModels
{
    public class UserViewModel: INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Commands
        public ICommand GetAllTripsCommand { get; set; }
        public ICommand GetMyCurrentTripsCommand { get; set; }
        public ICommand GetlMyPreviousTripsCommand { get; set; }
        public ICommand RegistrationOfTheTripCommand { get; set; }
        public ICommand GetMyRegistredTripsCommand { get; set; }

        #endregion

        #region Properties and fields
        public int ErrorCode;

        public string ErrorMessage;

        private TokenClient tokenClient;

        private HttpClient httpClient;

        public UserInfo user;

        public UserInfo User
        {
            get
            {
                return this.user;
            }

            set
            {
                this.user = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User"));
            }

        }

        #endregion

        public UserViewModel()
        {
            ConnectToServerAndGettingRefreshToken();
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"]);
            GetUserInfo();
            this.GetAllTripsCommand = new Command(o => GetAllTrip());
            this.GetMyCurrentTripsCommand = new Command(o => GetMyCurrentTrips());
            this.GetlMyPreviousTripsCommand = new Command(o => GetMyPreviousTrips());
            this.RegistrationOfTheTripCommand = new Command(o => RegistrationOfTheTrip());
            this.GetMyRegistredTripsCommand = new Command(o => GetMyRegistredTrips());
        }

        private void RegistrationOfTheTrip()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("CampingTripsRegistration") as DataTemplate;
        }

        private void GetMyPreviousTrips()
        {
            throw new NotImplementedException();
        }

        private void GetMyRegistredTrips()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("UsersRegistredTrips") as DataTemplate;
        }

        private void GetMyCurrentTrips()
        {
            throw new NotImplementedException();
        }

        private void GetAllTrip()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("InProgressCampingTrips") as DataTemplate;
        }

        public void GetUserInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync($"api/User/{ConfigurationManager.AppSettings["userId"]}").Result;

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
                Image = ImageConverter.ConvertImageToImageSource(user.Image) ?? ImageConverter.DefaultProfilePicture(user.Gender)
            };
            
            this.User = userinfo;
        }

        private void ConnectToServerAndGettingRefreshToken()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationManager.AppSettings["authenticationService"]).Result;

            if (disco.IsError)
            {
                ErrorCode = 404;

                ErrorMessage = disco.Error;

                return;
            }
            else
            {
                tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }
    }
}
