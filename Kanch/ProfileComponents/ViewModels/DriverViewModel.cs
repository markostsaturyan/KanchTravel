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

namespace Kanch.ProfileComponents.ViewModels
{
    public class DriverViewModel : INotifyPropertyChanged
    {
        public DriverViewModel() { }

        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;


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

        public async void GetDriverInfoAsync()
        {
            var response = await httpClient.GetAsync("api/Driver/" + ConfigurationSettings.AppSettings["userId"]);

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

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
               // Image = ImageConverter.ConvertImageToImageSource(driver.Image),
                Car = new CarInfo
                {
                    Id = driver.Car.Id,
                    Brand = driver.Car.Brand,
                   // CarPicture1 = ImageConverter.ConvertImageToImageSource(driver.Car.CarPicture1),
                   // CarPicture2 = ImageConverter.ConvertImageToImageSource(driver.Car.CarPicture2),
                   // CarPicture3 = ImageConverter.ConvertImageToImageSource(driver.Car.CarPicture3),
                    FuelType = driver.Car.FuelType,
                    HasAirConditioner = driver.Car.HasAirConditioner,
                    HasKitchen = driver.Car.HasKitchen,
                    HasMicrophone = driver.Car.HasMicrophone,
                    HasToilet = driver.Car.HasToilet,
                    HasWiFi = driver.Car.HasWiFi,
                    LicensePlate = driver.Car.LicensePlate,
                    NumberOfSeats = driver.Car.NumberOfSeats
                },
                //DrivingLicencePicFront = ImageConverter.ConvertImageToImageSource(driver.DrivingLicencePicFront),
                //DrivingLicencePicBack = ImageConverter.ConvertImageToImageSource(driver.DrivingLicencePicBack),
                KnowledgeOfLanguages = driver.KnowledgeOfLanguages,
                NumberOfAppraisers = driver.NumberOfAppraisers,
                Rating = driver.Rating
            };

            Driver = driverInfo;
        }

        public async void JoinToTripAsync(object campingTripId)
        {
            var tripId = campingTripId as string;

            if (tripId == null) return;

            await httpClient.PutAsync("api/MembersOfCampingTrip/" + Driver.Id, new StringContent(tripId));
        }
    }
}
