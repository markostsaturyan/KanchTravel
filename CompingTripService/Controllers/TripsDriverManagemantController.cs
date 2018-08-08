using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CampingTripService.Controllers
{
    [Authorize(Policy ="OnlyForAdmin")]
    [Produces("application/json")]
    [Route("api/TripsDriverManagemant")]
    public class TripsDriverManagemantController : Controller
    {
        private readonly ISignUpForTheTrip signUpForTheTrip;

        public TripsDriverManagemantController(ISignUpForTheTrip signUpForTheTrip)
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
