using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /*[HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetAllRegistartionCompletedCampingTrips();
            }
            else
            {

            }

        }*/

        // GET: api/RegistrationCompletedTrips/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/RegistrationCompletedTrips
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/RegistrationCompletedTrips/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
