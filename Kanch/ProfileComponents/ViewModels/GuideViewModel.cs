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
    public class GuideViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;

        private TokenClient tokenClient;

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

        public ICommand Requests { get; set; }

        public ICommand GetAllTripsCommand { get; set; }
        public ICommand GetMyCurrentTripsCommand { get; set; }
        public ICommand GetlMyPreviousTripsCommand { get; set; }
        public ICommand RegistrationOfTheTripCommand { get; set; }
        public ICommand GetMyRegistredTripsCommand { get; set; }

        public GuideViewModel()
        {
            this.Requests = new Command(o => SeeRequests());
            ConnectToServerAndGettingRefreshToken();
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"]);
            GetGuideInfo();
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

        private void GetMyCurrentTrips()
        {
            throw new NotImplementedException();
        }

        private void GetMyRegistredTrips()
        {
            var window = Application.Current.MainWindow;

            var presenter = window.FindName("mainPage") as ContentPresenter;
            presenter.ContentTemplate = window.FindResource("UsersRegistredTrips") as DataTemplate;
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

        public void GetGuideInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]).Result;

            this.httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync($"api/Guide/{ConfigurationManager.AppSettings["userId"]}").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var guide = JsonConvert.DeserializeObject<Guide>(jsonContent);

            var guideInfo = new GuideInfo
            {
                Id = guide.Id,
                FirstName = guide.FirstName,
                LastName = guide.LastName,
                Gender = guide.Gender,
                DateOfBirth = guide.DateOfBirth,
                Email = guide.Email,
                PhoneNumber = guide.PhoneNumber,
                UserName = guide.UserName,
                KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                NumberOfAppraisers = guide.NumberOfAppraisers,
                Rating = guide.Rating,
                EducationGrade = guide.EducationGrade,
                Places = guide.Places,
                Profession = guide.Profession,
                WorkExperience = guide.WorkExperience,
                Image = ImageConverter.ConvertImageToImageSource(guide.Image) ?? ImageConverter.DefaultProfilePicture(guide.Gender)
            };

            Guide = guideInfo;
        }

        private void ConnectToServerAndGettingRefreshToken()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationManager.AppSettings["authenticationService"]).Result;

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
