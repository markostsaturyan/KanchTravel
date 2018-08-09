using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Linq;

namespace CampingTripService.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/MembersOfCampingTrip")]
    public class MembersOfCampingTripController : Controller
    {
        private readonly ISignUpForTheTrip signUpForTheTrip;

        public MembersOfCampingTripController(ISignUpForTheTrip signUpForTheTrip)
        {
            this.signUpForTheTrip = signUpForTheTrip;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<string>> GetAsync(int id)
        {
            return await signUpForTheTrip.GetTripsByMemberId(id);
        }

        // PUT: api/MembersOfCampingTrip/5
        [Authorize(Policy ="OnlyForAUDGP")]
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]string campingTripId)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.First();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            await this.signUpForTheTrip.AsMember(id, campingTripId);
        }

        // DELETE: api/ApiWithActions/5
        [Route("api/MembersOfCampingTrip/{id:int}/{campingTripId}")]
        [HttpDelete("{id:int},{campingTripId}")]
        public Task Delete(int id,string campingTripId)
        {


            return this.signUpForTheTrip.RemoveMemberFromTheTrip(id, campingTripId);
        }
    }
}
