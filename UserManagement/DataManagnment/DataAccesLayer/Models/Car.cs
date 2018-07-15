using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veldrid.ImageSharp;

namespace UserManagement.DataManagnment.DataAccesLayer.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public int NumberOfSeats { get; set; }

        public string FuelType { get; set; }

        public ImageSharpTexture CarPicture1 { get; set; }

        public ImageSharpTexture CarPicture2 { get; set; }

        public ImageSharpTexture CarPicture3 { get; set; }

        public string LicensePlate { get; set; }

        public bool HasWiFi { get; set; }

        public bool HasMicrophone { get; set; }

        public bool HasAirConditioner { get; set; }

        public bool HasKitchen { get; set; }

        public bool HasToilet { get; set; }
    }
}
