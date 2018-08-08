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
        Task<bool> IsOrganizerAsync(int id);
        Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredTripsAsync();
        Task<IEnumerable<ServiceRequest>> GetAllInProgresServiceRequestsAsync();
        Task<IEnumerable<CampingTripFull>> GetAllUnconfirmedTrips();
        Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredCompletedTripsAsync();
        Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredTripsForUserAsync(int userId);
        Task<IEnumerable<ServiceRequestResponse>> GetAllServiceRequestResponsesAsync();
        Task<CampingTripFull> GetUnconfirmedTripById(string campingtripId);
        Task SendingServiceRequests(CampingTripFull campingTrip);
        Task<CampingTripFull> GetCompletedCampingTripForUserAsync(string campingTripId);
        Task RemoveUserRegistredCampingTripAndSendingEmail(string campingTripId);
        Task<IEnumerable<CampingTripFull>> GetDriverTripsAsync(int userId);
        Task<CampingTripFull> GetCampingTripAsync(string id);
        Task<CampingTripFull> GetCampingTripForUserAsync(string id);
        Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredCompletedTripsForUserAsync();
        Task<IEnumerable<ServiceRequestResponse>> GetAllServiceRequestResponsesByProviderIdAsync(int userId);
        Task<IEnumerable<ServiceRequest>> GetServiceProvidersAllInProgresServiceRequests(int userId);
        Task AddCampingTripAsync(CampingTripFull item);
        Task<DeleteResult> RemoveCampingTripAsync(string id);
        Task<ReplaceOneResult> UpdateCampingTrip(string id, CampingTripFull trip);
        Task<CampingTripFull> GetUserRegisteredTripAsync(string campingTripId);
        Task<ServiceRequestResponse> GetServiceRequestResponseByIdAsync(string id);
        Task<CampingTripFull> GetUserRegisteredTripsForUserAsync(string campingTripId,int userId);
        Task UpdateUserRegistredCampingTripAsync(string campingTripId, int organizerId, CampingTripFull campingTrip);
        Task<ServiceRequest> GetServiceRequestByIdAsync(string id);
        Task<CampingTripFull> GetUserRegisteredCompletedTripAsync(string campingTripId);
        Task<CampingTripFull> GetUserRegisteredCompletedTripForUserAsync(string campingTripId);
        Task<ServiceRequestResponse> GetServiceRequestResponsesByIdAndProviderIdAsync(string id, int userId);
        Task RemoveUserRegistredCampingTripAsync(string campingTripId, int userId);
        Task<CampingTrip> GetTripAsync(string campingTripId);
        Task<ServiceRequest> GetServiceRequestByIdAndProviderIdAsync(string id, int providerId);
        Task AddServiceRequestResponceAsync(ServiceRequestResponse response);
        Task AddServiceRequestAsync(ServiceRequest request);
        Task RemoveServiceRequestAsync(string id);
        Task RemoveServiceRequestByIdAndProviderIdAsync(string id, int userId);
        Task UpdateServiceRequestResponseAsync(string id, ServiceRequestResponse response);
        Task RemoveServiceRequestResponseAsync(string id);
        Task RemoveServiceRequestResponseByIdAndProviderIdAsync(string id, int userId);
    }
}
