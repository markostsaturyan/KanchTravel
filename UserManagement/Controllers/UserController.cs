using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.DataManagement.DataAccesLayer;
using Microsoft.AspNetCore.Authorization;
using UserManagement.Verification;
using UserManagement.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Net;
using System.Threading.Tasks;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly DataAccesLayer usersDataAccessLayer;

        public UserController(DataAccesLayer usersDataAccesLayer)
        {
            this.usersDataAccessLayer = usersDataAccesLayer;
        }

        // GET: api/User
        [Authorize]
        [HttpGet]
        public IEnumerable<UserInfo> Get()
        {
            return this.usersDataAccessLayer.GetAllUsers();
        }

        // GET: api/User/5
        [Authorize]
        [HttpGet("{id}")]
        public UserInfo Get(int id)
        {
            return this.usersDataAccessLayer.GetUserById(id);
        }

        // POST: api/User
        [HttpPost]
        public Status Post([FromBody]UserInfo user)
        {
            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(user.Email)) return new Status
            {
                StatusCode = 2002,
                IsOk = false,
                Message = "Email is not valid"
            };

            if (!this.usersDataAccessLayer.IsValidUserName(user.UserName)) return new Status
            {
                StatusCode = 2001,
                IsOk = false,
                Message = "UserName is already existing"
            };

            this.usersDataAccessLayer.AddUser(user);

            var code = this.usersDataAccessLayer.AddUserVerification(user.UserName);

            var emailSender=new SendVerificationCodeEmail(new NetworkCredential("noreply.recipeverify@gmail.com","159recipe@&)"));
                
            emailSender.Send(user.Email, code.ToString());

            return new Status
            {
                StatusCode = 1000,
                IsOk = true,
                Message = "Your account is crated."
            };
        }

        // PUT: api/User/5
        [Authorize(Policy = "OnlyForUser")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]UserInfo user)
        {
            this.usersDataAccessLayer.UpdateUserInfo(user);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "OnlyForUser")]
        [HttpDelete("{id}")]
        public async Task<Status> Delete(int id)
        {
            if (await this.usersDataAccessLayer.IsOrganaizer(id)) return new Status
            {
                StatusCode = 2100,
                IsOk = false,
                Message = "You can not delete your account because you are organizer."
            };

            this.usersDataAccessLayer.DeleteUserFromCampingTripsMember(id);

            this.usersDataAccessLayer.DeleteUser(id);

            return new Status
            {
                StatusCode=1002,
                IsOk = true,
                Message = "Your account deleted."
            };
        }
    }
}