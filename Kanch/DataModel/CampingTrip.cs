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
        public string TypeOfTrip { get; set; }
        public string OrganizationType { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int MinCountOfMembers { get; set; }
        public int MaxCountOfMembers { get; set; }
        public int OrganzierID { get; set; }
        public int CountOfMembers { get; set; }
        public int DriverID { get; set; }
        public int GuideID { get; set; }
        public int PhotographerID { get; set; }
        public List<Food> Food { get; set; }
        public double PriceOfTrip { get; set; }
        public bool IsRegistrationCompleted { get; set; }
    }
}
