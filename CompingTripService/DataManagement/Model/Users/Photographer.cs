namespace CampingTripService.DataManagement.Model.Users
{
    public class Camera
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public bool IsProfessional { get; set; }
    }

    public class Photographer:User
    {
        public Camera Camera { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public string Profession { get; set; }

        public string WorkExperience { get; set; }

        public double Rating { get; set; }

        public bool HasDron { get; set; }

        public bool HasCameraStabilizator { get; set; }

        public bool HasGopro { get; set; }
    }
}
