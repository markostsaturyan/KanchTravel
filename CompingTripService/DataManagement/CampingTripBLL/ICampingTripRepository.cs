using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ICampingTripRepository
    {
        Task<IEnumerable<CampingTrip>> GetAllRegistartionCompletedCampingTrips();
        Task<IEnumerable<CampingTrip>> GetAllRegistartionNotCompletedCampingTrips();
        Task<CampingTripFull> GetCampingTrip(string id);
        Task AddCampingTrip(CampingTrip item);
        Task<DeleteResult> RemoveCampingTrip(string id);
        Task<ReplaceOneResult> UpdateCampingTrip(string id, CampingTrip trip);
        Task<UpdateResult> UpdateDepartureDate(string id, DateTime departureDate);
        Task<UpdateResult> UpdateCountOfMembers(string id, int count);
    }
}
