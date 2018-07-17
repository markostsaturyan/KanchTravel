using System.Collections.Generic;
using UserManagement.DataManagnment.DataAccesLayer;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.Verification;
using UserManagement.Validation;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Driver")]
    public class DriverController : Controller
    {
        private readonly DataAccesLayer usersDataAccessLayer;

        public DriverController(DataAccesLayer user)
        {
            this.usersDataAccessLayer = user;
        }

        // GET: api/Driver
        [HttpGet]
        public IEnumerable<DriverInfo> Get()
        {
            return this.usersDataAccessLayer.GetAllDrivers();
        }

        // GET: api/Driver/5
        [HttpGet("{id}", Name = "Get")]
        public DriverInfo Get(int id)
        {
            return this.usersDataAccessLayer.GetDriverById(id);
        }
        
        // POST: api/Driver
        [HttpPost]
        public void Post([FromBody]DriverInfo driver)
        {
            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(driver.Email))
                return;

            var id = this.usersDataAccessLayer.AddDriver(driver);

            var code = this.usersDataAccessLayer.AddUserVerification(id);

            SendVerificationLinkEmail.SendEmail(driver.Email, code);
        }
        
        // PUT: api/Driver/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DriverInfo driver)
        {
            this.usersDataAccessLayer.UpdateDriverInfo(driver);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.usersDataAccessLayer.DeleteDriver(id);
        }
    }
}
