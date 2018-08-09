using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagement.DataAccesLayer;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.Verification;
using UserManagement.Validation;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/photographer")]
    public class PhotographerController : Controller
    {
        private readonly DataAccesLayer usersDataAccessLayer;

        public PhotographerController(DataAccesLayer users)
        {
            this.usersDataAccessLayer = users;
        }

        // GET: api/Photographer
        [Authorize]
        [HttpGet]
        public IEnumerable<PhotographerInfo> Get()
        {
            return this.usersDataAccessLayer.GetAllPhotographers();
        }

        // GET: api/Photographer/5
        [Authorize]
        [HttpGet("{id}")]
        public PhotographerInfo Get(int id)
        {
            return this.usersDataAccessLayer.GetPhotographerById(id);
        }
        
        // POST: api/Photographer

        [HttpPost]
        public Status Post([FromBody]PhotographerInfo photographer)
        {
            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(photographer.Email)) return new Status
            {
                StatusCode = 2002,
                IsOk = false,
                Message = "Email is not valid"
            };

            if (!this.usersDataAccessLayer.IsValidUserName(photographer.UserName)) return new Status
            {
                StatusCode = 2001,
                IsOk = false,
                Message = "UserName is not valid"
            };

            var id = this.usersDataAccessLayer.AddPhotographer(photographer);

            return new Status
            {
                StatusCode = 1000,
                IsOk = true,
                Message = "Your account is crated."
            };
        }
        
        // PUT: api/Photographer/5
        [Authorize(Policy ="OnlyForPhotographer")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]PhotographerInfo photographer)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.FirstOrDefault();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (!int.TryParse(userIdClaim?.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            if (id != userId || userId != photographer?.Id) return;

            this.usersDataAccessLayer.UpdatePhotographerInfo(photographer);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "OnlyForPhotographer")]
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
                StatusCode = 5003,
                Message = "Invalid id"
            };

            if (await this.usersDataAccessLayer.IsOrganaizer(id)) return new Status
            {   
                // 2100 - deleting is feiled because user is organizer
                StatusCode = 2100,
                IsOk = false,
                Message = "You can not delete your account becouse you are organizer."
            };

            this.usersDataAccessLayer.DeletePhotographer(id);

            this.usersDataAccessLayer.DeletePhotographerFromCampingTrips(id);

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
