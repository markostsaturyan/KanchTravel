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
    class ConfirmationOfTripsViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;
        private TokenClient tokenClient;

        public ObservableCollection<CampingTripInfo> UnconfirmedTrips { get; set; }

        public ICommand SendRequestsToDriverCommand { get; set; }
        public ICommand SendRequestsToGuideCommand { get; set; }
        public ICommand SendRequestsToPhotographerCommand { get; set; }

        public ConfirmationOfTripsViewModel()
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["userManagementBaseUri"]);
            ConnectToServer();
            this.UnconfirmedTrips = new ObservableCollection<CampingTripInfo>();
        }

        public void GetAllUnconfirmedTrips()
        {
            var response = httpClient.GetAsync("api/UserRegisteredTripsManagement").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var trips = JsonConvert.DeserializeObject<List<CampingTrip>>(jsonContent);

            var campingTrips = new ObservableCollection<UnconfirmedTrips>();

            foreach(var trip in trips)
            {
                var campingtrip = new CampingTripInfo()
                {
                    Place = trip.Place,
                    DepartureDate = trip.DepartureDate,
                    ArrivalDate = trip.ArrivalDate,
                    CountOfMembers = trip.CountOfMembers,
                    MinAge = trip.MinAge,
                    MaxAge = trip.MaxAge,
                    Direction = trip.Direction
                };
                if(trip.TypeOfTrip == Kanch.DataModel.TypeOfCampingTrip.Campaign)
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
                campingtrip.Organizer = new UserInfo()
                {
                    FirstName = trip.Organzier.FirstName,
                    LastName = trip.Organzier.LastName,
                    UserName = trip.Organzier.UserName,
                    Email = trip.Organzier.Email,
                    PhoneNumber = trip.Organzier.PhoneNumber,
                    DateOfBirth = trip.Organzier.DateOfBirth,
                    Gender = trip.Organzier.Gender
                };
                if (trip.Organzier.Image != null)
                {
                    campingtrip.Organizer.Image = ImageConverter.ConvertImageToImageSource(trip.Organzier.Image);
                }
                else
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    if (campingtrip.Organizer.Gender == "Female")
                    {
                        img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/female.jpg");
                    }
                    else
                    {
                        img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/male.jpg");
                    }
                    img.EndInit();
                    campingtrip.Organizer.Image = img;
                }
                campingTrips.Add(new HelperClasses.UnconfirmedTrips()
                {
                    CampingTrip = campingtrip,
                    ConfirmCommand = new Command(Confirm),
                    IgnoreCommand = new Command(Ignore)
                });
            };
        }

        public void Confirm(object trip)
        {
            var response = httpClient.GetAsync("api/UserRegisteredTripsManagement").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var trips = JsonConvert.DeserializeObject<List<CampingTrip>>(jsonContent);
        }
        public void Ignore(object trip)
        {

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
