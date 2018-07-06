using System;
using System.Collections.Generic;
using System.Text;
using Veldrid.ImageSharp;

namespace UsersDataAccesLayer.DalDataModel
{
    public class GuideFull
    {
        public int Id;
        public string FirstName;
        public string LastName;
        public DateTime DataOfBirth;
        public string PhoneNumber;
        public string Email;
        public ImageSharpTexture Image;
        public string UserName;
        public string Password;
        public string Profession;
        public string EducationGrade;
        public string KnowledgeOfLanguages;
        public string WorkExperience;
        public List<string> Places;
        public double Raiting;
    }
}
