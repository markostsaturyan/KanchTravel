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
        public Status Post([FromBody]VerificationInfo verification)
        {
            if (this.dataAccesLayer.CodeIsValid(verification))
            {
                this.dataAccesLayer.UpdateApproveValue(verification.UserName, true);

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
        
    }
}
