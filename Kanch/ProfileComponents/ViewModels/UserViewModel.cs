using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kanch.ProfileComponents.ViewModels
{
    public class UserViewModel: INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Commands

       

#endregion

        #region Properties and fields
        public int ErrorCode;

        public string ErrorMessage;

        private TokenClient tokenClient;

        private HttpClient httpClient;

        public UserInfo user;

        private ImageSource male;

        private ImageSource female;

        public UserInfo User
        {
            get
            {
                return this.user;
            }

            set
            {
                this.user = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User"));
            }

        }

        #endregion

        public UserViewModel()
        {
            this.male = new BitmapImage(new Uri(String.Format("Images/male.jpg"), UriKind.Relative));
            this.male.Freeze();
            this.female= new BitmapImage(new Uri(String.Format("Images/female.jpg"), UriKind.Relative));
            this.female.Freeze();
            
            
            ConnectToServerAndGettingRefreshTokenAsync();
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);
            GetUserInfo();
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
            if (user.Image != null)
            {
                userinfo.Image = ImageConverter.ConvertImageToImageSource(user.Image);
            }
            else
            {
                if (userinfo.Gender == "Female")
                {
                    userinfo.Image = this.female;
                }
                else
                {
                    userinfo.Image = this.male;
                }
            }
            this.User = userinfo;
        }

        private async void ConnectToServerAndGettingRefreshTokenAsync()
        {
            var disco = DiscoveryClient.GetAsync(ConfigurationSettings.AppSettings["authenticationService"]).Result;

            if (disco.IsError)
            {
                ErrorCode = 404;

                ErrorMessage = disco.Error;

                return;
            }
            else
            {
                tokenClient = new TokenClient(disco.TokenEndpoint, "kanchDesktopApp", "secret");
            }
        }
    }
}
