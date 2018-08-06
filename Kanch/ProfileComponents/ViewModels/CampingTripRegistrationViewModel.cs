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
        public ICommand AddFoodToTripFoodsCommand { get; set; }
        public ICommand AddDirectionCommand { get; set; }
        public ICommand RegisterTripCommand { get; set; }
        #endregion

        #region Properties and fields
        private UserInfo user;

        private TokenClient tokenClient;

        private HttpClient httpClient;

        private CampingTripInfo tripRegistration;

        private string inputDirection;

        private ObservableCollection<Direction> direction;

        private ObservableCollection<CampingTripInfo> myOrderedCampingTrips;

        private string errorMessage;

        public string InputDirection
        {
            get { return this.inputDirection; }
            set
            {
                this.inputDirection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InputDirection"));
            }
        }

        public ObservableCollection<Direction> Direction
        {
            get
            {
                return this.direction;
            }

            set
            {
                this.direction = value;
                NotifyPropertyChanged("Direction");
            }
        }

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

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                this.errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }


        #endregion


        #region FoodInfo
        private string foodName;
        private double measureOfTheFood;
        private string measurementUnit;
        private ObservableCollection<Food> foods;
        private Food registrationFood;

        public string FoodName
        {
            get { return this.foodName; }
            set
            {
                this.foodName = value;
                NotifyPropertyChanged("FoodName");
            }
        }

        public double MeasureOfTheFood
        {
            get { return this.measureOfTheFood; }
            set
            {
                this.measureOfTheFood = value;
                NotifyPropertyChanged("MeasureOfTheFood");
            }
        }

        public string MeasurementUnit
        {
            get { return this.measurementUnit; }
            set
            {
                if (value.Contains(':'))
                {
                    this.measurementUnit = value.Split(':')[1].TrimStart(' ');
                }
                else
                {
                    this.measurementUnit = value;
                }
                NotifyPropertyChanged("MeasurementUnit");
            }
        }

        public ObservableCollection<Food> Foods
        {
            get { return this.foods; }
            set
            {
                this.foods = value;
                NotifyPropertyChanged("Foods");
            }
        }

        public Food RegistrationFood
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
        #endregion



        public CampingTripRegistrationViewModel()
        {
            this.AddDirectionCommand = new Command(o => AddDirectionToRegistrationTrip());
            this.AddFoodToTripFoodsCommand = new Command(o => AddFoodToRegistrationTripFoods(), o => CanAddFoodToTripFoods());
            this.RegisterTripCommand = new Command(o => RegistrationTrip());

            this.direction = new ObservableCollection<Direction>();
            this.foods = new ObservableCollection<Food>();
            this.tripRegistration = new CampingTripInfo();
            this.tripRegistration.DepartureDate = DateTime.Now;
            this.tripRegistration.ArrivalDate = DateTime.Now;

            
        }

        public void DeleteDirectionFromRegistrationTrip(object selectedItem)
        {
            if (this.Direction == null)
                return;
            if (selectedItem != null)
                this.Direction.Remove((Direction)selectedItem);
            
        }

        public void AddDirectionToRegistrationTrip()
        {
            if (this.direction == null)
                this.Direction = new ObservableCollection<Direction>();
            this.Direction.Add(new ViewModels.Direction()
            {
                Name = this.inputDirection,
                DeleteDirectionCommand=new Command(DeleteDirectionFromRegistrationTrip)
            });
            this.InputDirection = "";
        }

        public void EditTripFood(object food)
        {
            var foodInfo = food as Food;
            this.FoodName = foodInfo.Name;
            this.MeasurementUnit = foodInfo.MeasurementUnit;
            this.MeasureOfTheFood = foodInfo.Measure;
            DeleteFoodFromFoods(food);
        }

        public void AddFoodToRegistrationTripFoods()
        {
            this.RegistrationFood = new Food()
            {
                Name = this.FoodName,
                Measure = this.MeasureOfTheFood,
                MeasurementUnit = this.MeasurementUnit,
                EditFoodCommand = new Command(EditTripFood),
                DeleteFoodFromFoodsCommand=new Command(DeleteFoodFromFoods)
            };
            this.Foods.Add(this.registrationFood);

            this.FoodName = "";
            this.MeasureOfTheFood = 0;
        }

        public void DeleteFoodFromFoods(object food)
        {
            if (this.Foods == null || food == null)
                return;
            this.Foods.Remove((Food)food);
        }

        public bool CanAddFoodToTripFoods()
        {
            if (this.FoodName == null || this.FoodName == "" || this.MeasureOfTheFood == 0) return false;


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

        public void RegistrationTrip()
        {
            var registrationTrip = new CampingTripInfo()
            {
                Place = this.tripRegistration.Place,
                DepartureDate = this.tripRegistration.DepartureDate,
                ArrivalDate = this.tripRegistration.ArrivalDate,
                MinAge = this.tripRegistration.MinAge,
                MaxAge = this.tripRegistration.MaxAge,
                CountOfMembers = this.tripRegistration.CountOfMembers,
                HasGuide = this.tripRegistration.HasGuide,
                HasPhotographer = this.tripRegistration.HasPhotographer,
                TypeOfTrip = this.tripRegistration.TypeOfTrip,
                OrganizationType = DataModel.TypeOfOrganization.OrderByUser,
                Organizer = this.user
            };
            registrationTrip.Direction = new List<string>();
            foreach(var elem in this.direction)
            {
                registrationTrip.Direction.Add(elem.Name);
            }
            registrationTrip.Food = new ObservableCollection<FoodInfo>();
            foreach(var food in foods)
            {
                registrationTrip.Food.Add(new FoodInfo()
                {
                    Name = food.Name,
                    Measure = food.Measure,
                    MeasurementUnit = food.MeasurementUnit
                });
            }


            var response = httpClient.PostAsync("api/CampingTrips", new StringContent(JsonConvert.SerializeObject(registrationTrip), Encoding.UTF8, "application/json"));
        }

        public bool CampingTripRegistrationValidation()
        {
            if (this.tripRegistration.Place == null)
            {
                this.ErrorMessage = "Input the place name.";
                return false;
            }
            if (this.tripRegistration.DepartureDate < DateTime.Now)
            {
                this.ErrorMessage = "Choose a correct departure date.";
                return false;
            }
            if (this.tripRegistration.ArrivalDate < this.tripRegistration.ArrivalDate)
            {
                this.ErrorMessage = "Choose a correct arrival date.";
                return false;
            }
            return true;
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class Direction
    {
        public string Name { get; set; }
        public ICommand DeleteDirectionCommand { get; set; }
    }

    

    public class Food
    {
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public double Measure { get; set; }
        public ICommand EditFoodCommand { get; set; }
        public ICommand DeleteFoodFromFoodsCommand { get; set; }
    }

    public enum TypeOfCampingTrip
    {
        Excursion,
        Campaign,
        CampingTrip
    }
}
