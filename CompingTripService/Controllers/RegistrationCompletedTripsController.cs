using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/RegistrationCompletedTrips")]
    public class RegistrationCompletedTripsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public RegistrationCompletedTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // GET: api/RegistrationCompletedTrips
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetAllRegistartionCompletedCampingTripsAsync();
            }
            else
            {
                return await campingTripRepository.GetAllRegistartionCompletedCampingTripsForUserAsync();
            }

        }

        // PUT: api/RegistrationCompletedTrips/5
        [Authorize(Policy ="OnlyForAdmin")]
        [HttpPut("{tripId}")]
        public async void Put(string tripId, [FromBody]CampingTripFull campingTrip)
        {
            await campingTripRepository.UpdateCampingTrip(tripId,campingTrip);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "OnlyForAdmin")]
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await campingTripRepository.RemoveCampingTripAsync(id);
        }
    }
}
