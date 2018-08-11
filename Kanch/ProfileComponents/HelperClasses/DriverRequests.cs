using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kanch.ProfileComponents.DataModel;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class DriverRequests
    {
        public DriverInfo Driver { get; set; }
        public ICommand Accept { get; set; }
        public ICommand Ignore { get; set; }
    }
}
