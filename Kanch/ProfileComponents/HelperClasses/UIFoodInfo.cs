using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kanch.ProfileComponents.HelperClasses
{
    public class UIFoodInfo
    {
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public double Measure { get; set; }
        public ICommand EditFoodCommand { get; set; }
        public ICommand DeleteFoodFromFoodsCommand { get; set; }
    }
}
