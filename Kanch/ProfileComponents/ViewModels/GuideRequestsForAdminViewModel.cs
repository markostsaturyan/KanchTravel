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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kanch.ProfileComponents.ViewModels
{
    public class GuideRequestsForAdminViewModel
    {
        private TokenClient tokenClient;

        private HttpClient httpClient;

        public ObservableCollection<GuideRequests> GuideRequests { get; set; }

        public GuideRequestsForAdminViewModel()
        {
            this.GuideRequests = new ObservableCollection<GuideRequests>();
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["userManagementBaseUri"]);
            ConnectToServer();
            GetAllGuideRequests();
        }

        public void GetAllGuideRequests()
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            var response = httpClient.GetAsync("api/guideverification").Result;

            var content = response.Content;

            var jsonContent = content.ReadAsStringAsync().Result;

            var guides = JsonConvert.DeserializeObject<List<Guide>>(jsonContent);

            foreach (var guide in guides)
            {
                var guideInfo = new GuideInfo
                {
                    Id = guide.Id,
                    FirstName = guide.FirstName,
                    LastName = guide.LastName,
                    Gender = guide.Gender,
                    DateOfBirth = guide.DateOfBirth,
                    Email = guide.Email,
                    PhoneNumber = guide.PhoneNumber,
                    UserName = guide.UserName,
                    Image = ImageConverter.ConvertImageToImageSource(guide.Image),
                    EducationGrade = guide.EducationGrade,
                    KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                    Places = guide.Places,
                    Profession = guide.Profession,
                    WorkExperience = guide.WorkExperience
                };

                var guideRequest = new GuideRequests()
                {
                    Guide = guideInfo,
                    Accept = new Command(Accept),
                    Ignore = new Command(Ignore)
                };

                this.GuideRequests.Add(guideRequest);
            }

        }

        public void Accept(object guideRequest)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var guideInfo = (guideRequest as GuideRequests).Guide;

            var guide = new Guide()
            {
                UserName = guideInfo.UserName,
                Email = guideInfo.Email
            };

            var response = httpClient.PostAsync("api/guideverification", new StringContent(JsonConvert.SerializeObject(guide), Encoding.UTF8, "application/json")).Result;

            this.GuideRequests.Remove(guideRequest as GuideRequests);
        }

        public void Ignore(object guideRequest)
        {
            var tokenResponse = tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]).Result;

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var request = guideRequest as GuideRequests;

            var guide = request.Guide;

            var response = httpClient.DeleteAsync("api/guideverification/" + guide.UserName).Result;

            this.GuideRequests.Remove(request);
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
                tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }


    }
}
