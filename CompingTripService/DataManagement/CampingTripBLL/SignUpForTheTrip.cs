using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class SignUpForTheTrip : ISignUpForTheTrip
    {
        [Authorize(Policy ="Only For Drivers")]
        public Task AsDriver(int id,string CampingTripID)
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy ="Only For Guides")]
        public Task AsGuide(int id,string CampingTripID)
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy ="Only For Photographers")]
        public Task AsPhotographer(int id,string CampingTripID)
        {
            throw new NotImplementedException();
        }

        [Authorize(Policy ="Users")]
        public Task AsMember(int id,string CampingTripID)
        {
            throw new NotImplementedException();
        }

    }
}
