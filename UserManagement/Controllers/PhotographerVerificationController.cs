using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserManagement.DataManagement.DataAccesLayer;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.Verification;

namespace UserManagement.Controllers
{
    [Authorize(Policy ="OnlyForAdmin")]
    [Produces("application/json")]
    [Route("api/photographerVerification")]
    public class PhotographerVerificationController : Controller
    {
        private readonly DataAccesLayer dataAccesLayer;
        
        public PhotographerVerificationController(DataAccesLayer dataAccesLayer)
        {
            this.dataAccesLayer = dataAccesLayer;
        }

        // GET: api/PhotographerVerification
        [HttpGet]
        public IEnumerable<PhotographerInfo> Get()
        {
            return this.dataAccesLayer.GetAllNonApprovedPhotographers();
        }

        // GET: api/PhotographerVerification/5
        [HttpGet("{id}")]
        public PhotographerInfo Get(int id)
        {
            return this.dataAccesLayer.GetNonApprovedPhotographerById(id);
        }
        
        // POST: api/PhotographerVerification
        [HttpPost]
        public void Post([FromBody]PhotographerInfo photographer)
        {
            var code = this.dataAccesLayer.AddUserVerification(photographer.UserName);

            SendVerificationLinkEmail.SendEmail(photographer.Email, code);
        }
        
        // PUT: api/PhotographerVerification/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{userName}")]
        public void Delete(string userName)
        {
            this.dataAccesLayer.DeletePhotographerVerification(userName);
        }
    }
}
