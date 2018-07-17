using System.Drawing;
using Veldrid.ImageSharp;

namespace UserManagement.DataManagnment.DataAccesLayer.Models
{
    public class DriverInfo : UserInfo
    {
        public Car Car { get; set; }

        public Image DrivingLicencePicFront { get; set; }

        public Image DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Rating { get; set; }

        public int NumberOfAppraisers { get; set; }
    }
}
