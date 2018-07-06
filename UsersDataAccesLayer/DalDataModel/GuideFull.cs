using System;
using System.Collections.Generic;
using Veldrid.ImageSharp;

namespace UsersDataAccesLayer.DalDataModel
{
    public class GuideFull
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DataOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ImageSharpTexture Image { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Profession { get; set; }

        public string EducationGrade { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public string WorkExperience { get; set; }

        public List<string> Places { get; set; }

        public double Raiting { get; set; }
    }
}
