using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Newtonsoft.Json;

namespace Kanch.ProfileComponents.ViewModels
{
    class UsersJoinedCampingTripsViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;
        private TokenClient tokenClient;
        private User user;
        private ObservableCollection<TripsInProgress> tripsInProgresses;
        public ObservableCollection<TripsInProgress> TripsInProgress
        {
            get { return this.tripsInProgresses; }
            set
            {
                this.tripsInProgresses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TripsInProgress"));
            }
        }

        public UsersJoinedCampingTripsViewModel()
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            ConnectToServer();
            this.TripsInProgress = new ObservableCollection<TripsInProgress>();
            GetUserInfo();
            GetAllTripsUserIsJoined();

        }

        public void GetAllTripsUserIsJoined()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var response = httpClient.GetAsync($"api/MembersOfCampingTrip/{user.Id}").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var tripIds = JsonConvert.DeserializeObject<List<string>>(jsonContent);

           

            var campingTrips = new ObservableCollection<TripsInProgress>();
            if (tripIds != null)
            {
                foreach (var tripId in tripIds)
                {
                    var tripResponse = httpClient.GetAsync($"api/CampingTrips/{tripId}").Result;
                    var tripContent = tripResponse.Content;
                    var tripJsonContent = tripContent.ReadAsStringAsync().Result;
                    var trip = JsonConvert.DeserializeObject<CampingTrip>(tripJsonContent);


                    var campingtrip = new CampingTripInfo()
                    {
                        Place = trip.Place,
                        DepartureDate = trip.DepartureDate,
                        ArrivalDate = trip.ArrivalDate,
                        CountOfMembers = trip.CountOfMembers,
                        MinAge = trip.MinAge,
                        MaxAge = trip.MaxAge,
                        MinCountOfMembers = trip.MinCountOfMembers,
                        MaxCountOfMembers = trip.MaxCountOfMembers,
                        Direction = trip.Direction,
                        HasGuide = trip.HasGuide,
                        HasPhotographer = trip.HasPhotographer,
                        PriceOfTrip = trip.PriceOfTrip,
                        ID = trip.ID
                    };
                    if (trip.TypeOfTrip == Kanch.DataModel.TypeOfCampingTrip.Campaign)
                    {
                        campingtrip.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.Campaign;
                    }
                    else if (trip.TypeOfTrip == Kanch.DataModel.TypeOfCampingTrip.CampingTrip)
                    {
                        campingtrip.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.CampingTrip;
                    }
                    else
                    {
                        campingtrip.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.Excursion;
                    }
                    if (trip.Food != null)
                    {
                        campingtrip.Food = new ObservableCollection<FoodInfo>();
                        foreach (var food in trip.Food)
                        {
                            campingtrip.Food.Add(new FoodInfo()
                            {
                                Name = food.Name,
                                Measure = food.Measure,
                                MeasurementUnit = food.MeasurementUnit
                            });
                        }
                    }

                    campingtrip.MembersOfCampingTrip = new ObservableCollection<UserInfo>();
                    campingtrip.IAmJoined = true;

                    var tripInProgress = new TripsInProgress();

                    tripInProgress.CampingTrip = campingtrip;
                    tripInProgress.RefuseCommand = new Command(Refuse);

                    this.TripsInProgress.Add(tripInProgress);
                }
            }
        }


        public async void Refuse(object trip)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var tripInProgress = trip as TripsInProgress;
            var campingTrip = tripInProgress.CampingTrip;

            var response = await httpClient.DeleteAsync($"api/MembersOfCampingTrip/{user.Id}/{campingTrip.ID}");

            if (response.IsSuccessStatusCode)
            {
                this.TripsInProgress.Remove(tripInProgress);
            }

        }

        public void GetUserInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;
            var httpClientLocal = new HttpClient();
            httpClientLocal.BaseAddress = new Uri(ConfigurationSettings.AppSettings["userManagementBaseUri"]);
            httpClientLocal.SetBearerToken(tokenResponse.AccessToken);


            var response = httpClientLocal.GetAsync("api/User/" + ConfigurationSettings.AppSettings["userId"]).Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            this.user = JsonConvert.DeserializeObject<User>(jsonContent);

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
                this.tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }
    }
}
