using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class ResponseOfTrip:INotifyPropertyChanged
    {
        private TokenClient tokenClient;

        private double providersTotalPrice;
        private bool driverIsSelected;
        private bool guideIsSelected;
        private bool photographerIsSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        ICommand SelectDriver { get; set; }
        ICommand SelectGuide { get; set; }
        ICommand SelectPhotographer { get; set; }

        ICommand RemoveDriver { get; set; }
        ICommand RemoveGuide { get; set; }
        ICommand RemovePhotographer { get; set; }

        ICommand AcceptTrip { get; set; }

        public bool DriverIsSelected
        {
            get
            {
                return this.driverIsSelected;
            }

            set
            {
                this.driverIsSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DriverSelected"));
            }
        }

        public bool GuideIsSelected
        {
            get
            {
                return this.guideIsSelected;
            }
            set
            {
                this.guideIsSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("GuideIsSelected"));
            }
        }

        public bool PhotographerIsSelected
        {
            get
            {
                return this.photographerIsSelected;
            }
            set
            {
                this.photographerIsSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PhotographerIsSelected"));
            }
        }

        public double ProvidersTotalPrice {
            get
            {
                return providersTotalPrice;
            }

            set
            {
                this.providersTotalPrice = value;
                PropertyChanged(this,new PropertyChangedEventArgs("ProvidersTotalPrice"));
            }
        }

        public CampingTripInfo CampingTrip { get; set; }

        private ServiceRequestResponse selectedDriverResponse;
        public ObservableCollection<ServiceRequestResponse> DriversResponses { get; set; }

        private ServiceRequestResponse selectedGuideResponse;
        public ObservableCollection<ServiceRequestResponse> GudiesResponses { get; set; }

        private ServiceRequestResponse selectedPhotographerResponse;
        public ObservableCollection<ServiceRequestResponse> PhotographersResponses { get; set; }

        public ResponseOfTrip(TokenClient tokenClient)
        {
            this.providersTotalPrice = 0;

            this.tokenClient = tokenClient;

            SelectDriver = new Command(AcceptDriverAsync);
            SelectGuide = new Command(AcceptGuideAsync);
            SelectPhotographer = new Command(AcceptPhotographerAsync);

            RemoveDriver = new Command((_) => DeleteDriver());
            RemoveGuide = new Command((_) => DeleteGuide());
            RemovePhotographer = new Command((_) => DeletePhotographer());

            AcceptTrip = new Command(_ => AcceptTripAsync());
        }

        public async void AcceptDriverAsync(object providerId)
        {
            var driverId = (int)providerId;

            var response = DriversResponses.Where(requestResponse => requestResponse.ProviderId == driverId).First();

            selectedDriverResponse = response;

            CampingTrip.Driver = await GetDriverAsync(driverId);

            ProvidersTotalPrice += response.Price;
        }

        private async Task<DriverInfo> GetDriverAsync(int driverId)
        {
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"])
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var httpResponse = await httpClient.GetAsync($"api/Driver/{driverId}");

            var content = httpResponse.Content;

            var driverJson =await content.ReadAsStringAsync();

            var driver = JsonConvert.DeserializeObject<Driver>(driverJson);

            var car = new CarInfo
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
            };

            var driverInfo = new DriverInfo
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                DateOfBirth = driver.DateOfBirth,
                DrivingLicencePicBack = ImageConverter.ConvertImageToImageSource(driver.DrivingLicencePicBack),
                DrivingLicencePicFront = ImageConverter.ConvertImageToImageSource(driver.DrivingLicencePicFront),
                KnowledgeOfLanguages = driver.KnowledgeOfLanguages,
                Email = driver.Email,
                Gender = driver.Gender,
                Image = ImageConverter.ConvertImageToImageSource(driver.Image),
                NumberOfAppraisers = driver.NumberOfAppraisers,
                PhoneNumber = driver.PhoneNumber,
                Rating = driver.Rating,
                Car = car
            };

            return driverInfo;
        }

        public async void AcceptGuideAsync(object providerId)
        {
            var guideId = (int)providerId;

            var response = DriversResponses.Where(requestResponse => requestResponse.ProviderId == guideId).First();

            selectedGuideResponse = response;

            CampingTrip.Guide = await GetGuideAsync(guideId);

            ProvidersTotalPrice += response.Price;
        }

        private async Task<GuideInfo> GetGuideAsync(int guideId)
        {
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"])
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var httpResponse = await httpClient.GetAsync($"api/Guide/{guideId}");

            var content = httpResponse.Content;

            var guideJson = await content.ReadAsStringAsync();

            var guide = JsonConvert.DeserializeObject<Guide>(guideJson);

            var guideInfo = new GuideInfo
            {
                Id = guide.Id,
                FirstName = guide.FirstName,
                LastName = guide.LastName,
                DateOfBirth = guide.DateOfBirth,
                KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                Email = guide.Email,
                Gender = guide.Gender,
                Image = ImageConverter.ConvertImageToImageSource(guide.Image),
                NumberOfAppraisers = guide.NumberOfAppraisers,
                PhoneNumber = guide.PhoneNumber,
                Rating = guide.Rating,
                EducationGrade=guide.EducationGrade,
                Places=guide.Places,
                WorkExperience=guide.WorkExperience,
                Profession=guide.Profession
            };

            return guideInfo;
        }

        public async void AcceptPhotographerAsync(object providerId)
        {
            var photographerId = (int)providerId;

            var response = DriversResponses.Where(requestResponse => requestResponse.ProviderId == photographerId).First();

            selectedPhotographerResponse = response;

            PhotographersResponses.Remove(response);

            CampingTrip.Photographer = await GetPhotographerAsync(photographerId);

            ProvidersTotalPrice += response.Price;

            PhotographerIsSelected = true;
        }

        private async Task<PhotographerInfo> GetPhotographerAsync(int guideId)
        {
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"])
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var httpResponse = await httpClient.GetAsync($"api/Photographer/{guideId}");

            var content = httpResponse.Content;

            var photographerJson = await content.ReadAsStringAsync();

            var photographer = JsonConvert.DeserializeObject<Photographer>(photographerJson);

            var camera = new CameraInfo
            {
                Id = photographer.Camera.Id,
                IsProfessional = photographer.Camera.IsProfessional,
                Model = photographer.Camera.Model
            };

            var photographerInfo = new PhotographerInfo
            {
                Id = photographer.Id,
                FirstName = photographer.FirstName,
                LastName = photographer.LastName,
                DateOfBirth = photographer.DateOfBirth,
                KnowledgeOfLanguages = photographer.KnowledgeOfLanguages,
                Email = photographer.Email,
                Gender = photographer.Gender,
                Image = ImageConverter.ConvertImageToImageSource(photographer.Image),
                NumberOfAppraisers = photographer.NumberOfAppraisers,
                PhoneNumber = photographer.PhoneNumber,
                Raiting = photographer.Raiting,
                WorkExperience = photographer.WorkExperience,
                Profession = photographer.Profession,
                HasCameraStabilizator=photographer.HasCameraStabilizator,
                HasDron=photographer.HasDron,
                HasGopro=photographer.HasGopro,
                Camera=camera
            };

            return photographerInfo;
        }

        public void DeleteDriver()
        {
            DriverIsSelected = false;

            ProvidersTotalPrice -= selectedDriverResponse.Price;

            DriversResponses.Add(selectedDriverResponse);

            CampingTrip.Driver = new DriverInfo();
        }

        public void DeleteGuide()
        {
            GuideIsSelected = false;

            ProvidersTotalPrice -= selectedGuideResponse.Price;

            GudiesResponses.Add(selectedGuideResponse);

            CampingTrip.Guide = new GuideInfo();
        }

        public void DeletePhotographer()
        {
            PhotographerIsSelected = false;

            ProvidersTotalPrice -= selectedPhotographerResponse.Price;

            PhotographersResponses.Add(selectedPhotographerResponse);

            CampingTrip.Photographer = new PhotographerInfo();
        }

        public async void AcceptTripAsync()
        {
            var tokenResponse =await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["baseUrl"]);

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var tripJson = JsonConvert.SerializeObject(CampingTrip);

            await httpClient.PostAsync($"api/CampingTrips/{CampingTrip.ID}", new StringContent(tripJson));
        }

        public void AddServiceRequestResponce(ServiceRequestResponse response)
        {
            switch (response.ProviderRole)
            {
                case "Driver":
                    {
                        DriversResponses.Add(response);

                        break;
                    }
                case "Guide":
                    {
                        GudiesResponses.Add(response);

                        break;
                    }
                case "Photographer":
                    {
                        PhotographersResponses.Add(response);

                        break;
                    }
            }
        }
    }
}
