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
    [Route("api/PhotographersOfCampingTrips")]
    public class PhotographersOfCampingTripsController : Controller
    {
        private readonly ISignUpForTheTrip signUpForTheTrip;

        public PhotographersOfCampingTripsController(ISignUpForTheTrip signUpForTheTrip)
        {
            this.signUpForTheTrip = signUpForTheTrip;
        }

        // PUT: api/PhotographersOfCampingTrips/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody]string campingTripID)
        {
            await this.signUpForTheTrip.AsPhotographer(id, campingTripID); 
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
