using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kanch.DataModel;

namespace Kanch.Validation
{
    public class GuideInfoValidation
    {
        private bool ProfessionValidation(string profession,ref string status)
        {
            if (profession == null)
            {
                status = "Enter your profession.";
                return false;
            }
            return true;
        }

        private bool EducationGradeValidation(string educationGrade, ref string status)
        {
            if (educationGrade == null)
            {
                status = "Enter your education grade.";
                return false;
            }
            return true;
        }

        private bool WorkExperienceValidation(string workExperience,ref string status)
        {
            if (workExperience == null)
            {
                status = "Enter your work experience.";
                return false;
            }
            return true;
        }

        public bool GuideInfoValidationResult(Guide guide,out string status)
        {
            status = "";
            if (!ProfessionValidation(guide.Profession, ref status))
                return false;
            else if (!EducationGradeValidation(guide.EducationGrade, ref status))
                return false;
            else if (!WorkExperienceValidation(guide.WorkExperience, ref status))
                return false;
            return true;
        }

    }
}
