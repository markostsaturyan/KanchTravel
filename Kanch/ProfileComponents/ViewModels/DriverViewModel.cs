using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kanch.ProfileComponents.ViewModels
{
    public class DriverViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;

        private TokenClient tokenClient;

        public DriverInfo driver;

        public DriverInfo Driver
        {

            get
            {
                return this.driver;
            }

            set
            {
                this.driver = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Driver"));
            }
        }

        public List<CampingTripInfo> campingTrips;

        public List<CampingTripInfo> CampingTrips
        {
            get
            {
                return this.campingTrips;
            }

            set
            {
                this.campingTrips = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CampingTrips"));
            }
        }

        public ICommand Requests { get; set; }

        public ICommand GetAllTripsCommand { get; set; }
        public ICommand GetMyCurrentTripsCommand { get; set; }
        public ICommand RegistrationOfTheTripCommand { get; set; }
        public ICommand GetMyRegistredTripsCommand { get; set; }

        public DriverViewModel()
        {
            this.Requests = new Command(o => SeeRequests());
            ConnectToServerAndGettingRefreshToken();
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"]);
            GetDriverInfo();
            this.GetAllTripsCommand = new Command(o => GetAllTrip());
            this.GetMyCurrentTripsCommand = new Command(o => GetMyCurrentTrips());
            this.RegistrationOfTheTripCommand = new Command(o => RegistrationOfTheTrip());
            this.GetMyRegistredTripsCommand = new Command(o => GetMyRegistredTrips());
        }

        private void RegistrationOfTheTrip()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("CampingTripsRegistration") as DataTemplate;
        }

       

        private void GetMyCurrentTrips()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("CampingTripsUserIsJoined") as DataTemplate;
        }

        private void GetMyRegistredTrips()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("UsersRegisteredTrips") as DataTemplate;
        }

        private void GetAllTrip()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("InProgressCampingTrips") as DataTemplate;
        }

        private void SeeRequests()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("CampingTripRequests") as DataTemplate;
        }

        public void GetDriverInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]).Result;

            this.httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync($"api/Driver/{ConfigurationManager.AppSettings["userId"]}").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var driver = JsonConvert.DeserializeObject<Driver>(jsonContent);

            var driverInfo = new DriverInfo
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Gender = driver.Gender,
                DateOfBirth = driver.DateOfBirth,
                Email = driver.Email,
                PhoneNumber = driver.PhoneNumber,
                UserName = driver.UserName,
                Car = new CarInfo
                {
                    Id = driver.Car.Id,
                    Brand = driver.Car.Brand,
                    CarPicture1 = ImageConverter.ConvertImageToImageSource(driver.Car.CarPicture1),
                    CarPicture2 = ImageConverter.ConvertImageToImageSource(driver.Car.CarPicture2),
                    CarPicture3 = ImageConverter.ConvertImageToImageSource(driver.Car.CarPicture3),
                    FuelType = driver.Car.FuelType,
                    HasAirConditioner = driver.Car.HasAirConditioner,
                    HasKitchen = driver.Car.HasKitchen,
                    HasMicrophone = driver.Car.HasMicrophone,
                    HasToilet = driver.Car.HasToilet,
                    HasWiFi = driver.Car.HasWiFi,
                    LicensePlate = driver.Car.LicensePlate,
                    NumberOfSeats = driver.Car.NumberOfSeats
                },
                DrivingLicencePicFront = ImageConverter.ConvertImageToImageSource(driver.DrivingLicencePicFront),
                DrivingLicencePicBack = ImageConverter.ConvertImageToImageSource(driver.DrivingLicencePicBack),
                KnowledgeOfLanguages = driver.KnowledgeOfLanguages,
                NumberOfAppraisers = driver.NumberOfAppraisers,
                Rating = driver.Rating,
                Image = ImageConverter.ConvertImageToImageSource(driver.Image) ?? ImageConverter.DefaultProfilePicture(driver.Gender)
            };

            Driver = driverInfo;
        }

        private void ConnectToServerAndGettingRefreshToken()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationSettings.AppSettings["authenticationService"]).Result;

            if (disco.IsError)
            {
                return;
            }
            else
            {
                tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }
    }
}