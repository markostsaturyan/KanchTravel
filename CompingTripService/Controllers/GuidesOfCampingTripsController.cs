using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/GuidesOfCampingTrips")]
    public class GuidesOfCampingTripsController : Controller
    {

        private readonly ISignUpForTheTrip signUpForTheTrip;

        public GuidesOfCampingTripsController(ISignUpForTheTrip signUpForTheTrip)
        {
            this.signUpForTheTrip = signUpForTheTrip;
        }

        // PUT: api/GuidesOfCampingTrips/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody]string campingTripID)
        {
            await this.signUpForTheTrip.AsGuide(id, campingTripID);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async void Delete(string campingTripId)
        {
            await this.signUpForTheTrip.RemoveGuideFromTheTrip(campingTripId);
        }
    }
}
