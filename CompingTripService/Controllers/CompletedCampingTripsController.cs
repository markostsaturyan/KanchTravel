using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampingTripService.DataManagement.Model;
using CampingTripService.DataManagement.CampingTripBLL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/CompletedCampingTrips")]
    public class CompletedCampingTripsController : Controller
    {
        private ICampingTripRepository campingTripRepository;

        public CompletedCampingTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }


        // GET: api/CompletedCampingTrips
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetAllCompletedCampingTripsAsync();
            }
            else
            {
                var userIdClaim = claims.Where(cl => cl.Type == "user_id").First();

                if (int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid user id");
                }

                return await campingTripRepository.GetAllCompletedCampingTripsForUserAsync();
            }
        }

        // GET: api/CompletedCampingTrips/5
        [Authorize]
        [HttpGet("{campingTripId}")]
        public async Task<CampingTripFull> Get(string campingTripId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetCompletedCampingTripAsync(campingTripId);
            }
            else
            {
                return await campingTripRepository.GetCompletedCampingTripForUserAsync(campingTripId);
            }
        }
        
    }
}
