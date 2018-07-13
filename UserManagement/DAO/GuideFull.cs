using System.Collections.Generic;

namespace UserManagement.DAO
{
    public class GuideFull : UserFull
    {
        public string Profession { get; set; }

        public string EducationGrade { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public string WorkExperience { get; set; }

        public List<string> Places { get; set; }

        public double Raiting { get; set; }

        public int IsApproved { get; set; }
    }
}
