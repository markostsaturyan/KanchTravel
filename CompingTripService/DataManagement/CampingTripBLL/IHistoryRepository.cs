using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface IHistoryRepository
    {
        Task<IEnumerable<History>> GetAllCampingTrips();
        Task<History> GetCampingTrip(string id);
        Task AddCampingTrip(CampingTrip item);
        Task<DeleteResult> RemoveCampingTrip(string id);
        Task<ReplaceOneResult> UpdateCampingTrip(string id, History campingTrip);
        Task<UpdateResult> UpdateComments(string id, Comment comment);
        Task<UpdateResult> UpdateRaiting(string id, double raiting);
    }
}
