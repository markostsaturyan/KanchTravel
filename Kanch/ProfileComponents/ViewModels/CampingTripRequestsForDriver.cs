using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kanch.ProfileComponents.DataModel;
using Kanch.ProfileComponents.HelperClasses;

namespace Kanch.ProfileComponents.ViewModels
{
    class CampingTripRequestsForDriver : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CampingTripRequests> CampingTripRequests { get; set; }

        public CampingTripRequestsForDriver()
        {
            this.CampingTripRequests = new ObservableCollection<CampingTripRequests>();
        }

        public void AcceptRequest(object request)
        {

        }

        public void IgnoreRequest(object request)
        {

        }
    }
}
