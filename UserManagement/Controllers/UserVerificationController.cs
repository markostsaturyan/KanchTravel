using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagement.DataAccesLayer;
using UserManagement.DataManagement.DataAccesLayer.Models;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/userverification")]
    public class UserVerificationController : Controller
    {
        private DataAccesLayer dataAccesLayer;

        public UserVerificationController(DataAccesLayer dataAccesLayer)
        {
            this.dataAccesLayer = dataAccesLayer;
        }


        [HttpPost]
        public Status Post([FromBody]KeyValuePair<string,int> userNameCode)
        {
            if (this.dataAccesLayer.CodeIsValid(userNameCode))
            {
                this.dataAccesLayer.UpdateApproveValue(userNameCode.Key, 1);

                return new Status
                {
                    StatusCode = 1000,
                    IsOk = true,
                    Message = "You are verified."
                };
            }

            return new Status
            {
                StatusCode = 2003,
                IsOk = false,
                Message = "You are no verified.Verification code is incorrect."
            };
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{userName}")]
        public void Delete(string userName)
        {
            this.dataAccesLayer.DeleteUserVerification(userName);
        }
    }
}
