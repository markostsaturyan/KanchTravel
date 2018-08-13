using Kanch.ProfileComponents.DataModel;
using System.Windows.Input;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class PhotographerRequests
    {
        public PhotographerInfo Photographer { get; set; }
        public ICommand Accept { get; set; }
        public ICommand Ignore { get; set; }
    }
}