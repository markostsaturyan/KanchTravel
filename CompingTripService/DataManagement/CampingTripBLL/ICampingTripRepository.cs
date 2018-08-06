using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ICampingTripRepository
    {
        Task<IEnumerable<CampingTripFull>> GetAllRegistartionCompletedCampingTripsAsync();
        Task<IEnumerable<CampingTripFull>> GetAllRegistartionCompletedCampingTripsForUserAsync();
        Task<IEnumerable<CampingTripFull>> GetAllRegistartionNotCompletedCampingTripsAsync();
        Task<IEnumerable<CampingTripFull>> GetAllRegistartionNotCompletedCampingTripsForUserAsync();
        Task<IEnumerable<CampingTripFull>> GetAllCompletedCampingTripsAsync();
        Task<IEnumerable<CampingTripFull>> GetAllCompletedCampingTripsForUserAsync();
        Task<CampingTripFull> GetCompletedCampingTripAsync(string campingTripId);
        Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredTripsAsync();
        Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredCompletedTripsAsync();
        Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredTripsForUserAsync(int userId);
        Task<CampingTripFull> GetCompletedCampingTripForUserAsync(string campingTripId);
        Task<CampingTripFull> GetCampingTripAsync(string id);
        Task<CampingTripFull> GetCampingTripForUserAsync(string id);
        Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredCompletedTripsForUserAsync();
        Task AddCampingTrip(CampingTripFull item);
        Task<DeleteResult> RemoveCampingTripAsync(string id);
        Task<ReplaceOneResult> UpdateCampingTrip(string id, CampingTripFull trip);
        Task<CampingTripFull> GetUserRegisteredTripAsync(string campingTripId);
        Task<CampingTripFull> GetUserRegisteredTripsForUserAsync(string campingTripId,int userId);
        void UpdateUserRegistredCampingTrip(string campingTripId, int organizerId, CampingTripFull campingTrip);
        Task<CampingTripFull> GetUserRegisteredCompletedTripAsync(string campingTripId);
        Task<CampingTripFull> GetUserRegisteredCompletedTripForUserAsync(string campingTripId);
        void RemoveUserRegistredCampingTripAsync(string campingTripId, int userId);
        Task<CampingTrip> GetTripAsync(string campingTripId);
    }
}
