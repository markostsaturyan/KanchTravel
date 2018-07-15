using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampingTripService.DataManagement.Model
{
    public class Comment
    {
        public int UserId { get; set; }
        public string Text { get; set; }
    }

    public class Raiting
    {
        public int CountOfAppraisers { get; set; }
        public double CurrentRaiting { get; set; }
    }
    [DataContract]
    public class CompletedCampingTrip
    {
        public CompletedCampingTrip() { }
        public CompletedCampingTrip(CampingTrip campingTrip)
        {
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
            this.DriverID = campingTrip.DriverID;
            this.GuideID = campingTrip.GuideID;
            this.PhotographerID = campingTrip.PhotographerID;
            this.CountOfMembers = campingTrip.CountOfMembers;
            this.Food = campingTrip.Food;
            this.PriceOfTrip = campingTrip.PriceOfTrip;
            this.OrganzierID = campingTrip.OrganzierID;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
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
        public int OrganzierID { get; set; }
        [DataMember]
        public int CountOfMembers { get; set; }
        [DataMember]
        public int DriverID { get; set; }
        [DataMember]
        public int GuideID { get; set; }
        [DataMember]
        public int PhotographerID { get; set; }
        [DataMember]
        public List<Food> Food { get; set; }
        [DataMember]
        public double PriceOfTrip { get; set; }
        [DataMember]
        public Raiting CurrentRaiting { get; set; }
        [DataMember]
        public List<Comment> Comments { get; set; }
    }
}
