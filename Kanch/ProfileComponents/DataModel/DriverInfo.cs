using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kanch.ProfileComponents.DataModel
{
    public class DriverInfo:UserInfo
    {
        public CarInfo Car { get; set; }

        public ImageSource DrivingLicencePicFront { get; set; }

        public ImageSource DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Rating { get; set; }

        public int NumberOfAppraisers { get; set; }

        

    }
}
