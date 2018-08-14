namespace Kanch.ProfileComponents.DataModel
{
    public class PhotographerInfo:UserInfo
    {
        public CameraInfo Camera { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public string Profession { get; set; }

        public string WorkExperience { get; set; }

        public double Rating { get; set; }

        public bool HasDron { get; set; }

        public bool HasCameraStabilizator { get; set; }

        public bool HasGopro { get; set; }

        public int NumberOfAppraisers { get; set; }
    }
}
