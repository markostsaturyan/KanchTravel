using IdentityModel.Client;
using Kanch.Commands;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kanch.ProfileComponents
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties and fields

        private int userId;

        private string accessToken;

        public int ErrorCode;

        public string ErrorMessage;

        
        

        

        

        

        #endregion

        #region Commands

        ICommand JoinToTrip;

        #endregion

        /*public ProfileViewModel()
        {
            ConnectToServerAndGettingAccessTokenAsync();

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]),
            };

            httpClient.SetBearerToken(this.accessToken);

            userId = Convert.ToInt32(ConfigurationSettings.AppSettings["userId"]);

            JoinToTrip = new Command(JoinToTripAsync);

        }*/

        

        

        

        

       

        private async void ConnectToServerAndGettingAccessTokenAsync()
        {
            var disco = await DiscoveryClient.GetAsync(ConfigurationSettings.AppSettings["authenticationService"]);

            TokenClient tokenClient;

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

            var response = await tokenClient.RequestRefreshTokenAsync(ConfigurationSettings.AppSettings["refreshToken"]);

            this.accessToken = response.AccessToken;
        }

        
    }
}
