using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.DataManagnment.DataAccesLayer;
using Microsoft.AspNetCore.Authorization;
using UserManagement.Verification;
using UserManagement.Validation;
using System.Collections.Generic;

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
        [Authorize(Policy ="OnlyForAdmin")]
        [HttpGet]
        public IEnumerable<UserInfo> Get()
        {
            return this.usersDataAccessLayer.GetAllUsers();
        }

        // GET: api/User/5
        [Authorize(Policy ="OnlyForUser")]
        [HttpGet("{id}", Name = "Get")]
        public UserInfo Get(int id)
        {
            return this.usersDataAccessLayer.GetUserById(id);
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody]UserInfo user)
        {
            var id = this.usersDataAccessLayer.AddUser(user);

            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(user.Email))
                return;

            var code = this.usersDataAccessLayer.AddUserVerification(id);

            SendVerificationLinkEmail.SendEmail(user.Email, code);
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
        public void Delete(int id)
        {
            this.usersDataAccessLayer.DeleteUser(id);

            //Ջնջել յուզերին ընթացիկ արշավներից
        }
    }
}