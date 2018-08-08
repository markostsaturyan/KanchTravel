using System.ComponentModel;
using Kanch.ProfileComponents.Utilities;

namespace Kanch.ProfileComponents.DataModel
{
    public class FoodInfo
    {
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public double Measure { get; set; }
        public double Price { get; set; }
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum TypeOfCampingTrip
    {
        [Description("Excursion")]
        Excursion,
        [Description("Campaign")]
        Campaign,
        [Description("Camping trip")]
        CampingTrip
    }

    public enum TypeOfOrganization
    {
        OrderByUser,
        OrderByAdmin
    }
}
