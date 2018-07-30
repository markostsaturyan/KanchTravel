using System.Threading.Tasks;
using Kanch.DataManagement.Model.Users;
using MongoDB.Driver;

namespace Kanch.DataManagement.CampingTripBLL
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
        Task AsMember(int id,string CampingTripID);
        void RemoveMemberFromTheTrip(int id, string CampingTripID);
    }
}
