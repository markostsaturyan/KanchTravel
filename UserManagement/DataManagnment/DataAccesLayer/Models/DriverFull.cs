using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veldrid.ImageSharp;

namespace UserManagement.DataManagnment.DataAccesLayer.Models
{
    public class DriverFull:UserFull
    {
        public Car Car { get; set; }

        public ImageSharpTexture DrivingLicencePicFront { get; set; }

        public ImageSharpTexture DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Raiting { get; set; }
    }
}
