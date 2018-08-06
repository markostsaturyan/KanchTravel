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
    [Authorize]
    [Produces("application/json")]
    [Route("api/UsersTrips")]
    public class UsersTripsController : Controller
    {
        private ICampingTripRepository campingTripRepository;

        public UsersTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // GET: api/UsersTrips
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetAllUserRegisteredTripsAsync();
            }
            else
            {
                var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

                if (userIdClaim == null) throw new Exception("user_id claim not found");

                if (int.TryParse(userIdClaim.Value,out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                return await campingTripRepository.GetAllUserRegisteredTripsForUserAsync(userId);
            }
        }

        // GET: api/UsersTrips/5
        [HttpGet("{id}")]
        public async Task<CampingTripFull> Get(string campingTripId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetUserRegisteredTripAsync(campingTripId);
            }
            else
            {
                var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

                if (userIdClaim == null) throw new Exception("user_id claim not found");

                if (int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                return await campingTripRepository.GetUserRegisteredTripsForUserAsync(campingTripId, userId);
            }
        }
        
        // POST: api/UsersTrips
        [HttpPost]
        public void Post([FromBody]CampingTripFull campingTrip)
        {
            campingTripRepository.AddCampingTrip(campingTrip);
        }
        
        // PUT: api/UsersTrips/5
        [HttpPut("{campingTripId}")]
        public void Put(string campingTripId, [FromBody]CampingTripFull campingTrip)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            campingTripRepository.UpdateUserRegistredCampingTrip(campingTripId, userId, campingTrip);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string campingTripId)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            campingTripRepository.RemoveUserRegistredCampingTripAsync(campingTripId, userId);
        }
    }
}
