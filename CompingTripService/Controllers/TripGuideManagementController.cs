using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model.Users;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public async Task<Guide> Get(string id)
        {
            return await this.signUpForTheTrip.GetGuide(id);
        }

        // PUT: api/TripGuide/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody]string campingTripID)
        {
            await this.signUpForTheTrip.AsGuide(id, campingTripID);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async void Delete(string campingTripId)
        {
            await this.signUpForTheTrip.RemoveGuideFromTheTrip(campingTripId);
        }
    }
}
