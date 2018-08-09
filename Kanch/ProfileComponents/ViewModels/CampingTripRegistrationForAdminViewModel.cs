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
using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.HelperClasses;
using Newtonsoft.Json;

namespace Kanch.ProfileComponents.ViewModels
{
    class CampingTripRegistrationForAdminViewModel:INotifyPropertyChanged
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

        private ObservableCollection<DirectionDetails> direction;

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

        public ObservableCollection<DirectionDetails> Direction
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
        private ObservableCollection<UIFoodInfo> foods;
        private UIFoodInfo registrationFood;
        private double price;

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

        public  double Price
        {
            get { return this.price; }
            set
            {
                if (this.price != value)
                {
                    this.price = value;
                    NotifyPropertyChanged("Price");
                }
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

        public ObservableCollection<UIFoodInfo> Foods
        {
            get { return this.foods; }
            set
            {
                this.foods = value;
                NotifyPropertyChanged("Foods");
            }
        }

        public UIFoodInfo RegistrationFood
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



        public CampingTripRegistrationForAdminViewModel()
        {
            this.AddDirectionCommand = new Command(o => AddDirectionToRegistrationTrip());
            this.AddFoodToTripFoodsCommand = new Command(o => AddFoodToRegistrationTripFoods(), o => CanAddFoodToTripFoods());
            this.RegisterTripCommand = new Command(o => RegistrationTrip());


            this.direction = new ObservableCollection<DirectionDetails>();
            this.foods = new ObservableCollection<UIFoodInfo>();
            this.tripRegistration = new CampingTripInfo();
            this.tripRegistration.DepartureDate = DateTime.Now;
            this.tripRegistration.ArrivalDate = DateTime.Now;
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            ConnectToServer();
            GetUserInfo();

        }

        public void DeleteDirectionFromRegistrationTrip(object selectedItem)
        {
            if (this.Direction == null)
                return;
            if (selectedItem != null)
                this.Direction.Remove((DirectionDetails)selectedItem);

        }

        public void AddDirectionToRegistrationTrip()
        {
            if (this.direction == null)
                this.Direction = new ObservableCollection<DirectionDetails>();
            this.Direction.Add(new DirectionDetails()
            {
                Name = this.inputDirection,
                DeleteDirectionCommand = new Command(DeleteDirectionFromRegistrationTrip)
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
            this.RegistrationFood = new UIFoodInfo()
            {
                Name = this.FoodName,
                Measure = this.MeasureOfTheFood,
                MeasurementUnit = this.MeasurementUnit,
                EditFoodCommand = new Command(EditTripFood),
                DeleteFoodFromFoodsCommand = new Command(DeleteFoodFromFoods)
            };
            this.Foods.Add(this.registrationFood);

            this.FoodName = "";
            this.MeasureOfTheFood = 0;
        }

        public void DeleteFoodFromFoods(object food)
        {
            if (this.Foods == null || food == null)
                return;
            this.Foods.Remove((UIFoodInfo)food);
        }

        public bool CanAddFoodToTripFoods()
        {
            if (this.FoodName == null || this.FoodName == "" || this.MeasureOfTheFood == 0) return false;


            return true;
        }



        public void GetUserInfo()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            var httpCl = new HttpClient();

            httpCl.BaseAddress = new Uri(ConfigurationSettings.AppSettings["userManagementBaseUri"]);

            httpCl.SetBearerToken(tokenResponse.AccessToken);

            var response = httpCl.GetAsync("api/User/" + ConfigurationSettings.AppSettings["userId"]).Result;

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
            if (!CampingTripRegistrationValidation())
            {
                return;
            }

            var registrationTrip = new CampingTrip()
            {
                Place = this.tripRegistration.Place,
                DepartureDate = this.tripRegistration.DepartureDate,
                ArrivalDate = this.tripRegistration.ArrivalDate,
                MinAge = this.tripRegistration.MinAge,
                MaxAge = this.tripRegistration.MaxAge,
                MaxCountOfMembers = this.tripRegistration.MaxCountOfMembers,
                MinCountOfMembers = this.tripRegistration.MinCountOfMembers,
                HasGuide = this.tripRegistration.HasGuide,
                HasPhotographer = this.tripRegistration.HasPhotographer,
                OrganizationType = Kanch.DataModel.TypeOfOrganization.OrderByAdmin,
                Organizer = new User()
                {
                    FirstName = this.tripRegistration.Organizer.FirstName,
                    LastName = this.tripRegistration.Organizer.LastName,
                    DateOfBirth = this.tripRegistration.Organizer.DateOfBirth,
                    Email = this.tripRegistration.Organizer.Email,
                    Gender = this.tripRegistration.Organizer.Gender,
                    Image = Utilities.ImageConverter.ImageSourceToBytes(this.tripRegistration.Organizer.Image),
                    Id= this.tripRegistration.Organizer.Id,
                    PhoneNumber=this.tripRegistration.Organizer.PhoneNumber,
                    UserName=this.tripRegistration.Organizer.UserName
                },
                PriceOfTrip=this.price,
               
            };

            if (this.tripRegistration.TypeOfTrip == ProfileComponents.DataModel.TypeOfCampingTrip.Campaign)
            {
                registrationTrip.TypeOfTrip = Kanch.DataModel.TypeOfCampingTrip.Campaign;
            }
            else if (this.tripRegistration.TypeOfTrip == ProfileComponents.DataModel.TypeOfCampingTrip.CampingTrip)
            {
                registrationTrip.TypeOfTrip = Kanch.DataModel.TypeOfCampingTrip.CampingTrip;
            }
            else
            {
                registrationTrip.TypeOfTrip = Kanch.DataModel.TypeOfCampingTrip.Excursion;
            }

            

            registrationTrip.Direction = new List<string>();
            foreach (var elem in this.direction)
            {
                registrationTrip.Direction.Add(elem.Name);
            }
            registrationTrip.Food = new List<Food>();
            foreach (var food in foods)
            {
                registrationTrip.Food.Add(new Food()
                {
                    Name = food.Name,
                    Measure = food.Measure,
                    MeasurementUnit = food.MeasurementUnit
                });
            }

            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var trip = JsonConvert.SerializeObject(registrationTrip);

            var response = httpClient.PostAsync("api/CampingTrips", new StringContent(trip, Encoding.UTF8, "application/json")).Result;
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
            if (this.price == 0)
            {
                this.ErrorMessage = "Input a price.";
                return false;
            }
            return true;
        }
        private void ConnectToServer()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationSettings.AppSettings["authenticationService"]).Result;

            if (disco.IsError)
            {
                ErrorMessage = disco.Error;

                return;
            }
            else
            {
                tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }


        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
