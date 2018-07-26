using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Kanch.DataModel
{
    public class Driver:User
    {
        public Car Car { get; set; }

        public Image DrivingLicencePicFront { get; set; }

        public Image DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Rating { get; set; }

        public int NumberOfAppraisers { get; set; }
    }
}
