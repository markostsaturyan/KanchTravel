using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ICompletedCampingTripRepository
    {
        Task<IEnumerable<CompletedCampingTripFull>> GetAllCampingTrips();
        Task<CompletedCampingTripFull> GetCampingTrip(string id);
        Task AddCampingTrip(CampingTrip item);
        Task<DeleteResult> RemoveCampingTrip(string id);
        Task<ReplaceOneResult> UpdateCampingTrip(string id, CompletedCampingTrip campingTrip);
        Task<UpdateResult> UpdateComments(string id, Comment comment);
        Task DeleteComment(string campingTripId, string commentId);
        Task<UpdateResult> UpdateRaiting(string id, double raiting);
    }
}
