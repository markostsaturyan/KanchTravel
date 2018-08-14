using Kanch.ProfileComponents.DataModel;
using System.Windows.Input;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class GuideRequests
    {
        public GuideInfo Guide { get; set; }
        public ICommand Accept { get; set; }
        public ICommand Ignore { get; set; }
    }
}
