using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

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
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]string campingTripId)
        {
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
