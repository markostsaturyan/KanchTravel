using System.Drawing;

namespace Kanch.DataModel
{
    class Driver:User
    {
        public Car Car { get; set; }

        public Image DrivingLicencePicFront { get; set; }

        public Image DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Rating { get; set; }

        public int NumberOfAppraisers { get; set; }
    }
}
