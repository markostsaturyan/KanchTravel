using System.Collections.Generic;

namespace Kanch.DataManagement.Model.Users
{
    public class Guide:User
    {
        public string Profession { get; set; }

        public string EducationGrade { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public string WorkExperience { get; set; }

        public List<string> Places { get; set; }

        public double Rating { get; set; }
    }
}
