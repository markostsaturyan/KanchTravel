using System.Collections.Generic;
using UserManagement.DataManagement.DataAccesLayer;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.Verification;
using UserManagement.Validation;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using System.Linq;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/driver")]
    public class DriverController : Controller
    {
        private readonly DataAccesLayer usersDataAccessLayer;

        public DriverController(DataAccesLayer user)
        {
            this.usersDataAccessLayer = user;
        }

        // GET: api/Driver
        [Authorize]
        [HttpGet]
        public IEnumerable<DriverInfo> Get()
        {
            return this.usersDataAccessLayer.GetAllDrivers();
        }

        // GET: api/Driver/5
        [Authorize]
        [HttpGet("{id}")]
        public DriverInfo Get(int id)
        {
            return this.usersDataAccessLayer.GetDriverById(id);
        }
        
        // POST: api/Driver
        [HttpPost]
        public Status Post([FromBody]DriverInfo driver)
        {
            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(driver.Email)) return new Status
            {
                StatusCode = 2002,
                IsOk = false,
                Message = "Email is not valid"
            };

            if (!this.usersDataAccessLayer.IsValidUserName(driver.UserName)) return new Status
            {
                StatusCode = 2001,
                IsOk = false,
                Message = "UserName is not valid"
            };

            this.usersDataAccessLayer.AddDriver(driver);

            return new Status
            {
                StatusCode = 1000,
                IsOk = true,
                Message = "Your account is crated."
            };
        }
        
        // PUT: api/Driver/5
        [Authorize(Policy ="OnlyForDriver")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]DriverInfo driver)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.FirstOrDefault();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (!int.TryParse(userIdClaim?.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            if (id != userId || userId!=driver?.Id) return;
            
            this.usersDataAccessLayer.UpdateDriverInfo(driver);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "OnlyForDriver")]
        [HttpDelete("{id}")]
        public async Task<Status> Delete(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.FirstOrDefault();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (!int.TryParse(userIdClaim?.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            if (id != userId) return new Status
            {
                IsOk = false,
                StatusCode=5003,
                Message="Invalid id"
            };

            if (await this.usersDataAccessLayer.IsOrganaizer(id)) return new Status
            {   
                // 2100 - deleting is feiled because user is organizer
                StatusCode = 2100,
                IsOk = false,
                Message = "You can not delete your account becouse you are organizer."
            };

            this.usersDataAccessLayer.DeleteDriver(id);

            this.usersDataAccessLayer.DeleteDriverFromCampingTrips(id);

            this.usersDataAccessLayer.DeleteUserFromCampingTripsMember(id);

            return new Status
            {
                StatusCode = 1000,
                IsOk = true,
                Message = "Your account deleted."
            };
        }
    }
}
