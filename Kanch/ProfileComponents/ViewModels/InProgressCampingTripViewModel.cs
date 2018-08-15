using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.HelperClasses;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;

namespace Kanch.ProfileComponents.ViewModels
{
    public class InProgressCampingTripViewModel : INotifyPropertyChanged
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

        public InProgressCampingTripViewModel()
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["baseUrl"]);
            ConnectToServer();
            this.TripsInProgress = new ObservableCollection<TripsInProgress>();
            GetUserInfo();
            GetAllInProgressTrips();
            
        }

        public void GetAllInProgressTrips()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var response = httpClient.GetAsync("api/CampingTrips").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var trips = JsonConvert.DeserializeObject<List<CampingTrip>>(jsonContent);

            var campingTrips = new ObservableCollection<TripsInProgress>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    var zeroTime = new DateTime(1, 1, 1);

                    var span = DateTime.Now - this.user.DateOfBirth;

                    var userAge = (zeroTime + span).Year - 1;

                    if (userAge >= trip.MinAge && userAge <= trip.MaxAge)
                    {

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
                            PriceOfTrip=trip.PriceOfTrip,
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

                        if (trip.MembersOfCampingTrip != null)
                        {
                            foreach(var member in trip.MembersOfCampingTrip)
                            {
                                if (member.Id == this.user.Id)
                                {
                                    campingtrip.IAmJoined = true;
                                }
                                campingtrip.MembersOfCampingTrip.Add(new UserInfo
                                {
                                    Id = member.Id
                                });
                            }

                        }

                        if (trip.CountOfMembers >= trip.MaxCountOfMembers)
                        {
                            campingtrip.CanIJoin = false;
                        }


                        if(!campingtrip.IAmJoined)
                        {
                            campingtrip.CanIJoin = true;
                        }

                        var tripInProgress = new TripsInProgress();

                        tripInProgress.CampingTrip = campingtrip;
                        tripInProgress.JoinCommand = new Command(Join);
                        tripInProgress.RefuseCommand = new Command(Refuse);

                        this.TripsInProgress.Add(tripInProgress);


                    }
                }
            }
        }

        public void Join(object trip)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var campingTrip = (trip as TripsInProgress).CampingTrip;
            var tripInfo = JsonConvert.SerializeObject(campingTrip);

            var campingTripId = campingTrip.ID;

            var tripId = new CampingTripId()
            {
                CampingTripID = campingTrip.ID,
            };

            var response = httpClient.PutAsync($"api/MembersOfCampingTrip/{user.Id}", new StringContent(JsonConvert.SerializeObject(tripId), Encoding.UTF8, "application/json")).Result;


            if (response.IsSuccessStatusCode)
            {
                campingTrip.IAmJoined = true;
                campingTrip.CanIJoin = false;
                if (campingTrip.MembersOfCampingTrip == null)
                {
                    campingTrip.MembersOfCampingTrip = new ObservableCollection<UserInfo>();
                }
                campingTrip.CountOfMembers++;
                campingTrip.MembersOfCampingTrip.Add(new UserInfo()
                {
                    Id=this.user.Id
                });
            }
        }

        public async void Refuse(object trip)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var campingTrip = (trip as TripsInProgress).CampingTrip;

            var response = await httpClient.DeleteAsync($"api/MembersOfCampingTrip/{user.Id}/{campingTrip.ID}");

            if (response.IsSuccessStatusCode)
            {
                campingTrip.IAmJoined = false;
                campingTrip.CanIJoin = true;
                campingTrip.CountOfMembers--;
               campingTrip.MembersOfCampingTrip.Remove(campingTrip.MembersOfCampingTrip.Where(userInfo => userInfo.Id == user.Id)?.First());
            }

        }

        public void GetUserInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;
            var httpClientLocal = new HttpClient();
            httpClientLocal.BaseAddress= new Uri(ConfigurationSettings.AppSettings["userManagementBaseUri"]);
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
