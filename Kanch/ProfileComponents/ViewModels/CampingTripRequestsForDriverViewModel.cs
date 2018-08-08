using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.HelperClasses;
using Newtonsoft.Json;

namespace Kanch.ProfileComponents.ViewModels
{
    class CampingTripRequestsForDriverViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;
        private TokenClient tokenClient;


        public ObservableCollection<CampingTripRequests> CampingTripRequests { get; set; }

        public CampingTripRequestsForDriverViewModel()
        {
            this.CampingTripRequests = new ObservableCollection<CampingTripRequests>();
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            ConnectToServer();
            GetAllRequests();
        }

        public void GetAllRequests()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var response = httpClient.GetAsync("api/ServiceRequests").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var serviceRequests = JsonConvert.DeserializeObject<List<ServiceRequest>>(jsonContent);

            var campingTrips = new ObservableCollection<CampingTripRequests>();

            if (serviceRequests != null)
            {
                foreach (var request in serviceRequests)
                {
                    var campingTripResponse = httpClient.GetAsync("api/campingtrips/" + request.CampingTripId).Result;

                    var campingTripContent = campingTripResponse.Content;

                    var campingTripJsonContent = campingTripContent.ReadAsStringAsync().Result;

                    var trip = JsonConvert.DeserializeObject<CampingTrip>(campingTripJsonContent);
                    var campingtrip = new CampingTripRequests()
                    {
                        Place = trip.Place,
                        DepartureDate = trip.DepartureDate,
                        ArrivalDate = trip.ArrivalDate,
                        CountOfMembers = trip.CountOfMembers,
                        MinAge = trip.MinAge,
                        MaxAge = trip.MaxAge,
                        MaxCountOfMembers = trip.MaxCountOfMembers,
                        MinCountOfMembers = trip.MinCountOfMembers,
                        Direction = trip.Direction,
                    };
                    if (trip.TypeOfTrip == Kanch.DataModel.TypeOfCampingTrip.Campaign)
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

                    var tripRequest = new HelperClasses.CampingTripRequests();

                    campingtrip.Accept = new Command(AcceptRequest);
                    campingtrip.Ignore = new Command(IgnoreRequest);


                    this.CampingTripRequests.Add(campingtrip);
                }
            }
        }
        public void AcceptRequest(object request)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var tripRequest = request as CampingTripRequests;


            var serviceRequestResponse = new ServiceRequestResponse()
            {
                CampingTripId = tripRequest.ID,
                Price = tripRequest.Price,
                ProviderRole = ConfigurationSettings.AppSettings["role"],
                ProviderId = int.Parse(ConfigurationSettings.AppSettings["userId"]),
                ResponseValidityPeriod = tripRequest.ArrivalDate
            };

            var response = httpClient.PostAsync("api/ServiceRequestResponses", new StringContent(JsonConvert.SerializeObject(serviceRequestResponse), Encoding.UTF8, "application/json")).Result;

            this.CampingTripRequests.Remove(request as CampingTripRequests);
        }

        public void IgnoreRequest(object request)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var tripRequest = request as CampingTripRequests;

            var response = httpClient.DeleteAsync("api/ServiceRequestResponses/" + tripRequest.ID).Result;

            this.CampingTripRequests.Remove(request as CampingTripRequests);
        }
        private void ConnectToServer()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationSettings.AppSettings["authenticationService"]).Result;

            if (disco.IsError)
            {
                return;
            }
            else
            {
                this.tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }
    }
}
