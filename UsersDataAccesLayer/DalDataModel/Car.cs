using Veldrid.ImageSharp;

namespace UsersDataAccesLayer.DalDataModel
{
    public class Car
    {
        public int Id;
        public string Brand;
        public int NumberOfSeats;
        public string FuelType;
        public ImageSharpTexture CarPicture1;
        public ImageSharpTexture CarPicture2;
        public ImageSharpTexture CarPicture3;
        public string LicensePlate;
        public bool HasWiFi;
        public bool HasMicrophone;
        public bool HasAirConditioner;
        public bool HasKitchen;
        public bool HasToilet;
    }
}
