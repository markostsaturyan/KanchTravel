using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/PhotographersOfCampingTrips")]
    public class TripPhotographerController : Controller
    {
        private readonly ISignUpForTheTrip signUpForTheTrip;

        public TripPhotographerController(ISignUpForTheTrip signUpForTheTrip)
        {
            this.signUpForTheTrip = signUpForTheTrip;
        }

        public async Task<Photographer> Get(string id)
        {
            return await this.signUpForTheTrip.GetPhotographer(id);
        }

        // PUT: api/PhotographersOfCampingTrips/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody]string campingTripID)
        {
            await this.signUpForTheTrip.AsPhotographer(id, campingTripID); 
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async void Delete(string campingTripId)
        {
            await this.signUpForTheTrip.RemovePhotographerFromTheTrip(campingTripId);
        }
    }
}
