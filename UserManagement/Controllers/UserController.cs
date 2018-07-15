using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.DataManagnment.DataAccesLayer;
using UserManagement.DataManagnment.Security;
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
        public UserInfo Get(int id)
        {
            return this.usersDataAccessLayer.GetUserById(id);
        }

        // POST: api/User
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
        public void Put(int id, [FromBody]UserFullWithConfirmation user)
        {
            if (user.ConfirmationPassword != null)
            {
                if (user.Password != null)
                {
                    var userCurrentPassword = this.usersDataAccessLayer.GetUserPasswordById(user.Id, out string gude);

                    var confirmationHashedPassword = (user.ConfirmationPassword + gude).HashSHA1();

                    if (userCurrentPassword == confirmationHashedPassword)
                    {
                        this.usersDataAccessLayer.UpdateUserInfo(new UserFull
                        {

                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Gender = user.Gender,
                            DataOfBirth = user.DataOfBirth,
                            Email=user.Email,
                            Image=user.Image,
                            Password=user.Password,
                            PhoneNumber=user.PhoneNumber,
                            Role



                        });

                    }


                }
            }
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
