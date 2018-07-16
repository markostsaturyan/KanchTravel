using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.DataManagnment.DataAccesLayer;
using UserManagement.DataManagnment.Security;
using Microsoft.AspNetCore.Authorization;
using UserManagement.Verification;
using UserManagement.Validation;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly UsersDataAccesLayer usersDataAccessLayer;

        public UserController(DataAccesLayer usersDataAccesLayer)
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

            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(user.Email)) return;

            var code = this.usersDataAccessLayer.AddUserVerification(id);

            SendVerificationLinkEmail.SendEmail(user.Email, code);
        }

        // PUT: api/User/5
        [Authorize(Policy = "OnlyForUser")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]UserFullWithConfirmation user)
        {
            var currentUserName = this.usersDataAccessLayer.GetUserNamePasswordGuideAndEmailById(user.Id, out string userCurrentPassword, out string guide, out string userCurrentEmail);

            if (user.ConfirmationPassword != null)
            {
                var confirmationHashedPassword = (user.ConfirmationPassword + guide).HashSHA1();
                
                if (user.UserName != currentUserName && userCurrentPassword==confirmationHashedPassword && this.usersDataAccessLayer.UsarNameValidating(user.UserName))
                {
                    this.usersDataAccessLayer.UpdateUserInfo(new UserInfo
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Gender = user.Gender,
                        DataOfBirth = user.DataOfBirth,
                        Email = user.Email,
                        Image = user.Image,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                    });
                }

                if (user.Password != null)
                {
                    if (userCurrentPassword == confirmationHashedPassword)
                    {
                        this.usersDataAccessLayer.UpdateUserInfo(new UserFull
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Gender = user.Gender,
                            DataOfBirth = user.DataOfBirth,
                            Email = user.Email,
                            Image = user.Image,
                            Password = user.Password.HashSHA1(),
                            PhoneNumber = user.PhoneNumber,
                            UserName = user.UserName,
                        });
                    }
                }

                var emailValidator = new EmailValidation();

                if(user.Email != userCurrentEmail && userCurrentPassword == confirmationHashedPassword && emailValidator.IsValidEmail(user.Email))
                {
                    this.usersDataAccessLayer.UpdateUserInfo(new UserInfo
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Gender = user.Gender,
                        DataOfBirth = user.DataOfBirth,
                        Email = user.Email,
                        Image = user.Image,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                    });
                }
            }
            else
            {
                if (currentUserName != user.UserName || userCurrentEmail != user.Email || userCurrentPassword != user.Password) return;

                this.usersDataAccessLayer.UpdateUserInfo(new UserInfo
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    DataOfBirth = user.DataOfBirth,
                    Email = user.Email,
                    Image = user.Image,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                });
            }
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
