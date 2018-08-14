using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.HelperClasses;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace Kanch.ProfileComponents.ViewModels
{
    public class PhotographerRequestsForAdminViewModel
    {
        private TokenClient tokenClient;

        private HttpClient httpClient;

        public ObservableCollection<PhotographerRequests> PhotographerRequests { get; set; }

        public PhotographerRequestsForAdminViewModel()
        {
            this.PhotographerRequests = new ObservableCollection<PhotographerRequests>();
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["userManagementBaseUri"]);
            ConnectToServer();
            GetAllPhotographerRequests();
        }

        public void GetAllPhotographerRequests()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = httpClient.GetAsync("api/photographerverification").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var photographers = JsonConvert.DeserializeObject<List<Photographer>>(jsonContent);

            foreach (var photographer in photographers)
            {
                var photographerInfo = new PhotographerInfo
                {
                    Id = photographer.Id,
                    FirstName = photographer.FirstName,
                    LastName = photographer.LastName,
                    Gender = photographer.Gender,
                    DateOfBirth = photographer.DateOfBirth,
                    Email = photographer.Email,
                    PhoneNumber = photographer.PhoneNumber,
                    UserName = photographer.UserName,
                    Image = ImageConverter.ConvertImageToImageSource(photographer.Image),
                    KnowledgeOfLanguages = photographer.KnowledgeOfLanguages,
                    Profession = photographer.Profession,
                    WorkExperience = photographer.WorkExperience,
                    Camera = new CameraInfo
                    {
                        Id = photographer.Camera.Id,
                        IsProfessional = photographer.Camera.IsProfessional,
                        Model = photographer.Camera.Model
                    },
                    HasCameraStabilizator = photographer.HasCameraStabilizator,
                    HasDron=photographer.HasDron,
                    HasGopro=photographer.HasGopro,
                };

                var guideRequest = new PhotographerRequests()
                {
                    Photographer = photographerInfo,
                    Accept = new Command(AcceptAsync),
                    Ignore = new Command(IgnoreAsync)
                };

                this.PhotographerRequests.Add(guideRequest);
            }
        }

        public async void AcceptAsync(object photographerRequest)
        {
            var tokenResponse =await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var photRequest = photographerRequest as PhotographerRequests;

            var photographerInfo = photRequest.Photographer;

            var photographer = new Photographer()
            {
                UserName = photographerInfo.UserName,
                Email = photographerInfo.Email
            };

            var response = await httpClient.PostAsync("api/photographerverification", new StringContent(JsonConvert.SerializeObject(photographer), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                this.PhotographerRequests.Remove(photRequest);
            }
        }

        public async void IgnoreAsync(object photographerRequest)
        {
            var tokenResponse =await tokenClient.RequestRefreshTokenAsync(ConfigurationManager.AppSettings["refreshToken"]);

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var request = photographerRequest as PhotographerRequests;

            var photographer = request.Photographer;

            var response =await httpClient.DeleteAsync($"api/photographerverification/{photographer.UserName}");

            if (response.IsSuccessStatusCode)
            {
                this.PhotographerRequests.Remove(request);
            }
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
