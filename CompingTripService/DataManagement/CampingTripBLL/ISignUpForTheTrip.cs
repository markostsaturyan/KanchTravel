using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using CampingTripService.DataManagement.Model.Users;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ISignUpForTheTrip
    {
        Task<UpdateResult> AsDriver(int id, string campingTripID);
        Task<Driver> GetDriver(string id);
        Task<UpdateResult> RemoveDriverFromTheTrip(string campingTripID);
        Task<UpdateResult> AsGuide(int id, string campingTripID);
        Task<Guide> GetGuide(string id);
        Task<UpdateResult> RemoveGuideFromTheTrip(string campingTripID);
        Task<UpdateResult> AsPhotographer(int id, string campingTripID);
        Task<Photographer> GetPhotographer(string id);
        Task<UpdateResult> RemovePhotographerFromTheTrip(string campingTripID);
        Task<Status> AsMember(int id,string CampingTripID);
        Task<Status> RemoveMemberFromTheTrip(int id, string CampingTripID);
    }
}
