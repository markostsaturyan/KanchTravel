using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using System.Linq;
using CampingTripService.DataManagement.Model;
using System.Net.Http;

namespace CampingTripService.Controllers
{
    
    [Produces("application/json")]
    [Route("api/MembersOfCampingTrip")]
    public class MembersOfCampingTripController : Controller
    {
        private readonly ISignUpForTheTrip signUpForTheTrip;

        public MembersOfCampingTripController(ISignUpForTheTrip signUpForTheTrip)
        {
            this.signUpForTheTrip = signUpForTheTrip;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IEnumerable<string>> GetAsync(int id)
        {
            return await signUpForTheTrip.GetTripsByMemberId(id);
        }

        // PUT: api/MembersOfCampingTrip/5
        [HttpPut("{id}")]
        [Authorize(Policy = "OnlyForAUDGP")]
        public async Task<Status> Put(int id, [FromBody]CampingTripId tripId)
        {
            var campingTripId = tripId.CampingTripID;

            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.First();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            if (id != userId) return new Status
            {
                IsOk = false,
                StatusCode = 5003,
                Message = "Invalid id"
            };

            return await this.signUpForTheTrip.AsMemberAsync(id, campingTripId);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize]
        [Route("{id}/{campingTripId}")]
        [HttpDelete("{id:int},{campingTripId}")]
        public async Task<Status> Delete([FromRoute]int id, [FromRoute]string campingTripId)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.FirstOrDefault();

            if (userIdClaim == null)
            {

                var clientIdClaim = claims.Where(claim => claim.Type == "client_id")?.FirstOrDefault();

                if (clientIdClaim?.Value == "userMangemant")
                {
                    return await this.signUpForTheTrip.RemoveMemberFromTheTripAsync(id, campingTripId);
                }
                else
                {
                    throw new Exception("Invalid request");
                }

            }
            else
            {
                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                if (id != userId) return new Status
                {
                    IsOk = false,
                    StatusCode = 5003,
                    Message = "Invalid id"
                };

                return await this.signUpForTheTrip.RemoveMemberFromTheTripAsync(id, campingTripId);
            }
        }
    }
}
