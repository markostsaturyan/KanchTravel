using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kanch.ProfileComponents.ViewModels
{
    public class UserRegistredTripsViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;
        private TokenClient tokenClient;
        private ObservableCollection<UserRegisteredInProgressTrip> tripsInProgresses;
        public ObservableCollection<UserRegisteredInProgressTrip> TripsInProgress
        {
            get { return this.tripsInProgresses; }
            set
            {
                this.tripsInProgresses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TripsInProgress"));
            }
        }

        public UserRegistredTripsViewModel()
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["baseUrl"]);
            ConnectToServer();
            this.TripsInProgress = new ObservableCollection<UserRegisteredInProgressTrip>();
            GetAllUserRegisteredTrips();

        }

        public void GetAllUserRegisteredTrips()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync($"api/UsersTrips").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var trips = JsonConvert.DeserializeObject<List<CampingTrip>>(jsonContent);

            var userTrips = new ObservableCollection<UserRegisteredInProgressTrip>();


            if (trips != null)
            {
                foreach (var trip in trips)
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

                    var tripInProgress = new UserRegisteredInProgressTrip();

                    tripInProgress.CampingTrip = campingtrip;
                    tripInProgress.DeleteTrip = new Command(DeleteTrip);

                    this.TripsInProgress.Add(tripInProgress);
                }
            }
        }

        private async void DeleteTrip(object userRegistredTrip)
        {
            var userRegisteredInProgressTrip = userRegistredTrip as UserRegisteredInProgressTrip;

            var deltaTime = userRegisteredInProgressTrip.CampingTrip.DepartureDate - DateTime.Now;

            if (deltaTime.Hours <= 12)
            {
                return;
            }

            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await httpClient.DeleteAsync($"api/UsersTrips/{userRegisteredInProgressTrip.CampingTrip.ID}");

            if (response.IsSuccessStatusCode)
            {
                this.TripsInProgress.Remove(userRegisteredInProgressTrip);
            }
        }

        private void ConnectToServer()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationManager.AppSettings["authenticationService"]).Result;

            if (disco.IsError)
            {
                return;
            }
            else
            {
                this.tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }
    }
}
