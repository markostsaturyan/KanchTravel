using System.Windows.Input;
using Kanch.ProfileComponents.DataModel;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class UserRegisteredInProgressTrip
    {
        public CampingTripInfo CampingTrip { get; set; }
        public ICommand DeleteTrip { get; set; }
    }
}
