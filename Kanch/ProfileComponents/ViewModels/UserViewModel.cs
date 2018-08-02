using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kanch.ProfileComponents.ViewModels
{
    public class UserViewModel: INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand JoinToTrip { get; set; }

        public string StatusMessage;

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

        private List<CampingTripInfo> generalCampingTrips;

        public List<CampingTripInfo> GeneralCampingTrips
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

        private List<CampingTripInfo> myOrderedCampingTrips;

        public List<CampingTripInfo> MyOrderedCampingTrips {
            get
            {
                return this.myOrderedCampingTrips;
            }

            set
            {
                this.myOrderedCampingTrips = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MyOrderedCampingTrips"));
            }

        }

        public UserViewModel()
        {
            JoinToTrip = new Command(JoinToTripAsync, CanIJoinToTrip);
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
                Image = ImageConverter.ConvertImageToImageSource(user.Image),
            };

            User = userinfo;
        }

        public async void JoinToTripAsync(object campingTripId)
        {
            var tripId = campingTripId as string;

            var trip = this.generalCampingTrips.Find(campingTrip => campingTrip.ID == tripId);

            var response = await httpClient.PutAsync("api/MembersOfCampingTrip/" + User.Id, new StringContent(tripId));

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var status = JsonConvert.DeserializeObject<Status>(jsonContent);

            if (status.StatusCode == 5001)
            {
                trip.IAmJoined = true;
                trip.Status = "I joined to trip";
            }
        }

        public bool CanIJoinToTrip(object campingTripId)
        {
            string tripId = campingTripId as string;

            if(tripId==null)
            {
                throw new ArgumentException("Invalid value for camping trip id");
            }

            var trip = this.generalCampingTrips.Find(campingTrip => campingTrip.ID == tripId);

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

        }

        public bool CanDismissTrip(object campingTripId)
        {
            var tripId = campingTripId as string;

            var trip = this.generalCampingTrips.Find(campingTrip => campingTrip.ID == tripId);

            if (trip == null)
            {
                trip.Status = "Trip is not found";
                return false;
            }



        }



        public async void GetCampingTripsAsync()
        {
            var response = await httpClient.GetAsync("api/CampingTrips");

            var content = response.Content;

            var jsonContent = await content.ReadAsStringAsync();

            var trips = JsonConvert.DeserializeObject<List<CampingTripInfo>>(jsonContent);

            var myOrderedtrips = new List<CampingTripInfo>();

            var campingTrips = new List<CampingTripInfo>();

            var zeroTime = new DateTime(1, 1, 1);

            var span = DateTime.Now - user.DateOfBirth;

            var userAge = (zeroTime + span).Year - 1;

            foreach (var trip in trips)
            {
                if(trip.OrganizationType == DataModel.TypeOfOrganization.OrderByUser && trip.Organizer.Id == user.Id)
                {
                    trip.CanIJoin = false;

                    trip.Status = "My Orderd Trip";

                    myOrderedtrips.Add(trip);
                }

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

            MyOrderedCampingTrips = myOrderedtrips;
        }
    }
}
