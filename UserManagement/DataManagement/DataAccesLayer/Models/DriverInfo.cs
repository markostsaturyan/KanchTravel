using System.Drawing;

namespace UserManagement.DataManagement.DataAccesLayer.Models
{
    public class DriverInfo : UserInfo
    {
        public Car Car { get; set; }

        public byte[] DrivingLicencePicFront { get; set; }

        public byte[] DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Rating { get; set; }

        public int NumberOfAppraisers { get; set; }
    }
}
