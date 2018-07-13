using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public interface ISignUpForTheTrip
    {
        Task AsDriver(int id, string CampingTripID);
        Task AsGuide(int id, string CampingTripID);
        Task AsPhotographer(int id, string CampingTripID);
        Task AsMember(int id,string CampingTripID);
    }
}
