using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CampingTripService.DataManagement.Model;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/ServiceRequests")]
    public class ServiceRequestsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public ServiceRequestsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // GET: api/ServiceRequests
        [Authorize(Policy ="OnlyForADGP")]
        [HttpGet]
        public async Task<IEnumerable<ServiceRequest>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetAllInProgresServiceRequestsAsync();
            }
            else
            {
                var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

                if (userIdClaim == null) throw new Exception("user_id claim not found");

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                return await campingTripRepository.GetServiceProvidersAllInProgresServiceRequests(userId);
            }
        }

        // GET: api/ServiceRequests/5
        [Authorize(Policy = "OnlyForADGP")]
        [HttpGet("{id}")]
        public async Task<ServiceRequest> Get(string id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetServiceRequestByIdAsync(id);
            }
            else
            {
                var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

                if (userIdClaim == null) throw new Exception("user_id claim not found");

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                return await campingTripRepository.GetServiceRequestByIdAndProviderIdAsync(id,userId);
            }
        }
        
        // POST: api/ServiceRequests
        [HttpPost]
        public void Post([FromBody]ServiceRequest request)
        {
            campingTripRepository.AddServiceRequestAsync(request);
        }
        
        // PUT: api/ServiceRequests/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [Authorize(Policy ="OnlyForADGP")]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                campingTripRepository.RemoveServiceRequestAsync(id);
            }
            else
            {
                var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

                if (userIdClaim == null) throw new Exception("user_id claim not found");

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                campingTripRepository.RemoveServiceRequestByIdAndProviderIdAsync(id, userId);
            }
        }
    }
}
