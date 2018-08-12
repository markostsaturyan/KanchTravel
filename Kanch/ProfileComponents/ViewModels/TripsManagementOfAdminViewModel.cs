using IdentityModel.Client;
using Kanch.ProfileComponents.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using Kanch.ProfileComponents.DataModel;
using Kanch.DataModel;
using System.Collections.ObjectModel;
using Kanch.ProfileComponents.Utilities;
using System.Windows.Media.Imaging;
using System.Linq;
using Kanch.Commands;
using System.Threading.Tasks;

namespace Kanch.ProfileComponents.ViewModels
{
    public class TripsManagementOfAdminViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TokenClient tokenClient; 
        private HttpClient httpClient;

        public ObservableCollection<ResponseOfTrip> Responses { get; set; }

        public TripsManagementOfAdminViewModel()
        {
            ConnectToServer();

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["baseUrl"])
            };

            Responses = new ObservableCollection<ResponseOfTrip>();
            GetTripsAndResponsesAsync();
        }

        public  async void GetTripsAndResponsesAsync()
        {
            var refreshToken = ConfigurationManager.AppSettings["refreshToken"];

            var tokenResponse = tokenClient.RequestRefreshTokenAsync(refreshToken).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var httpResponse = httpClient.GetAsync("api/ServiceRequestResponses").Result;

            var responsesJson = httpResponse.Content.ReadAsStringAsync().Result;

            var responses = JsonConvert.DeserializeObject<List<ServiceRequestResponse>>(responsesJson);

            foreach(var response in responses)
            {
                var res = Responses.Where(tripResp => tripResp.CampingTrip.ID == response.CampingTripId)?.FirstOrDefault();

                if (res == null)
                {
                    var tripRes = new ResponseOfTrip(tokenClient);

                    tripRes.CampingTrip = GetTrip(response.CampingTripId, refreshToken);

                    tripRes.AcceptTrip = new Command(AcceptTripAsync);

                    var user = await GetUserAsync(response.ProviderId);

                    response.FirstName = user.FirstName;
                    response.LastName = user.LastName;
                    response.Email = user.Email;

                    tripRes.AddServiceRequestResponce(response);

                    Responses.Add(tripRes);
                }
                else
                {
                    var user = await GetUserAsync(response.ProviderId);

                    response.FirstName = user.FirstName;
                    response.LastName = user.LastName;
                    response.Email = user.Email;

                    res.AddServiceRequestResponce(response);
                }
            }
        }

        private CampingTripInfo GetTrip(string campingTripId, string refreshToken)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(refreshToken).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync($"api/CampingTrips/{campingTripId}").Result;

            var tripJson = response.Content.ReadAsStringAsync().Result;

            var trip = JsonConvert.DeserializeObject<CampingTrip>(tripJson);

            var tripInfo = new CampingTripInfo()
            {
                ID = trip.ID,
                ArrivalDate = trip.ArrivalDate,
                DepartureDate = trip.DepartureDate,
                Direction = trip.Direction,
                CountOfMembers = trip.CountOfMembers,
                MinAge = trip.MinAge,
                MaxAge = trip.MaxAge,
                MinCountOfMembers = trip.MinCountOfMembers,
                MaxCountOfMembers = trip.MaxCountOfMembers,
                HasGuide = trip.HasGuide,
                HasPhotographer = trip.HasPhotographer,
                PriceOfTrip = trip.PriceOfTrip
            };
            if(trip.OrganizationType == Kanch.DataModel.TypeOfOrganization.OrderByAdmin)
            {
                tripInfo.OrganizationType = DataModel.TypeOfOrganization.OrderByAdmin;
            }
            else
            {
                tripInfo.OrganizationType = DataModel.TypeOfOrganization.OrderByUser;
            }

            if (trip.TypeOfTrip == Kanch.DataModel.TypeOfCampingTrip.Campaign)
            {
                tripInfo.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.Campaign;
            }
            else if (trip.TypeOfTrip == Kanch.DataModel.TypeOfCampingTrip.CampingTrip)
            {
                tripInfo.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.CampingTrip;
            }
            else
            {
                tripInfo.TypeOfTrip = ProfileComponents.DataModel.TypeOfCampingTrip.Excursion;
            }
            if (trip.Food != null)
            {
                tripInfo.Food = new ObservableCollection<FoodInfo>();
                foreach (var food in trip.Food)
                {
                    tripInfo.Food.Add(new FoodInfo()
                    {
                        Name = food.Name,
                        Measure = food.Measure,
                        MeasurementUnit = food.MeasurementUnit
                    });
                }
            }
            tripInfo.Organizer = new UserInfo()
            {
                FirstName = trip.Organizer.FirstName,
                LastName = trip.Organizer.LastName,
                UserName = trip.Organizer.UserName,
                Email = trip.Organizer.Email,
                PhoneNumber = trip.Organizer.PhoneNumber,
                DateOfBirth = trip.Organizer.DateOfBirth,
                Gender = trip.Organizer.Gender
            };
            if (trip.Organizer.Image != null)
            {
                tripInfo.Organizer.Image = ImageConverter.ConvertImageToImageSource(trip.Organizer.Image);
            }
            else
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                if (tripInfo.Organizer.Gender == "Female")
                {
                    img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/female.jpg");
                }
                else
                {
                    img.UriSource = new Uri(@"pack://application:,,,/Kanch;component/Images/male.jpg");
                }
                img.EndInit();
                tripInfo.Organizer.Image = img;
            }
            return tripInfo;
        }

        public async void AcceptTripAsync(object trip)
        {
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var campingTrip = (trip as ResponseOfTrip).CampingTrip;
            
            var tripJson = JsonConvert.SerializeObject(campingTrip);

            await httpClient.PostAsync($"api/CampingTrips/{campingTrip.ID}", new StringContent(tripJson));
        }

        private async Task<User> GetUserAsync(int userId)
        {
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            var httpClientForGetingUser = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"])
            };

            var response = await httpClientForGetingUser.GetAsync($"api/User/{userId}");

            var userJson = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<User>(userJson);

            return user;
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
                tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }
    }
}
