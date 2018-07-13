using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ICampingTripRepository
    {
        Task<IEnumerable<Model.CampingTrip>> GetAllCampingTrips();
        Task<Model.CampingTrip> GetCampingTrip(string id);
        Task AddCampingTrip(Model.CampingTrip item);
        Task<DeleteResult> RemoveCampingTrip(string id);
        Task<UpdateResult> UpdateCampingTrip(int id, DateTime departureTime);
    }
}
