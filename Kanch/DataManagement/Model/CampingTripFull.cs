using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Kanch.DataManagement.Model.Users;
using Kanch.DataManagement.Model.UsersDAL;

namespace Kanch.DataManagement.Model
{
    public class CampingTripFull
    {
        private readonly UsersDal usersDal;
        public CampingTripFull(CampingTrip campingTrip)
        {
            this.usersDal = new UsersDal();
            this.ID = campingTrip.ID;
            this.Place = campingTrip.Place;
            this.DepartureDate = campingTrip.DepartureDate;
            this.ArrivalDate = campingTrip.ArrivalDate;
            this.Direction = campingTrip.Direction;
            this.TypeOfTrip = campingTrip.TypeOfTrip;
            this.OrganizationType = campingTrip.OrganizationType;
            this.MinAge = campingTrip.MinAge;
            this.MaxAge = campingTrip.MaxAge;
            this.MaxCountOfMembers = campingTrip.MaxCountOfMembers;
            this.MinCountOfMembers = campingTrip.MinCountOfMembers;
            this.Driver = usersDal.GetDriver(campingTrip.DriverID);
            this.Guide = usersDal.GetGuide(campingTrip.GuideID);
            this.Photographer = usersDal.GetPhotographer(campingTrip.PhotographerID);
            this.CountOfMembers = campingTrip.CountOfMembers;
            this.Food = campingTrip.Food;
            this.IsRegistrationCompleted = campingTrip.IsRegistrationCompleted;
            this.PriceOfTrip = campingTrip.PriceOfTrip;
            this.Organzier = usersDal.GetUser(campingTrip.OrganzierID);
            this.MembersOfCampingTrip = usersDal.GetMembersOfTheCampingTrip(campingTrip.ID);
        }


        
        public string ID { get; set; }
        [DataMember]
        public string Place { get; set; }
        [DataMember]
        public DateTime DepartureDate { get; set; }
        [DataMember]
        public DateTime ArrivalDate { get; set; }
        [DataMember]
        public List<string> Direction { get; set; }
        [DataMember]
        public TypeOfCampingTrip TypeOfTrip { get; set; }
        [DataMember]
        public TypeOfOrganization OrganizationType { get; set; }
        [DataMember]
        public int MinAge { get; set; }
        [DataMember]
        public int MaxAge { get; set; }
        [DataMember]
        public int MinCountOfMembers { get; set; }
        [DataMember]
        public int MaxCountOfMembers { get; set; }
        [DataMember]
        public User Organzier { get; set; }
        [DataMember]
        public int CountOfMembers { get; set; }
        [DataMember]
        public Driver Driver { get; set; }
        [DataMember]
        public Guide Guide { get; set; }
        [DataMember]
        public Photographer Photographer { get; set; }
        [DataMember]
        public List<Food> Food { get; set; }
        [DataMember]
        public double PriceOfTrip { get; set; }
        [DataMember]
        public bool IsRegistrationCompleted { get; set; }
        [DataMember]
        public List<User> MembersOfCampingTrip { get; set; }
    }
}
