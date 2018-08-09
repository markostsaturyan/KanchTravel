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
            if (campingTrip.Driver != null)
            {
                this.DriverID = campingTrip.Driver.Id;
            }
            if (campingTrip.Guide != null)
            {
                this.GuideID = campingTrip.Guide.Id;
            }
            if (campingTrip.Photographer != null)
            {
                this.PhotographerID = campingTrip.Photographer.Id;
            }
            this.CountOfMembers = campingTrip.CountOfMembers;
            this.Food = campingTrip.Food;
            this.IsRegistrationCompleted = campingTrip.IsRegistrationCompleted;
            this.PriceOfTrip = campingTrip.PriceOfTrip;
            this.OrganzierID = campingTrip.Organizer.Id;
            this.HasGuide = campingTrip.HasGuide;
            this.HasPhotographer = campingTrip.HasPhotographer;
            if (campingTrip.MembersOfCampingTrip != null)
            {
                foreach (var member in campingTrip.MembersOfCampingTrip)
                {
                    this.MembersOfCampingTrip.Add(member.Id);
                }
            }
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement]
        public string Place { get; set; }

        [BsonElement]
        public DateTime DepartureDate { get; set; }

        [BsonElement]
        public DateTime ArrivalDate { get; set; }

        [BsonElement]
        public List<string> Direction { get; set; }

        [BsonElement]
        public TypeOfCampingTrip TypeOfTrip { get; set; }

        [BsonElement]
        public TypeOfOrganization OrganizationType { get; set; }

        [BsonElement]
        public int MinAge { get; set; }

        [BsonElement]
        public int MaxAge { get; set; }

        [BsonElement]
        public int MinCountOfMembers { get; set; }

        [BsonElement]
        public int MaxCountOfMembers { get; set; }

        [BsonElement]
        public int OrganzierID { get; set; }

        [BsonElement]
        public int CountOfMembers { get; set; }

        [BsonElement]
        public List<int> MembersOfCampingTrip { get; set; }

        [BsonElement]
        public int DriverID { get; set; }

        [BsonElement]
        public int GuideID { get; set; }

        [BsonElement]
        public int PhotographerID { get; set; }

        [BsonElement]
        public List<Food> Food { get; set; }

        [BsonElement]
        public double PriceOfTrip { get; set; }

        [BsonElement]
        public bool IsRegistrationCompleted { get; set; }

        [BsonElement]
        public bool HasGuide { get; set; }

        [BsonElement]
        public bool HasPhotographer { get; set; }
    }
}
