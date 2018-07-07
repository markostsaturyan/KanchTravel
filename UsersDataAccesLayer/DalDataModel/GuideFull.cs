using System;
using System.Collections.Generic;
using Veldrid.ImageSharp;

namespace UsersDataAccesLayer.DalDataModel
{
    public class GuideFull:UserFull
    {
        public string Profession { get; set; }

        public string EducationGrade { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public string WorkExperience { get; set; }

        public List<string> Places { get; set; }

        public double Raiting { get; set; }
    }
}
