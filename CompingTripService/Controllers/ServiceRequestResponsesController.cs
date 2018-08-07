using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CampingTripService.DataManagement.CampingTripBLL;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/ServiceRequestResponses")]
    public class ServiceRequestResponsesController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public ServiceRequestResponsesController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // GET: api/ServiceRequestResponses
        [Authorize(Policy ="OnlyForADGP")]
        [HttpGet]
        public async Task<IEnumerable<ServiceRequestResponse>> Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetAllServiceRequestResponsesAsync();
            }
            else
            {
                var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

                if (userIdClaim == null) throw new Exception("user_id claim not found");

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                return await campingTripRepository.GetAllServiceRequestResponsesByProviderIdAsync(userId);
            }
        }

        // GET: api/ServiceRequestResponses/5
        [Authorize(Policy = "OnlyForADGP")]
        [HttpGet("{id}")]
        public async Task<ServiceRequestResponse> Get(string id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                return await campingTripRepository.GetServiceRequestResponseByIdAsync(id);
            }
            else
            {
                var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

                if (userIdClaim == null) throw new Exception("user_id claim not found");

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                return await campingTripRepository.GetServiceRequestResponsesByIdAndProviderIdAsync(id,userId);
            }
        }

        // POST: api/ServiceRequestResponses
        [Authorize(Policy = "OnlyForDGP")]
        [HttpPost]
        public void Post([FromBody]ServiceRequestResponse response)
        {
           campingTripRepository.AddServiceRequestResponceAsync(response);
        }

        // PUT: api/ServiceRequestResponses/5
        [Authorize(Policy = "OnlyForDGP")]
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody]ServiceRequestResponse response)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            if (userId != response.ProviderId) return;

            await campingTripRepository.UpdateServiceRequestResponseAsync(id, response);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "OnlyForADGP")]
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            var role = claims.Where(claim => claim.Type == "role").First();

            if (role.Value == "Admin")
            {
                await campingTripRepository.RemoveServiceRequestResponseAsync(id);
            }
            else
            {
                var userIdClaim = claims.Where(claim => claim.Type == "user_id").First();

                if (userIdClaim == null) throw new Exception("user_id claim not found");

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new Exception("Invalid value for user_id in users claims");
                }

                await campingTripRepository.RemoveServiceRequestResponseByIdAndProviderIdAsync(id, userId);
            }

        }
    }
}
