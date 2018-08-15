using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/campingtrips")]
    public class CampingTripsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public CampingTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role")?.FirstOrDefault();

            if (role?.Value == "Admin")
            {
                return await campingTripRepository.GetAllRegistartionNotCompletedCampingTripsAsync();
            }
            else
            {
                return await campingTripRepository.GetAllRegistartionNotCompletedCampingTripsForUserAsync();
            }
        }

        // GET: api/CampingTrips/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<CampingTripFull> Get(string id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role")?.FirstOrDefault();

            if (role?.Value == "Admin")
            {
                return await campingTripRepository.GetCampingTripAsync(id);
            }
            else
            {
                return await campingTripRepository.GetCampingTripForUserAsync(id);
            }
        }

        // POST: api/CampingTrips
        [Authorize(Policy ="OnlyForAdmin")]
        [HttpPost]
        public void Post([FromBody]CampingTripFull campingTrip)
        {
            campingTripRepository.AddCampingTripAsync(campingTrip);
        }

        // PUT: api/CampingTrips/5
        [Authorize(Policy = "OnlyForAdmin")]
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]CampingTripFull campingTrip)
        {
            campingTripRepository.UpdateCampingTrip(id, campingTrip);
        }

        // DELETE: api/ApiWithActions/5
        //[Authorize(Policy = "Organizer Or Admin")]
        [Authorize(Policy = "OnlyForAdmin")]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            campingTripRepository.RemoveCampingTripAsync(id);
        }
    }
}
