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
    [Route("api/UserRegisteredCompletedTrips")]
    public class UserRegisteredCompletedTripsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public UserRegisteredCompletedTripsController(ICampingTripRepository campingTripRepository)
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
                var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.FirstOrDefault();

                if(!int.TryParse(userIdClaim.Value,out int userId))
                {
                    throw new System.Exception("Invalid value for claim user_id");
                }

                return await campingTripRepository.GetAllUserRegisteredCompletedTripsForUserAsync(userId);
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
                var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.FirstOrDefault();

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new System.Exception("Invalid value for claim user_id");
                }

                return await campingTripRepository.GetUserRegisteredCompletedTripForUserAsync(campingTripId,userId);
            }
        }
    }
}
