using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagnment.DataAccesLayer;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.Verification;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Photographer")]
    public class PhotographerController : Controller
    {
        private readonly UsersDataAccesLayer usersDataAccessLayer;

        public PhotographerController(UsersDataAccesLayer usersDataAccesLayer)
        {
            this.usersDataAccessLayer = usersDataAccessLayer;
        }

        // GET: api/Photographer
        [HttpGet]
        public IEnumerable<PhotographerFull> Get()
        {
            return this.usersDataAccessLayer.GetAllPhotographers();
        }

        // GET: api/Photographer/5
        [HttpGet("{id}", Name = "Get")]
        public PhotographerFull Get(int id)
        {
            return this.usersDataAccessLayer.GetPhotographerById(id);
        }
        
        // POST: api/Photographer
        [HttpPost]
        public void Post([FromBody]PhotographerFull photographer)
        {
            var id = this.usersDataAccessLayer.AddPhotographer(photographer);

            var code = this.usersDataAccessLayer.AddUserVerification(id);

            SendVerificationLinkEmail.SendEmail(photographer.Email, code);
        }
        
        // PUT: api/Photographer/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            // TODO
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.usersDataAccessLayer.DeletePhotographer(id);
        }
    }
}
