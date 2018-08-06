using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampingTripService.DataManagement.Model;
using CampingTripService.DataManagement.CampingTripBLL;
using System.Security.Claims;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/DriverTrips")]
    public class DriverTripsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public DriverTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }


        // GET: api/DriverTrips
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            return await campingTripRepository.GetDriverTripsAsync(userId);
        }

        // GET: api/DriverTrips/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/DriverTrips
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/DriverTrips/5
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
