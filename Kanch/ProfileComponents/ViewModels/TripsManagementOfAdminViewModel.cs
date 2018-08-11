using IdentityModel.Client;
using Kanch.ProfileComponents.HelperClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using Kanch.ProfileComponents.DataModel;

namespace Kanch.ProfileComponents.ViewModels
{
    public class TripsManagementOfAdminViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TokenClient tokenClient;
        private HttpClient httpClient;

        public List<ResponseOfTrip> Responses { get; set; }

        public TripsManagementOfAdminViewModel()
        {
            ConnectToServer();

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["baseUrl"])
            };

            Responses = new List<ResponseOfTrip>();
            GetTripsAndResponses();
        }

        public void GetTripsAndResponses()
        {
            var refreshToken = ConfigurationManager.AppSettings["refreshToken"];

            var tokenResponse = tokenClient.RequestRefreshTokenAsync(refreshToken).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var httpResponse = httpClient.GetAsync("api/ServiceRequestResponses").Result;

            var responsesJson = httpResponse.Content.ReadAsStringAsync().Result;

            var responses = JsonConvert.DeserializeObject<List<ServiceRequestResponse>>(responsesJson);

            foreach(var response in responses)
            {
                var res = Responses.Find(tripResp => tripResp.CampingTrip.ID == response.CampingTripId);

                if (res != null)
                {
                    var tripRes = new ResponseOfTrip(tokenClient);

                    tripRes.CampingTrip = GetTrip(response.CampingTripId, refreshToken);

                    tripRes.AddServiceRequestResponce(response);

                    Responses.Add(tripRes);
                }
                else
                {
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

            return JsonConvert.DeserializeObject<CampingTripInfo>(tripJson);
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
