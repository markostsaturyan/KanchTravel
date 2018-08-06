using System.Drawing;

namespace CampingTripService.DataManagement.Model.Users
{
    public class Car
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public int NumberOfSeats { get; set; }

        public string FuelType { get; set; }

        public byte[] CarPicture1 { get; set; }

        public byte[] CarPicture2 { get; set; }

        public byte[] CarPicture3 { get; set; }

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

        public byte[] DrivingLicencePicFront { get; set; }

        public byte[] DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Rating { get; set; }

        public int NumberOfAppraisers { get; set; }
    }
}
