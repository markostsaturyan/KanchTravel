using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.DataManagement.DataAccesLayer;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/servicesrequests")]
    public class ServicesRequestsController : Controller
    {
        private readonly DataAccesLayer dataAccesLayer;

        public ServicesRequestsController(DataAccesLayer dataAccesLayer)
        {
            this.dataAccesLayer = dataAccesLayer;
        }

        // GET: api/ServicesRequests
        [Authorize(Policy ="OnlyForAdmin")]
        [HttpGet]
        public IEnumerable<ServiceRequest> Get()
        {
            return this.dataAccesLayer.GetAllServicesRequests();
        }

        // GET: api/ServicesRequests/5
        [Authorize(Policy ="OnlyForADGP")]
        [HttpGet("{id}")]
        public IEnumerable<ServiceRequest> Get(int id)
        {
            return this.dataAccesLayer.GetServiceRequestsByUserId(id);
        }
        
        // POST: api/ServicesRequests
        [Authorize(Policy ="OnlyForAdmin")]
        [HttpPost]
        public void Post([FromBody]ServiceRequest serviceRequest)
        {
            this.dataAccesLayer.AddServiceRequest(serviceRequest);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "OnlyForAdmin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.dataAccesLayer.DeleteServicesRequests(id);
        }
    }
}
