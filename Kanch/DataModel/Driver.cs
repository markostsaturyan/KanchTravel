using System.Drawing;
using Newtonsoft.Json;

namespace Kanch.DataModel
{
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
