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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kanch.ProfileComponents.ViewModels
{
    public class DriverViewModel : INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;

        private TokenClient tokenClient;

        private ImageSource male;

        private ImageSource female;

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
        public ICommand GetlMyPreviousTripsCommand { get; set; }
        public ICommand RegistrationOfTheTripCommand { get; set; }

        public DriverViewModel()
        {
            this.male = new BitmapImage(new Uri(String.Format("Images/male.jpg"), UriKind.Relative));
            this.male.Freeze();
            this.female = new BitmapImage(new Uri(String.Format("Images/female.jpg"), UriKind.Relative));
            this.female.Freeze();

            this.Requests = new Command(o => SeeRequests());
            ConnectToServerAndGettingRefreshTokenAsync();
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["userManagementBaseUri"]);
            GetDriverInfo();
            this.GetAllTripsCommand = new Command(o => GetAllTrip());
            this.GetMyCurrentTripsCommand = new Command(o => GetMyCurrentTrips());
            this.GetlMyPreviousTripsCommand = new Command(o => GetMyPreviousTrips());
            this.RegistrationOfTheTripCommand = new Command(o => RegistrationOfTheTrip());
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

        private void SeeRequests()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("CampingTripRequests") as DataTemplate;
        }

        public void GetDriverInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            this.httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync("api/Driver/" + ConfigurationSettings.AppSettings["userId"]).Result;

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
                Rating = driver.Rating
            };
            if (driver.Image != null)
            {
                driverInfo.Image = ImageConverter.ConvertImageToImageSource(driver.Image);
            }
            else
            {
                if (driverInfo.Gender == "Female")
                {
                    driverInfo.Image = this.female;
                }
                else
                {
                    driverInfo.Image = this.male;
                }
            }

            Driver = driverInfo;
        }

        public async void JoinToTripAsync(object campingTripId)
        {
            var tripId = campingTripId as string;

            if (tripId == null) return;

            await httpClient.PutAsync("api/MembersOfCampingTrip/" + Driver.Id, new StringContent(tripId));
        }

        private async void ConnectToServerAndGettingRefreshTokenAsync()
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
