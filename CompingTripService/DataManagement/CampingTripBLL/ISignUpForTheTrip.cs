using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ISignUpForTheTrip
    {
        Task<UpdateResult> AsDriver(int id, string campingTripID);
        Task<UpdateResult> RemoveDriverFromTheTrip(string campingTripID);
        Task<UpdateResult> AsGuide(int id, string campingTripID);
        Task<UpdateResult> RemoveGuideFromTheTrip(string campingTripID);
        Task<UpdateResult> AsPhotographer(int id, string campingTripID);
        Task<UpdateResult> RemovePhotographerFromTheTrip(string campingTripID);
        Task AsMember(int id,string CampingTripID);
        void RemoveMemberFromTheTrip(int id, string CampingTripID);
    }
}
