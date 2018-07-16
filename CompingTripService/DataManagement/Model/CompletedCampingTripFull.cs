using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model.Users;
using CampingTripService.DataManagement.Model.UsersDAL;

namespace CampingTripService.DataManagement.Model
{
    public class CommentFull
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
    }
    public class CompletedCampingTripFull
    {
        private readonly UsersDal usersDal;
        public CompletedCampingTripFull(CompletedCampingTrip campingTrip)
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
            this.PriceOfTrip = campingTrip.PriceOfTrip;
            this.Organzier = usersDal.GetUser(campingTrip.OrganzierID);
            this.MembersOfCampingTrip = usersDal.GetMembersOfTheCampingTrip(campingTrip.ID);
            this.CurrentRaiting = CurrentRaiting;
            this.Comments = usersDal.GetComments(campingTrip.Comments);
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
        public List<User> MembersOfCampingTrip { get; set; }
        [DataMember]
        public Raiting CurrentRaiting { get; set; }
        [DataMember]
        public List<CommentFull> Comments { get; set; }
    }
}
