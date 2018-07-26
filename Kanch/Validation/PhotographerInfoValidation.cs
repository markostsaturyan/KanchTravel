using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kanch.DataModel;

namespace Kanch.Validation
{
    public class PhotographerInfoValidation
    {
        private bool ProfessionValidation(string profession, ref string status)
        {
            if (profession == null)
            {
                status = "Enter your profession.";
                return false;
            }
            return true;
        }

        private bool WorkExperienceValidation(string workExperience, ref string status)
        {
            if (workExperience == null)
            {
                status = "Enter your work experience.";
                return false;
            }
            return true;
        }

        private bool CameraModelValidation(string cameraModel, ref string status)
        {
            if (cameraModel == null)
            {
                status = "Enter model of the Camera.";
                return false;
            }
            return true;
        }

        public bool PhotographerInfoValidationResult(Photographer photographer,out string status)
        {
            status = "";
            if (!ProfessionValidation(photographer.Profession, ref status))
                return false;
            else if (!WorkExperienceValidation(photographer.WorkExperience, ref status))
                return false;
            else if (!CameraModelValidation(photographer.Camera.Model, ref status))
                return false;
            return true;
        }



    }
}
