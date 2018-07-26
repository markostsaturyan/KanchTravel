using Kanch.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kanch.Profile
{
    public class ProfileViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;

        public User user;

        public List<CampingTrip> campingTrips;

        public List<CampingTrip> CampingTrips
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

        public User User
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

        public ProfileViewModel()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            
        }

        public List<CampingTrip> GetCampingTrips()
        {
            httpClient.SetBearerToken(ConfigurationSettings.AppSettings["accessToken"]);

            httpClient.GetAsync()
        } 

    }
}
