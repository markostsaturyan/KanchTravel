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
    public class PhotographerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;

        private TokenClient tokenClient;

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

        public PhotographerViewModel()
        {
            this.Requests = new Command(o => SeeRequests());
            ConnectToServerAndGettingRefreshToken();
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"]);
            GetPhotographerInfo();
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

        public void GetPhotographerInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]).Result;

            this.httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync($"api/Photographer/{ConfigurationManager.AppSettings["userId"]}").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var photographer = JsonConvert.DeserializeObject<Photographer>(jsonContent);

            var photographerInfo = new PhotographerInfo
            {
                Id = photographer.Id,
                FirstName = photographer.FirstName,
                LastName = photographer.LastName,
                Gender = photographer.Gender,
                DateOfBirth = photographer.DateOfBirth,
                Email = photographer.Email,
                PhoneNumber = photographer.PhoneNumber,
                UserName = photographer.UserName,
                KnowledgeOfLanguages = photographer.KnowledgeOfLanguages,
                NumberOfAppraisers = photographer.NumberOfAppraisers,
                Profession = photographer.Profession,
                WorkExperience = photographer.WorkExperience,
                Image = ImageConverter.ConvertImageToImageSource(photographer.Image) ?? ImageConverter.DefaultProfilePicture(photographer.Gender),
                HasCameraStabilizator = photographer.HasCameraStabilizator,
                HasDron = photographer.HasDron,
                HasGopro = photographer.HasGopro,
                Rating = photographer.Rating,
                Camera = new CameraInfo
                {
                    Id = photographer.Camera.Id,
                    IsProfessional = photographer.Camera.IsProfessional,
                    Model = photographer.Camera.Model
                }
            };

            Photographer = photographerInfo;
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
