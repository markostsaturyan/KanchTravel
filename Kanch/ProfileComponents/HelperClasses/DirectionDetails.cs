using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class DirectionDetails
    {
        public string Name { get; set; }
        public ICommand DeleteDirectionCommand { get; set; }
    }
}
