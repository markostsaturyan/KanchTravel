using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagement.DataAccesLayer.Models;
using Microsoft.AspNetCore.Authorization;
using UserManagement.DataManagement.DataAccesLayer;
using System.Security.Claims;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/ServicesRequestsResponse")]
    public class ServicesRequestsResponseController : Controller
    {
        private readonly DataAccesLayer dataAccessLayer;

        public ServicesRequestsResponseController(DataAccesLayer dataAccesLayer)
        {
            this.dataAccessLayer = dataAccessLayer;
        }

        // GET: api/ServicesRequestsResponse
        [Authorize(Policy ="OnlyForAdmin")]
        [HttpGet]
        public IEnumerable<ServiceRequestResponse> Get()
        {
            return this.dataAccessLayer.GetAllServicesRequestResponses();
        }

        // GET: api/ServicesRequestsResponse/5
        [Authorize(Policy ="OnlyForAdmin")]
        [HttpGet("{campingTripId}", Name = "Get")]
        public IEnumerable<ServiceRequestResponse> Get(string campingTripId)
        {
            return this.dataAccessLayer.GetServicesRequestResponsesByCampingTripId(campingTripId);
        }

        [Authorize(Policy = "OnlyForADGP")]
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<ServiceRequestResponse> Get(int id)
        {
            return this.dataAccessLayer.GetServicesRequestResponsesByUserId(id);
        }

        // POST: api/ServicesRequestsResponse
        [Authorize(Policy ="OnlyForDGP")]
        [HttpPost]
        public void Post([FromBody]ServiceRequestResponse response)
        {
            this.dataAccessLayer.AddServiceRequestResponse(response);
        }

        // PUT: api/ServicesRequestsResponse/5
        [Authorize(Policy = "OnlyForDGP")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]ServiceRequestResponse response)
        {
            this.dataAccessLayer.UpdateServiceRequestResponse(id, response);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "OnlyForADGP")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.dataAccessLayer.DeleteServiceRequestResponse(id);
        }
    }
}
