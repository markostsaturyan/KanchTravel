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
using System.Text;
using System.Windows;

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

                    tripRes.DriverIsSelected = Visibility.Collapsed;
                    tripRes.GuideIsSelected = Visibility.Collapsed;
                    tripRes.PhotographerIsSelected = Visibility.Collapsed;
                    
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
                Place = trip.Place,
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
                Id=trip.Organizer.Id,
                FirstName = trip.Organizer.FirstName,
                LastName = trip.Organizer.LastName,
                UserName = trip.Organizer.UserName,
                Email = trip.Organizer.Email,
                PhoneNumber = trip.Organizer.PhoneNumber,
                DateOfBirth = trip.Organizer.DateOfBirth,
                Gender = trip.Organizer.Gender,
                Image=ImageConverter.ConvertImageToImageSource(trip.Organizer.Image)??ImageConverter.DefaultProfilePicture(trip.Organizer.Gender)
            };
            

            if (trip.Driver != null)
            {
                tripInfo.Driver = new DriverInfo
                {
                    Id = trip.Driver.Id,
                    FirstName = trip.Driver.FirstName,
                    LastName = trip.Driver.LastName,
                    Email = trip.Driver.Email,
                    Gender = trip.Driver.Gender,
                    PhoneNumber = trip.Driver.PhoneNumber,
                    Image = ImageConverter.ConvertImageToImageSource(trip.Driver.Image)??ImageConverter.DefaultProfilePicture(trip.Driver.Gender),
                    DateOfBirth = trip.Driver.DateOfBirth
                };
            }

            if (trip.Guide != null)
            {
                tripInfo.Guide = new GuideInfo
                {
                    Id = trip.Guide.Id,
                    FirstName = trip.Guide.FirstName,
                    LastName = trip.Guide.LastName,
                    Email = trip.Guide.Email,
                    Gender = trip.Guide.Gender,
                    PhoneNumber = trip.Guide.PhoneNumber,
                    Image = ImageConverter.ConvertImageToImageSource(trip.Guide.Image) ?? ImageConverter.DefaultProfilePicture(trip.Guide.Gender),
                    DateOfBirth = trip.Guide.DateOfBirth
                };
            }

            if (trip.Photographer != null)
            {
                tripInfo.Photographer = new PhotographerInfo
                {
                    Id = trip.Photographer.Id,
                    FirstName = trip.Photographer.FirstName,
                    LastName = trip.Photographer.LastName,
                    Email = trip.Photographer.Email,
                    Gender = trip.Photographer.Gender,
                    PhoneNumber = trip.Photographer.PhoneNumber,
                    Image = ImageConverter.ConvertImageToImageSource(trip.Photographer.Image)??ImageConverter.DefaultProfilePicture(trip.Photographer.Gender),
                    DateOfBirth = trip.Photographer.DateOfBirth
                };
            }

            return tripInfo;
        }

        public async void AcceptTripAsync(object responseOfTrip)
        {
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var resp = responseOfTrip as ResponseOfTrip;

            var campingTrip = resp.CampingTrip;

            var trip = TripConverter(campingTrip);

            var tripJson = JsonConvert.SerializeObject(trip);

            var content = new StringContent(tripJson, Encoding.UTF8, "application/json");

            var response = httpClient.PutAsync($"api/CampingTrips/{trip.ID}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                Responses.Remove(resp);
            }
        }

        private CampingTrip TripConverter(CampingTripInfo campingTrip)
        {
            var trip = new CampingTrip()
            {

                ID = campingTrip.ID,
                Place = campingTrip.Place,
                DepartureDate = campingTrip.DepartureDate,
                ArrivalDate = campingTrip.ArrivalDate,
                MinAge = campingTrip.MinAge,
                MaxAge = campingTrip.MaxAge,
                MaxCountOfMembers = campingTrip.MaxCountOfMembers,
                MinCountOfMembers = campingTrip.MinCountOfMembers,
                HasGuide = campingTrip.HasGuide,
                HasPhotographer = campingTrip.HasPhotographer,
                OrganizationType = Kanch.DataModel.TypeOfOrganization.OrderByAdmin,
                Organizer = new User()
                {
                    Id = campingTrip.Organizer.Id
                },
                PriceOfTrip = campingTrip.PriceOfTrip,
                CountOfMembers = campingTrip.CountOfMembers,
                Direction = campingTrip.Direction,
                IsRegistrationCompleted = campingTrip.IsRegistrationCompleted,
            };

            if (campingTrip.TypeOfTrip == ProfileComponents.DataModel.TypeOfCampingTrip.Campaign)
            {
                trip.TypeOfTrip = Kanch.DataModel.TypeOfCampingTrip.Campaign;
            }
            else if (campingTrip.TypeOfTrip == ProfileComponents.DataModel.TypeOfCampingTrip.CampingTrip)
            {
                trip.TypeOfTrip = Kanch.DataModel.TypeOfCampingTrip.CampingTrip;
            }
            else
            {
                trip.TypeOfTrip = Kanch.DataModel.TypeOfCampingTrip.Excursion;
            }

            if (campingTrip.Driver != null)
            {
                trip.Driver = new Driver { Id = campingTrip.Driver.Id };
            }

            if (campingTrip.Guide != null)
            {
                trip.Guide = new Guide { Id = campingTrip.Guide.Id };
            }

            if (campingTrip.Photographer != null)
            {
                trip.Photographer = new Photographer { Id = campingTrip.Photographer.Id };
            }

            trip.MembersOfCampingTrip = new List<User>();

            if (campingTrip.MembersOfCampingTrip != null)
            {
                foreach (var member in campingTrip.MembersOfCampingTrip)
                {
                    trip.MembersOfCampingTrip.Add(new User
                    {
                        Id = member.Id
                    });
                }
            }

            if (campingTrip.Food != null)
            {
                trip.Food = new List<Food>();

                foreach (var food in campingTrip.Food)
                {
                    trip.Food.Add(new Food()
                    {
                        Name = food.Name,
                        Measure = food.Measure,
                        MeasurementUnit = food.MeasurementUnit
                    });
                }
            }
            return trip;
        }

        private async Task<User> GetUserAsync(int userId)
        {
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            var httpClientForGetingUser = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"])
            };

            httpClientForGetingUser.SetBearerToken(tokenResponse.AccessToken);

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
