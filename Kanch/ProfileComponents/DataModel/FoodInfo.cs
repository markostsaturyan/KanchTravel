namespace Kanch.ProfileComponents.DataModel
{
    public class FoodInfo
    {
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public double Measure { get; set; }
        public double Price { get; set; }
    }

    public enum TypeOfCampingTrip
    {
        Excursion,
        Campaign,
        CampingTrip
    }

    public enum TypeOfOrganization
    {
        OrderByUser,
        OrderByAdmin
    }
}
