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
    [Route("api/DriversOfCampingTrips")]
    public class TripDriverController : Controller
    {
        private readonly ISignUpForTheTrip signUpForTheTrip;

        public TripDriverController(ISignUpForTheTrip signUpForTheTrip)
        {
            this.signUpForTheTrip = signUpForTheTrip;
        }

        [HttpGet("{id}")]
        public async Task<Driver> Get(string id)
        {
            return await this.signUpForTheTrip.GetDriver(id);
        }

        // PUT: api/DriversOfCampingTrips/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody]string campingTripID)
        {
            await this.signUpForTheTrip.AsDriver(id, campingTripID);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string campingTripID)
        {
            await this.signUpForTheTrip.RemoveDriverFromTheTrip(campingTripID);
        }
    }
}
