using Veldrid.ImageSharp;

namespace UserManagement.DAO
{
    public class DriverFull : UserFull
    {
        public Car Car { get; set; }

        public ImageSharpTexture DrivingLicencePicFront { get; set; }

        public ImageSharpTexture DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Raiting { get; set; }
    }
}
