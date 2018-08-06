using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
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

        //[Authorize(Policy ="For Admin")]
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetAllRegistartionNotCompletedCampingTripsAsync();
            }
            else
            {
                return await campingTripRepository.GetAllRegistartionNotCompletedCampingTripsForUserAsync();
            }
        }

        // GET: api/CampingTrips/5
        [HttpGet("{id}")]
        public async Task<CampingTripFull> Get(string id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetCampingTripAsync(id);
            }
            else
            {
                return await campingTripRepository.GetCampingTripForUserAsync(id);
            }
        }

        // POST: api/CampingTrips
        [Authorize]
        [HttpPost]
        public void Post([FromBody]CampingTripFull campingTrip)
        {
            campingTripRepository.AddCampingTrip(campingTrip);
        }
        
        // PUT: api/CampingTrips/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]CampingTripFull campingTrip)
        {
            campingTripRepository.UpdateCampingTrip(id, campingTrip);
        }

        // DELETE: api/ApiWithActions/5
        //[Authorize(Policy = "Organizer Or Admin")]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            campingTripRepository.RemoveCampingTripAsync(id);
        }
    }
}
