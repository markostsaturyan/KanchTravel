using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagement.DataManagement.DataAccesLayer;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.Validation;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/guide")]
    public class GuideController : Controller
    {
        private DataAccesLayer dataAccessLayer;

        public GuideController(DataAccesLayer dataAccesLayer)
        {
            this.dataAccessLayer = dataAccesLayer;
        }

        // GET: api/Guide
        [Authorize]
        [HttpGet]
        public IEnumerable<GuideInfo> Get()
        {
            return dataAccessLayer.GetAllGuides();
        }

        // GET: api/Guide/5
        [Authorize]
        [HttpGet("{id}")]
        public GuideInfo Get(int id)
        {
            return this.dataAccessLayer.GetGuideById(id);
        }
        
        // POST: api/Guide
        [HttpPost]
        public Status Post([FromBody]GuideInfo guide)
        {
            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(guide.Email)) return new Status
            {
                StatusCode = 2002,
                IsOk = false,
                Message = "Email is not valid"
            };

            if (!this.dataAccessLayer.IsValidUserName(guide.UserName)) return new Status
            {
                StatusCode = 2001,
                IsOk = false,
                Message = "UserName is not valid"
            };

            var id = this.dataAccessLayer.AddGuide(guide);

            return new Status
            {
                StatusCode = 1000,
                IsOk = true,
                Message = "Your account is crated."
            };
        }

        // PUT: api/Guide/5
        [Authorize(Policy = "OnlyForGuide")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]GuideInfo guide)
        {
            var identity = (ClaimsIdentity)User.Identity;

            IEnumerable<Claim> claims = identity.Claims;

            var userIdClaim = claims.Where(claim => claim.Type == "user_id")?.FirstOrDefault();

            if (userIdClaim == null) throw new Exception("user_id claim not found");

            if (!int.TryParse(userIdClaim?.Value, out int userId))
            {
                throw new Exception("Invalid value for user_id in users claims");
            }

            if (id != userId || userId != guide?.Id) return;

            this.dataAccessLayer.UpdateGuideInfo(guide);
        }
        
        // DELETE: api/ApiWithActions/5
        [Authorize(Policy ="OnlyForGuide")]
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

            if (await this.dataAccessLayer.IsOrganaizer(id)) return new Status
            {   
                // 2100 - deleting is feiled because user is organizer
                StatusCode = 2100,
                IsOk = false,
                Message = "You can not delete your account becouse you are organizer."
            };

            this.dataAccessLayer.DeleteGuide(id);

            this.dataAccessLayer.DeleteGuideFromCampingTrips(id);

            this.dataAccessLayer.DeleteUserFromCampingTripsMember(id);

            return new Status
            {
                StatusCode = 1000,
                IsOk = true,
                Message = "Your account deleted."
            };
        }
    }
}
