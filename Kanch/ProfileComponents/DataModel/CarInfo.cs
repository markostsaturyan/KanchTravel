using System.Windows.Media;

namespace Kanch.ProfileComponents.DataModel
{
    public class CarInfo
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public int NumberOfSeats { get; set; }

        public string FuelType { get; set; }

        public ImageSource CarPicture1 { get; set; }

        public ImageSource CarPicture2 { get; set; }

        public ImageSource CarPicture3 { get; set; }

        public bool HasWiFi { get; set; }

        public bool HasMicrophone { get; set; }

        public bool HasAirConditioner { get; set; }

        public bool HasKitchen { get; set; }

        public bool HasToilet { get; set; }

        public string LicensePlate { get; set; }

    }
}
