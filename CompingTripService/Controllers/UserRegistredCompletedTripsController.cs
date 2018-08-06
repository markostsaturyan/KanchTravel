using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CampingTripService.DataManagement.Model;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace CampingTripService.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/UserRegistredCompletedTrips")]
    public class UserRegistredCompletedTripsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public UserRegistredCompletedTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // GET: api/UserRegistredCompletedTrips
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetAllUserRegisteredCompletedTripsAsync();
            }
            else
            {
                return await campingTripRepository.GetAllUserRegisteredCompletedTripsForUserAsync();
            }
        }

        // GET: api/UserRegistredCompletedTrips/5
        [HttpGet("{id}")]
        public async Task<CampingTripFull> Get(string campingTripId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetUserRegisteredCompletedTripAsync(campingTripId);
            }
            else
            {
                return await campingTripRepository.GetUserRegisteredCompletedTripForUserAsync(campingTripId);
            }
        }
    }
}
