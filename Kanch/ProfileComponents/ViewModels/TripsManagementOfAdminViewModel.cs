using IdentityModel.Client;
using Kanch.ProfileComponents.HelperClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kanch.ProfileComponents.ViewModels
{
    public class TripsManagementOfAdminViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient;
        private TokenClient tokenClient;


    }
}
