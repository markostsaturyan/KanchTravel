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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class ResponseOfTrip:INotifyPropertyChanged
    {
        private TokenClient tokenClient;

        private Visibility driverIsSelected;
        private Visibility guideIsSelected;
        private Visibility photographerIsSelected;

        private bool inputingPriceIsEnable;

        public bool InputingPriceIsEnable
        {
            get { return this.inputingPriceIsEnable; }
            set
            {
                this.inputingPriceIsEnable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InputingPriceIsEnable"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RemoveDriver { get; set; }
        public ICommand RemoveGuide { get; set; }
        public ICommand RemovePhotographer { get; set; }

        public ICommand AcceptTrip { get; set; }

        public Visibility DriverIsSelected
        {
            get
            {
                return this.driverIsSelected;
            }

            set
            {
                this.driverIsSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DriverIsSelected"));
            }
        }

        public Visibility GuideIsSelected
        {
            get
            {
                return this.guideIsSelected;
            }
            set
            {
                this.guideIsSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GuideIsSelected"));

            }
        }

        public Visibility PhotographerIsSelected
        {
            get
            {
                return this.photographerIsSelected;
            }
            set
            {
                this.photographerIsSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PhotographerIsSelected"));

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

                this.InputingPriceIsEnable = true;

                if (campingTrip.Driver != null)
                {
                    DriverIsSelected = Visibility.Visible;
                }
                else
                {
                    this.InputingPriceIsEnable = false;
                }

                if (campingTrip.HasGuide)
                {
                    if (campingTrip.Guide != null)
                    {
                        GuideIsSelected = Visibility.Visible;
                    }
                    else
                    {
                        this.InputingPriceIsEnable = false;
                    }
                }
                if (campingTrip.HasPhotographer)
                {
                    if (campingTrip.Photographer != null)
                    {
                        PhotographerIsSelected = Visibility.Visible;
                    }
                    else
                    {
                        this.InputingPriceIsEnable = false;
                    }
                }
            }
        }

        private ServiceRequestResponse selectedDriverResponse;

        public ServiceRequestResponse SelectedDriverResponse
        {
            get { return this.selectedDriverResponse; }
            set
            {
                this.selectedDriverResponse = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedDriverResponse"));
            }
        }

        public ObservableCollection<ServiceRequestResponse> DriversResponses { get; set; }

        private ServiceRequestResponse selectedGuideResponse;

        public ServiceRequestResponse SelectedGuideResponse
        {
            get { return this.selectedGuideResponse; }

            set
            {
                this.selectedGuideResponse = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedGuideResponse"));
            }
        }

        public ObservableCollection<ServiceRequestResponse> GuidesResponses { get; set; }

        private ServiceRequestResponse selectedPhotographerResponse;

        public ServiceRequestResponse SelectedPhotographerResponse
        {
            get { return this.selectedPhotographerResponse; }
            set
            {
                this.selectedPhotographerResponse = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedPhotographerResponse"));
            }
        }

        private CampingTripInfo campingTrip;

        public ObservableCollection<ServiceRequestResponse> PhotographersResponses { get; set; }

        public ResponseOfTrip(TokenClient tokenClient)
        {
            this.tokenClient = tokenClient;

            this.DriversResponses = new ObservableCollection<ServiceRequestResponse>();

            this.GuidesResponses = new ObservableCollection<ServiceRequestResponse>();

            this.PhotographersResponses = new ObservableCollection<ServiceRequestResponse>();

            RemoveDriver = new Command((_) => DeleteDriver());

            RemoveGuide = new Command((_) => DeleteGuide());

            RemovePhotographer = new Command((_) => DeletePhotographer());
        }

        public async void AcceptDriverAsync(object providerId)
        {
            var driverId = (int)providerId;

            var response = DriversResponses.Where(requestResponse => requestResponse.ProviderId == driverId).First();

            SelectedDriverResponse = response;

            DriversResponses.Remove(selectedDriverResponse);

            CampingTrip.Driver = await GetDriverAsync(driverId);

            DriverIsSelected = Visibility.Visible;

            if (campingTrip.HasGuide)
            {
                if (campingTrip.Guide != null)
                {
                    this.InputingPriceIsEnable = true;
                }
            }
            else
            {
                this.InputingPriceIsEnable = true;
            }

            if (campingTrip.HasPhotographer)
            {
                if (campingTrip.Photographer != null)
                {
                    this.InputingPriceIsEnable = true;
                }
                else
                {
                    this.InputingPriceIsEnable = false;
                }
            }
            else
            {
                this.InputingPriceIsEnable = true;
            }
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

            var response = GuidesResponses.Where(requestResponse => requestResponse.ProviderId == guideId).First();

            SelectedGuideResponse = response;

            GuidesResponses.Remove(selectedGuideResponse);

            CampingTrip.Guide = await GetGuideAsync(guideId);

            GuideIsSelected = Visibility.Visible;

            if (campingTrip.Driver != null)
            {
                if (campingTrip.HasPhotographer)
                {
                    if (campingTrip.Photographer != null)
                    {
                        this.InputingPriceIsEnable = true;
                    }
                }
                else
                {
                    this.inputingPriceIsEnable = true;
                }
            }
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

            var response = PhotographersResponses.Where(requestResponse => requestResponse.ProviderId == photographerId).First();

            SelectedPhotographerResponse = response;

            PhotographersResponses.Remove(response);

            CampingTrip.Photographer = await GetPhotographerAsync(photographerId);

            PhotographerIsSelected = Visibility.Visible;

            if (campingTrip.Driver != null)
            {
                if (campingTrip.HasGuide)
                {
                    if (campingTrip.Guide!= null)
                    {
                        this.InputingPriceIsEnable = true;
                    }
                }
                else
                {
                    this.InputingPriceIsEnable = true;
                }
            }
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
                Rating = photographer.Rating,
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
            DriverIsSelected = Visibility.Collapsed;

            DriversResponses.Add(selectedDriverResponse);

            SelectedDriverResponse = null;

            CampingTrip.Driver = null;

            this.InputingPriceIsEnable = false;
        }

        public void DeleteGuide()
        {
            GuideIsSelected = Visibility.Collapsed;

            GuidesResponses.Add(selectedGuideResponse);

            SelectedGuideResponse = null;

            CampingTrip.Guide = null;

            this.InputingPriceIsEnable = false;
        }

        public void DeletePhotographer()
        {
            PhotographerIsSelected = Visibility.Collapsed;

            PhotographersResponses.Add(selectedPhotographerResponse);

            SelectedPhotographerResponse = null;

            CampingTrip.Photographer = null;

            this.InputingPriceIsEnable = false;
        }

        public void AddServiceRequestResponce(ServiceRequestResponse response)
        {
            switch (response.ProviderRole)
            {
                case "Driver":
                    {
                        if (campingTrip.Driver != null && response.ProviderId==campingTrip.Driver.Id)
                        {
                            this.SelectedDriverResponse = response;
                            response.SelectDriver = new Command(AcceptDriverAsync);
                            break;
                        }

                        response.SelectDriver = new Command(AcceptDriverAsync);

                        this.DriversResponses.Add(response);

                        break;
                    }
                case "Guide":
                    {
                        if(campingTrip.Guide?.Id == response.ProviderId)
                        {
                            this.SelectedGuideResponse = response;
                            response.SelectGuide = new Command(AcceptGuideAsync);
                            break;
                        }

                        response.SelectGuide = new Command(AcceptGuideAsync);

                        this.GuidesResponses.Add(response);

                        break;
                    }
                case "Photographer":
                    {
                        if (campingTrip.Photographer?.Id == response.ProviderId)
                        {
                            this.SelectedPhotographerResponse = response;
                            response.SelectPhotographer = new Command(AcceptPhotographerAsync);
                            break;
                        }

                        response.SelectPhotographer = new Command(AcceptPhotographerAsync);

                        this.PhotographersResponses.Add(response);

                        break;
                    }
            }
        }
    }
}
