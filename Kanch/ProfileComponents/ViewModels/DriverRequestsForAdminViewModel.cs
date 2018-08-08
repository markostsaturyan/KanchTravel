using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.HelperClasses;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;

namespace Kanch.ProfileComponents.ViewModels
{
    public class DriverRequestsForAdminViewModel
    {
        private TokenClient tokenClient;

        private HttpClient httpClient;

        public ObservableCollection<DriverRequests> DriverRequests { get; set; }

        public DriverRequestsForAdminViewModel()
        {
            this.DriverRequests = new ObservableCollection<DriverRequests>();
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["userManagementBaseUri"]);
            ConnectToServer();
            GetAllDriverRequests();
        }

        public void GetAllDriverRequests()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var response = httpClient.GetAsync("api/driververification").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var drivers = JsonConvert.DeserializeObject<List<Driver>>(jsonContent);

            foreach(var driver in drivers)
            {
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
                    Image = ImageConverter.ConvertImageToImageSource(driver.Image),
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
                var driverRequest = new HelperClasses.DriverRequests()
                {
                    Driver = driverInfo,
                    Accept = new Command(Accept),
                    Ignore = new Command(Ignore)
                };
                driverRequest.MoreInformation = new List<string>();
                if (driver.Car.HasAirConditioner)
                {
                    driverRequest.MoreInformation.Add("Air conditioner");
                }
                if (driver.Car.HasKitchen)
                {
                    driverRequest.MoreInformation.Add("Kitchen");
                }
                if (driver.Car.HasMicrophone)
                {
                    driverRequest.MoreInformation.Add("Microphone");
                }
                if (driver.Car.HasToilet)
                {
                    driverRequest.MoreInformation.Add("Toilet");
                }
                if (driver.Car.HasWiFi)
                {
                    driverRequest.MoreInformation.Add("WiFi");
                }
                this.DriverRequests.Add(driverRequest);
                
            }

        }

        public void Accept(object driverRequest)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var driver = (driverRequest as DriverRequests).Driver;

            var driverInfo = new Driver()
            {
                UserName = driver.UserName,
                Email = driver.Email
            };

            var response = httpClient.PostAsync("api/driververification", new StringContent(JsonConvert.SerializeObject(driverInfo), Encoding.UTF8, "application/json")).Result;

            this.DriverRequests.Remove(driverRequest as DriverRequests);
        }

        public void Ignore(object driverRequest)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var driver = (driverRequest as DriverRequests).Driver;
            var uri = new Uri("api/driververification/" + driver.UserName);

            var response = httpClient.DeleteAsync(uri).Result;

            this.DriverRequests.Remove(driverRequest as DriverRequests);
        }

        private void ConnectToServer()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationSettings.AppSettings["authenticationService"]).Result;

            if (disco.IsError)
            {
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
