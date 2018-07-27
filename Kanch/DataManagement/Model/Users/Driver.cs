using System.Drawing;

namespace Kanch.DataManagement.Model.Users
{
    public class Car
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public int NumberOfSeats { get; set; }

        public string FuelType { get; set; }

        public Image CarPicture1 { get; set; }

        public Image CarPicture2 { get; set; }

        public Image CarPicture3 { get; set; }

        public string LicensePlate { get; set; }

        public bool HasWiFi { get; set; }

        public bool HasMicrophone { get; set; }

        public bool HasAirConditioner { get; set; }

        public bool HasKitchen { get; set; }

        public bool HasToilet { get; set; }
    }

    public class Driver:User
    {
        public Car Car { get; set; }

        public Image DrivingLicencePicFront { get; set; }

        public Image DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Rating { get; set; }
    }
}
