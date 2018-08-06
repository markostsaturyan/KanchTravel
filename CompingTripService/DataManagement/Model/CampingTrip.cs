using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampingTripService.DataManagement.Model
{
    public enum TypeOfCampingTrip
    {
        excursion,
        campaign,
        campingTrip
    }

    public enum TypeOfOrganization
    {
        orderByUser,
        orderByAdmin
    }

    public class Food
    {
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public double Measure { get; set; }
        public double Price { get; set; }
    }

    [DataContract]
    public class CampingTrip
    {
        public CampingTrip() { }

        public CampingTrip(CampingTripFull campingTrip)
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
            this.DriverID = campingTrip.Driver.Id;
            this.GuideID = campingTrip.Guide.Id;
            this.PhotographerID = campingTrip.Photographer.Id;
            this.CountOfMembers = campingTrip.CountOfMembers;
            this.Food = campingTrip.Food;
            this.IsRegistrationCompleted = campingTrip.IsRegistrationCompleted;
            this.PriceOfTrip = campingTrip.PriceOfTrip;
            this.OrganzierID = campingTrip.Organzier.Id;
            this.HasGuide = campingTrip.HasGuide;
            this.HasPhotographer = campingTrip.HasPhotographer;
            foreach(var member in campingTrip.MembersOfCampingTrip)
            {
                this.MembersOfCampingTrip.Add(member.Id);
            }
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
        public List<int> MembersOfCampingTrip { get; set; }
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
        public bool IsRegistrationCompleted { get; set; }
        [DataMember]
        public bool HasGuide { get; set; }
        [DataMember]
        public bool HasPhotographer { get; set; }
    }
}
