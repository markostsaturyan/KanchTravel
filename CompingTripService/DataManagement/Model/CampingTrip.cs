using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
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
        orderByOrganizer
    }
    public enum Resrtiction
    {
        Age,
        Count,
        None
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
        [BsonId]
        public int ID { get; set; }
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
        public Resrtiction RestrictionOfTrip { get; set; }
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
    }
}
