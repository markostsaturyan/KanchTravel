using Kanch.Commands;
using Kanch.DataManagement.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;

namespace Kanch
{
    class HomeViewModel : BaseViewModel
    {
        public BaseViewModel ViewModel { get; set; }

        public ICommand LoginCommand { get; set; }

        private List<CampingTripFull> campingTrips = new List<CampingTripFull>();

        public List<CampingTripFull> CampingTrips
        {
            get
            {
                return campingTrips;
            }

            set
            {
                campingTrips = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("CampingTrips"));
            }
        }

        public HomeViewModel()
        {
            this.GetCampingTripsAsync();
            //LoginCommand = new Command(() => );
        }

        public async void GetCampingTripsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5001/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Http response message for get all products
                var response = await client.GetAsync("api/campingtrips");

                response.EnsureSuccessStatusCode();

                // Parse response content and print products
                using (HttpContent content = response.Content)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    var trips = JsonConvert.DeserializeObject<List<CampingTripFull>>(responseBody);

                    foreach (var trip in trips)
                    {
                        campingTrips.Add(trip);
                    }
                }
            }

        }
    }
}
