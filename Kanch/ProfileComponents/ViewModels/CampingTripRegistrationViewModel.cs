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
    public class CampingTripRegistrationViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

#region Commands
        public ICommand AddFoodInTripFoods { get; set; }

        public ICommand EditFood { get; set; }

        public ICommand AddDirection { get; set; }

        public ICommand DeleteDirection { get; set; }

        #endregion

        #region Properties and fields
        private UserInfo user;

        private TokenClient tokenClient;

        private HttpClient httpClient;

        private CampingTripInfo tripRegistration;

        public CampingTripInfo TripRegistration
        {
            get
            {
                return this.tripRegistration;
            }
            set
            {
                this.tripRegistration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TripRegistration"));
            }
        }

        private FoodInfo registrationFood;

        public FoodInfo RegistrationFood
        {
            get
            {
                return this.registrationFood;
            }
            set
            {
                this.registrationFood = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RegistrationFood"));
            }
        }

        private string direction;

        private ObservableCollection<CampingTripInfo> myOrderedCampingTrips;

        public ObservableCollection<CampingTripInfo> MyOrderedCampingTrips
        {
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

        public string Direction
        {
            get
            {
                return this.direction;
            }

            set
            {
                this.direction = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Direction"));
            }
        }
#endregion


        public CampingTripRegistrationViewModel()
        {
            AddFoodInTripFoods = new Command(o => AddFoodInRegistrationTripFoods(), o => CanAddFoodInTripFoods());
            AddDirection = new Command(o => AddDirectionToRegistrationTrip());
            EditFood = new Command(EditTripFood);
            DeleteDirection = new Command(DeleteDirectionInRegistrationTrip);
            this.TripRegistration = new CampingTripInfo();

            tripRegistration.Food = new ObservableCollection<FoodInfo>();

            this.RegistrationFood = new FoodInfo();
            this.Direction = "";
        }

        public void DeleteDirectionInRegistrationTrip(object selectedItem)
        {

        }

        public void AddDirectionToRegistrationTrip()
        {
            this.tripRegistration.Direction.Add(this.direction);
        }

        public void EditTripFood(object name)
        {
            var foodName = name as string;

            if (foodName == null) throw new ArgumentNullException("Food name is null");

            registrationFood = tripRegistration.Food.Where(food => food.Name == foodName).First();
        }

        public void AddFoodInRegistrationTripFoods()
        {
            this.tripRegistration.Food.Add(registrationFood);

            registrationFood = new FoodInfo();
        }

        public bool CanAddFoodInTripFoods()
        {
            if (registrationFood.Name == null || registrationFood.Name == "" || registrationFood.Measure == 0) return false;


            return true;
        }

        public void GetCampingTrips()
        {
            var response = httpClient.GetAsync("api/CampingTrips").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var trips = JsonConvert.DeserializeObject<List<CampingTripInfo>>(jsonContent);

            var myOrderedtrips = new ObservableCollection<CampingTripInfo>();

            var zeroTime = new DateTime(1, 1, 1);

            var span = DateTime.Now - user.DateOfBirth;

            var userAge = (zeroTime + span).Year - 1;

            foreach (var trip in trips)
            {
                if (trip.OrganizationType == DataModel.TypeOfOrganization.OrderByUser && trip.Organizer.Id == user.Id)
                {
                    trip.CanIJoin = false;

                    trip.Status = "My Orderd Trip";

                    myOrderedtrips.Add(trip);
                }

            }

            MyOrderedCampingTrips = myOrderedtrips;
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
