using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Mvc;

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

        // PUT: api/MembersOfCampingTrip/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]string campingTripId)
        {
            await this.signUpForTheTrip.AsMember(id, campingTripId);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id,[FromBody]string campingTripId)
        {
            this.signUpForTheTrip.RemoveMemberFromTheTrip(id, campingTripId);
        }
    }
}
