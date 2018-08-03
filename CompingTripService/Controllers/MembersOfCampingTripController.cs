using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using CampingTripService.DataManagement.Model.Users;
using CampingTripService.DataManagement.Model;

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
        public List<User> Get(string id)
        {
            return this.signUpForTheTrip.GetMembersOfCampingTrip(id);
        }

        [HttpGet("{id}")]
        public List<CampingTripFull> Get(int userId)
        {
            return this.signUpForTheTrip.GetUserRegisteredCampingTrips(userId);
        }

        // PUT: api/MembersOfCampingTrip/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]string campingTripId)
        {
            await this.signUpForTheTrip.AsMember(id, campingTripId);
        }

        // DELETE: api/ApiWithActions/5
        [Route("api/MembersOfCampingTrip/{id:int}/{campingTripId:string}")]
        [HttpDelete("{id:int},{campingTripId:string}")]
        public void Delete(int id,string campingTripId)
        {
            this.signUpForTheTrip.RemoveMemberFromTheTrip(id, campingTripId);
        }
    }
}
