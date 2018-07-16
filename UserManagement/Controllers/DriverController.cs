using System.Collections.Generic;
using UserManagement.DataManagnment.DataAccesLayer;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.Verification;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Driver")]
    public class DriverController : Controller
    {
        private UsersDataAccesLayer usersDataAccessLayer;

        public DriverController(UsersDataAccesLayer usersDataAccesLayer)
        {
            this.usersDataAccessLayer = usersDataAccessLayer;
        }

        // GET: api/Driver
        [HttpGet]
        public IEnumerable<DriverFull> Get()
        {
            return this.usersDataAccessLayer.GetAllDrivers();
        }

        // GET: api/Driver/5
        [HttpGet("{id}", Name = "Get")]
        public DriverFull Get(int id)
        {
            return this.usersDataAccessLayer.GetDriverById(id);
        }
        
        // POST: api/Driver
        [HttpPost]
        public void Post([FromBody]DriverFull driver)
        {
            var id = this.usersDataAccessLayer.AddDriver(driver);

            var code = this.usersDataAccessLayer.AddUserVerification(id);

            SendVerificationLinkEmail.SendEmail(driver.Email, code);
        }
        
        // PUT: api/Driver/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DriverFull driver)
        {
            //this.usersDataAccessLayer.UpdateDriverFullInfo(driver);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //this.usersDataAccessLayer.DeleteDriver(id);
        }
    }
}
