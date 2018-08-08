using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
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
    public class CampingTripViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand JoinToTrip { get; set; }

        public ICommand DismissInTrip { get; set; }

        private UserInfo user;

        private TokenClient tokenClient;

        private HttpClient httpClient;

        private ObservableCollection<CampingTripInfo> generalCampingTrips;

        public ObservableCollection<CampingTripInfo> GeneralCampingTrips
        {
            get
            {
                return this.generalCampingTrips;
            }

            set
            {
                this.generalCampingTrips = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GeneralCampingTrips"));
            }
        }


        public CampingTripViewModel()
        {
            JoinToTrip = new Command(JoinToCampingTrip, CanIJoinToTrip);
            DismissInTrip = new Command(DismissInTripAsync, CanDismissTrip);
            GetCampingTrips();
        }

        public void JoinToCampingTrip(object campingTripId)
        {
            var tripId = campingTripId as string;

            var trip = this.generalCampingTrips.Where(campingTrip => campingTrip.ID == tripId).First();

            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.PutAsync("api/MembersOfCampingTrip/" + user.Id, new StringContent(tripId)).Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var status = JsonConvert.DeserializeObject<Status>(jsonContent);

            if (status.StatusCode == 5001)
            {
                trip.IAmJoined = true;
                trip.CanIJoin = false;
                trip.MembersOfCampingTrip.Add(this.user);
                trip.Status = "I joined to trip";
            }
        }

        public bool CanIJoinToTrip(object campingTripId)
        {
            string tripId = campingTripId as string;

            if (tripId == null)
            {
                throw new ArgumentException("Invalid value for camping trip id");
            }

            var trip = this.generalCampingTrips.Where(campingTrip => campingTrip.ID == tripId).First();

            if (trip == null)
            {
                trip.Status = "This trip is not found";

                return false;
            }

            if (trip.IAmJoined || !trip.CanIJoin)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async void DismissInTripAsync(object campingTripId)
        {
            var tripId = campingTripId as string;

            var trip = this.generalCampingTrips.Where(campingTrip => campingTrip.ID == tripId).First();

            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]);

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await httpClient.DeleteAsync("api/MembersOfCampingTrip/" + user.Id + "/" + tripId);

            if (response.IsSuccessStatusCode)
            {
                trip.IAmJoined = false;
                trip.CanIJoin = true;
                trip.MembersOfCampingTrip.Remove(this.user);
                trip.Status = "You went out of the campaign list";
            }
        }

        public bool CanDismissTrip(object campingTripId)
        {
            var tripId = campingTripId as string;

            var trip = this.generalCampingTrips.Where(campingTrip => campingTrip.ID == tripId).First();

            if (trip == null)
            {
                trip.Status = "Trip is not found.";
                return false;
            }

            if (!trip.IAmJoined)
            {
                trip.Status = "You are not registered to this campaign.";
                return false;
            }

            return true;
        }

        public void GetCampingTrips()
        {
            var response = httpClient.GetAsync("api/CampingTrips").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var trips = JsonConvert.DeserializeObject<List<CampingTripInfo>>(jsonContent);

            var campingTrips = new ObservableCollection<CampingTripInfo>();

            var zeroTime = new DateTime(1, 1, 1);

            var span = DateTime.Now - user.DateOfBirth;

            var userAge = (zeroTime + span).Year - 1;

            foreach (var trip in trips)
            {
                if (trip.OrganizationType == DataModel.TypeOfOrganization.OrderByAdmin)
                {
                    if (userAge < trip.MinAge)
                    {
                        trip.CanIJoin = false;
                        trip.Status = "Your age is not enough for this campaign․";
                    }
                    else
                    {
                        if (userAge > trip.MaxAge)
                        {
                            trip.CanIJoin = false;
                            trip.Status = "Your age exceeds the maximum age for this campaign";
                        }
                        else
                        {
                            if (trip.CountOfMembers >= trip.MaxCountOfMembers)
                            {
                                trip.CanIJoin = false;
                                trip.Status = "Free places are over!";
                            }
                            else
                            {
                                trip.CanIJoin = true;
                                trip.Status = "Join our campaign!";
                            }
                        }
                    }

                    campingTrips.Add(trip);
                }
            }

            GeneralCampingTrips = campingTrips;
        }

        public void GetUserInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync("api/User/" + ConfigurationSettings.AppSettings["userId"]).Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

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

            };
            this.user = userinfo;
        }
    }
}
