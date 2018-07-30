using IdentityModel.Client;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kanch.ProfileComponents
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties and fields

        private string accessToken;

        public int ErrorCode;

        public string ErrorMessage;

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

        public DriverInfo driver;

        public DriverInfo Driver {

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

        public PhotographerInfo photographer;

        public PhotographerInfo Photographer
        {
            get
            {
                return this.photographer;
            }

            set
            {
                this.photographer = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Photographer"));
            }
        }

        public GuideInfo guide;

        public GuideInfo Guide
        {
            get
            {
                return this.guide;
            }

            set
            {
                this.guide = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Guide"));
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

#endregion

        public ProfileViewModel()
        {
            ConnectToServerAndGettingAccessTokenAsync();

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]),
            };

            httpClient.SetBearerToken(this.accessToken);
        }

        public async void GetCampingTripsAsync()
        {
            var response = await httpClient.GetAsync("api/CampingTrips");

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

            CampingTrips = JsonConvert.DeserializeObject<List<CampingTripInfo>>(jsonContent);
        } 

        public async void GetUserInfoAsync()
        {
            var response = await httpClient.GetAsync("api/User/" + ConfigurationSettings.AppSettings["userId"]);

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

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
                Image = ConvertImageToImageSource(user.Image),
            };

            User = userinfo;
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
                Image = ConvertImageToImageSource(driver.Image),
                Car = new CarInfo
                {
                    Id = driver.Car.Id,
                    Brand = driver.Car.Brand,
                    CarPicture1 = ConvertImageToImageSource(driver.Car.CarPicture1),
                    CarPicture2 = ConvertImageToImageSource(driver.Car.CarPicture2),
                    CarPicture3 = ConvertImageToImageSource(driver.Car.CarPicture3),
                    FuelType = driver.Car.FuelType,
                    HasAirConditioner = driver.Car.HasAirConditioner,
                    HasKitchen = driver.Car.HasKitchen,
                    HasMicrophone = driver.Car.HasMicrophone,
                    HasToilet = driver.Car.HasToilet,
                    HasWiFi = driver.Car.HasWiFi,
                    LicensePlate = driver.Car.LicensePlate,
                    NumberOfSeats = driver.Car.NumberOfSeats
                },
                DrivingLicencePicFront = ConvertImageToImageSource(driver.DrivingLicencePicFront),
                DrivingLicencePicBack = ConvertImageToImageSource(driver.DrivingLicencePicBack),
                KnowledgeOfLanguages=driver.KnowledgeOfLanguages,
                NumberOfAppraisers=driver.NumberOfAppraisers,
                Rating=driver.Rating
            };

            Driver = driverInfo;
        }

        public async void GetGuideInfoAsync()
        {
            var response = await httpClient.GetAsync("api/Guide/" + ConfigurationSettings.AppSettings["userId"]);

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

            var guide = JsonConvert.DeserializeObject<Guide>(jsonContent);

            var guideInfo = new GuideInfo
            {
                Id = guide.Id,
                FirstName = guide.FirstName,
                LastName = guide.LastName,
                Gender = guide.Gender,
                DateOfBirth = guide.DateOfBirth,
                Email = guide.Email,
                Image = ConvertImageToImageSource(guide.Image),
                EducationGrade = guide.EducationGrade,
                PhoneNumber = guide.PhoneNumber,
                KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                NumberOfAppraisers = guide.NumberOfAppraisers,
                UserName = guide.UserName,
                Profession = guide.Profession,
                Places = guide.Places,
                Rating = guide.Rating,
                WorkExperience = guide.WorkExperience
            };

            Guide = guideInfo;
        }

        public async void GetPhotographerInfoAsync()
        {
            var response = await httpClient.GetAsync("api/Photographer/" + ConfigurationSettings.AppSettings["userId"]);

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

            var photographer = JsonConvert.DeserializeObject<Photographer>(jsonContent);

            var photographerInfo = new PhotographerInfo
            {
                Id = photographer.Id,
                FirstName = photographer.FirstName,
                LastName = photographer.LastName,
                Gender = photographer.Gender,
                Email = photographer.Email,
                PhoneNumber = photographer.PhoneNumber,
                DateOfBirth = photographer.DateOfBirth,
                Image = ConvertImageToImageSource(photographer.Image),
                UserName = photographer.UserName,
                Camera = new CameraInfo
                {
                    Id = photographer.Camera.Id,
                    IsProfessional = photographer.Camera.IsProfessional,
                    Model = photographer.Camera.Model,
                },
                HasCameraStabilizator = photographer.HasCameraStabilizator,
                HasDron = photographer.HasDron,
                HasGopro = photographer.HasGopro,
                KnowledgeOfLanguages = photographer.KnowledgeOfLanguages,
                Profession = photographer.Profession,
                NumberOfAppraisers = photographer.NumberOfAppraisers,
                WorkExperience = photographer.WorkExperience,
                Raiting = photographer.Raiting
            };

            Photographer = photographerInfo;
        }

        private async void ConnectToServerAndGettingAccessTokenAsync()
        {
            var disco = await DiscoveryClient.GetAsync(ConfigurationSettings.AppSettings["authenticationService"]);

            TokenClient tokenClient;

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

            var response = await tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]);

            this.accessToken = response.AccessToken;
        }

        private ImageSource ConvertImageToImageSource(System.Drawing.Image image)
        {
            // ImageSource ...

            BitmapImage bi = new BitmapImage();

            bi.BeginInit();

            MemoryStream ms = new MemoryStream();

            // Save to a memory stream...

            image.Save(ms, ImageFormat.Bmp);

            // Rewind the stream...

            ms.Seek(0, SeekOrigin.Begin);

            // Tell the WPF image to use this stream...

            bi.StreamSource = ms;

            bi.EndInit();

            return bi;
        }
    }
}
