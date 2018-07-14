using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.DataManagnment.DataAccesLayer;
using Microsoft.AspNetCore.Authorization;
using UserManagement.Verification;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private UsersDataAccesLayer usersDataAccessLayer;

        public UserController(UsersDataAccesLayer usersDataAccesLayer)
        {
            this.usersDataAccessLayer = usersDataAccesLayer;
        }

        // GET: api/User/5
        [Authorize(Policy ="OnlyForUser")]
        [HttpGet("{id}", Name = "Get")]
        public UserFull Get(int id)
        {
            return this.usersDataAccessLayer.GetUserById(id);
        }

        // POST: api/User
        [Authorize(Policy = "OnlyForUser")]
        [HttpPost]
        public void Post([FromBody]UserFull user)
        {
            var id = this.usersDataAccessLayer.AddUser(user);

            var code = this.usersDataAccessLayer.AddUserVerification(id);

            SendVerificationLinkEmail.SendEmail(user.Email, code);

        }

        // PUT: api/User/5
        [Authorize(Policy = "OnlyForUser")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]UserFull user)
        {
            this.usersDataAccessLayer.UpdateUserFullInfo(user);
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "OnlyForUser")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.usersDataAccessLayer.DeleteUser(id);
        }
    }
}
