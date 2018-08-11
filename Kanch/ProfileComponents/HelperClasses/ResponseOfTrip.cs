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
using System.Windows.Media.Imaging;

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

        public ICommand SelectDriver { get; set; }
        public ICommand SelectGuide { get; set; }
        public ICommand SelectPhotographer { get; set; }

        public ICommand RemoveDriver { get; set; }
        public ICommand RemoveGuide { get; set; }
        public ICommand RemovePhotographer { get; set; }

        public ICommand AcceptTrip { get; set; }

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

        public CampingTripInfo CampingTrip {
            get
            {
                return this.campingTrip;


            }
            set
            {
                this.campingTrip = value;

                if (campingTrip.Driver != null)
                {
                    DriverIsSelected = true;
                }
                if (campingTrip.Guide != null)
                {
                    GuideIsSelected = true;
                }
                if (campingTrip.Photographer != null)
                {
                    PhotographerIsSelected = true;
                }
            }
        }

        private ServiceRequestResponse selectedDriverResponse;
        public ObservableCollection<ServiceRequestResponse> DriversResponses { get; set; }

        private ServiceRequestResponse selectedGuideResponse;
        public ObservableCollection<ServiceRequestResponse> GudiesResponses { get; set; }

        private ServiceRequestResponse selectedPhotographerResponse;
        private CampingTripInfo campingTrip;

        public ObservableCollection<ServiceRequestResponse> PhotographersResponses { get; set; }

        public ResponseOfTrip(TokenClient tokenClient)
        {
            this.providersTotalPrice = 0;

            this.tokenClient = tokenClient;

            this.DriversResponses = new ObservableCollection<ServiceRequestResponse>();
            this.GudiesResponses = new ObservableCollection<ServiceRequestResponse>();
            this.PhotographersResponses = new ObservableCollection<ServiceRequestResponse>();

            SelectDriver = new Command(AcceptDriverAsync);
            SelectGuide = new Command(AcceptGuideAsync);
            SelectPhotographer = new Command(AcceptPhotographerAsync);

            RemoveDriver = new Command((_) => DeleteDriver());
            RemoveGuide = new Command((_) => DeleteGuide());
            RemovePhotographer = new Command((_) => DeletePhotographer());

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
                NumberOfAppraisers = driver.NumberOfAppraisers,
                PhoneNumber = driver.PhoneNumber,
                Rating = driver.Rating,
                Car = car
            };

            if (driverInfo.Image != null)
            {
                driverInfo.Image = ImageConverter.ConvertImageToImageSource(driver.Image);
            }
            else
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                if (driverInfo.Gender == "Female")
                {
                    img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/female.jpg");
                }
                else
                {
                    img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/male.jpg");
                }
                img.EndInit();
                driverInfo.Image = img;
            }


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
                NumberOfAppraisers = guide.NumberOfAppraisers,
                PhoneNumber = guide.PhoneNumber,
                Rating = guide.Rating,
                EducationGrade=guide.EducationGrade,
                Places=guide.Places,
                WorkExperience=guide.WorkExperience,
                Profession=guide.Profession
            };

            if (guideInfo.Image != null)
            {
                guideInfo.Image = ImageConverter.ConvertImageToImageSource(guide.Image);
            }
            else
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                if (guideInfo.Gender == "Female")
                {
                    img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/female.jpg");
                }
                else
                {
                    img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/male.jpg");
                }
                img.EndInit();
                guideInfo.Image = img;
            }

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

            if (photographerInfo.Image != null)
            {
                photographerInfo.Image = ImageConverter.ConvertImageToImageSource(photographer.Image);
            }
            else
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                if (photographerInfo.Gender == "Female")
                {
                    img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/female.jpg");
                }
                else
                {
                    img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/male.jpg");
                }
                img.EndInit();
                photographerInfo.Image = img;
            }

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

       

        public void AddServiceRequestResponce(ServiceRequestResponse response)
        {
            switch (response.ProviderRole)
            {
                case "Driver":
                    {
                        this.DriversResponses.Add(response);

                        break;
                    }
                case "Guide":
                    {
                        this.GudiesResponses.Add(response);

                        break;
                    }
                case "Photographer":
                    {
                        this.PhotographersResponses.Add(response);

                        break;
                    }
            }
        }
    }
}
