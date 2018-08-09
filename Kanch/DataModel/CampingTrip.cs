using System;
using System.Collections.Generic;

namespace Kanch.DataModel
{
    public class CampingTrip
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

        public User Organizer { get; set; }

        public int CountOfMembers { get; set; }

        public Driver Driver { get; set; }

        public Photographer Guide { get; set; }

        public Photographer Photographer { get; set; }

        public List<Food> Food { get; set; }

        public double PriceOfTrip { get; set; }

        public bool IsRegistrationCompleted { get; set; }

        public List<Photographer> MembersOfCampingTrip { get; set; }

        public bool HasGuide { get; set; }

        public bool HasPhotographer { get; set; }
    }
}
