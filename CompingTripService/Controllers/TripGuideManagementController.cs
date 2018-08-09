using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace CampingTripService.Controllers
{
    
    [Produces("application/json")]
    [Route("api/TripGuideManagement")]
    public class TripGuideManagementController : Controller
    {

        private readonly ISignUpForTheTrip signUpForTheTrip;

        public TripGuideManagementController(ISignUpForTheTrip signUpForTheTrip)
        {
            this.signUpForTheTrip = signUpForTheTrip;
        }

        [Authorize(Policy ="OnlyForAdmin")]
        [HttpGet("{id}")]
        public async Task<Guide> Get(string id)
        {
            return await this.signUpForTheTrip.GetGuide(id);
        }

        // PUT: api/TripGuide/5
        [Authorize(Policy ="OnlyForAdmin")]
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody]string campingTripID)
        {
            await this.signUpForTheTrip.AsGuide(id, campingTripID);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("{id}")]
        public async void Delete(string campingTripId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role")?.FirstOrDefault();

            if (role?.Value == "Admin")
            {
                await this.signUpForTheTrip.RemoveGuideFromTheTrip(campingTripId);
            }
            else
            {
                var clientIdClaim=claims.Where(claim => claim.Type == "role")?.FirstOrDefault();

                if (clientIdClaim.Value == "userManagemant")
                {
                    await this.signUpForTheTrip.RemoveGuideFromTheTrip(campingTripId);
                }
            }
        }
    }
}
