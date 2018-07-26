using Kanch.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kanch
{
    /// <summary>
    /// Interaction logic for Verification.xaml
    /// </summary>
    public partial class Verification : Window
    {
        private string userName;

        public Verification()
        {
            InitializeComponent();
            this.userName = userName;
        }

        private void VerifyClick(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(verification.Text,out int verificationCode))
            {
                statusMessage.Text = "The code contains only numbers, enter the correct code";
                statusMessage.Visibility = Visibility.Visible;
                return;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationSettings.AppSettings["baseUrl"]);

                var verific = new VerificationInfo
                {
                    UserName = this.userName,
                    Code = verificationCode
                };

                var verificJsonString = JsonConvert.SerializeObject(verific);

                var httpContent = new StringContent(verificJsonString, Encoding.UTF8, "application/json");

                var requestResult = client.PostAsync("/api/userverification", httpContent).Result;

                var content = requestResult.Content;

                var jsonContent = content.ReadAsStringAsync().Result;

                var status = JsonConvert.DeserializeObject<Status>(jsonContent);

                if (status.StatusCode != 1000)
                {
                    statusMessage.Text = "Invalid activatoin code";
                    statusMessage.Visibility = Visibility.Visible;
                    return;
                }

                var login = new Login();

                login.Show();

                this.Close();

                return;
            }
        }
    }
}
