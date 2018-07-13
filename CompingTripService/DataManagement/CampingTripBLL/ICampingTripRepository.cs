using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ICampingTripRepository
    {
        Task<IEnumerable<CampingTrip>> GetAllCampingTrips();
        Task<Model.CampingTrip> GetCampingTrip(string id);
        Task AddCampingTrip(CampingTrip item);
        Task<DeleteResult> RemoveCampingTrip(string id);
        Task<ReplaceOneResult> UpdateCampingTrip(string id, CampingTrip trip);
        Task<UpdateResult> UpdateDepartureDate(string id, DateTime departureDate);
        Task<UpdateResult> UpdateCountOfMembers(string id, int count);
        Task<UpdateResult> UpdateDriver(string id, int driverId);
        Task<UpdateResult> UpdateGuide(string id, int guideId);
        Task<UpdateResult> UpdatePhotographer(string id, int photographerId);
    }
}
