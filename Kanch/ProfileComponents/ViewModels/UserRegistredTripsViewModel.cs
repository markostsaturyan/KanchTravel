using IdentityModel.Client;
using Kanch.Commands;
using Kanch.ProfileComponents.DataModel;
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

        public ICommand DeleteTrip { get; set; }

        private TokenClient tokenClient;

        private HttpClient httpClient;

        private ObservableCollection<CampingTripInfo> myTrips;

        public ObservableCollection<CampingTripInfo> MyTrips
        {
            get
            {
                return this.myTrips;
            }

            set
            {
                this.myTrips = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MyTrips"));
            }
        }

        public UserRegistredTripsViewModel()
        {
            DeleteTrip = new Command(DeleteMyTripAsync, CanDeleteTrip);

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["baseUrl"])
            };

            GetMyTrips();
        }

        public async void DeleteMyTripAsync(object campingTripId)
        {
            var tripId = campingTripId as string;

            var trip = this.myTrips.Where(campingTrip => campingTrip.ID == tripId).First();

            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await httpClient.DeleteAsync($"api/UsersTrips/{tripId}");

            if (response.IsSuccessStatusCode)
            {
                myTrips.Remove(trip);
            }
        }

        public bool CanDeleteTrip(object campingTripId)
        {
            var tripId = campingTripId as string;

            var trip = this.myTrips.Where(campingTrip => campingTrip.ID == tripId).First();

            var deltaTime = trip.DepartureDate - DateTime.Now;

            if (deltaTime.Hours <= 12)
            {
                trip.Status = "You can not delete trip";
                return false;
            }

            return true;
        }

        public void GetMyTrips()
        {
            var response = httpClient.GetAsync("api/CampingTrips").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            MyTrips = JsonConvert.DeserializeObject<ObservableCollection<CampingTripInfo>>(jsonContent);
        }
    }
}
