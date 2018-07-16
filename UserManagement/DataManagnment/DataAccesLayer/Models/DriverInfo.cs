using Veldrid.ImageSharp;

namespace UserManagement.DataManagnment.DataAccesLayer.Models
{
    public class DriverInfo : UserInfo
    {
        public Car Car { get; set; }

        public ImageSharpTexture DrivingLicencePicFront { get; set; }

        public ImageSharpTexture DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Rating { get; set; }

        public int NumberOfAppraisers { get; set; }
    }
}
