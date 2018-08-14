using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using CampingTripService.DataManagement.Model;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/TripsDriverManagemant")]
    public class TripsDriverManagemantController : Controller
    {
        private readonly ISignUpForTheTrip signUpForTheTrip;

        public TripsDriverManagemantController(ISignUpForTheTrip signUpForTheTrip)
        {
            this.signUpForTheTrip = signUpForTheTrip;
        }

        [Authorize(Policy = "OnlyForAdmin")]
        [HttpGet("{id}")]
        public async Task<Driver> Get(string id)
        {
            return await this.signUpForTheTrip.GetDriver(id);
        }

        // PUT: api/DriversOfCampingTrips/5
        [Authorize(Policy = "OnlyForAdmin")]
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody]CampingTripId campingTripID)
        {
            await this.signUpForTheTrip.AsDriver(id, campingTripID.CampingTripID);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(string campingTripId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role")?.FirstOrDefault();

            if (role?.Value == "Admin")
            {
                await this.signUpForTheTrip.RemoveDriverFromTheTrip(campingTripId);
            }
            else
            {
                var clientIdClaim = claims.Where(claim => claim.Type == "role")?.FirstOrDefault();

                if (clientIdClaim.Value == "userManagemant")
                {
                    await this.signUpForTheTrip.RemoveDriverFromTheTrip(campingTripId);
                }
            }
        }
    }
}
