using System;
using System.Collections.Generic;

namespace Kanch.ProfileComponents.DataModel
{
    public class CampingTripInfo
    {
        public string ID { get; set; }

        public string Place { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public List<string> Direction { get; set; }

        public TypeOfCampingTrip TypeOfTrip { get; set; }

        public TypeOfOrganization OrganizationType { get; set; }

        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public int MinCountOfMembers { get; set; }

        public int MaxCountOfMembers { get; set; }

        public UserInfo Organizer { get; set; }

        public int CountOfMembers { get; set; }

        public DriverInfo Driver { get; set; }

        public GuideInfo Guide { get; set; }

        public PhotographerInfo Photographer { get; set; }

        public List<FoodInfo> Food { get; set; }

        public double PriceOfTrip { get; set; }

        public bool IsRegistrationCompleted { get; set; }

        public List<UserInfo> MembersOfCampingTrip { get; set; }
    }
}
