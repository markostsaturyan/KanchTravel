using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kanch.DataModel;
using Kanch.ProfileComponents.DataModel;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class TripsInProgress
    {
        public CampingTripInfo CampingTrip { get; set; }
        public ICommand JoinCommand { get; set; }
        public ICommand RefuseCommand { get; set; }
    }
}
