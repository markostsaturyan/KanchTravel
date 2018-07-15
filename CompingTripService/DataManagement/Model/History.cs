using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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
    public class History:CampingTrip
    {
        public History(CampingTripFull campingTrip)
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
        }
        [DataMember]
        public Raiting CurrentRaiting { get; set; }
        [DataMember]
        public List<Comment> Comments { get; set; }
    }
}
