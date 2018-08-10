using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.DataManagement.DataAccesLayer;
using UserManagement.Verification;
using System.Net;

namespace UserManagement.Controllers
{
    [Authorize(Policy ="OnlyForAdmin")]
    [Produces("application/json")]
    [Route("api/guideverification")]
    public class GuideVerificationController : Controller
    {
        private readonly DataAccesLayer dataAccesLayer;

        public GuideVerificationController(DataAccesLayer dataAccesLayer)
        {
            this.dataAccesLayer = dataAccesLayer;
        }

        // GET: api/GuideVerification
        [HttpGet]
        public IEnumerable<GuideInfo> Get()
        {
            return this.dataAccesLayer.GetAllNonApprovedGuides();
        }

        // GET: api/GuideVerification/5
        [HttpGet("{id}")]
        public GuideInfo Get(int id)
        {
            return this.dataAccesLayer.GetNonApprovedGuideById(id);
        }
        
        // POST: api/GuideVerification
        [HttpPost]
        public void Post([FromBody]GuideInfo guide)
        {
            var code = this.dataAccesLayer.AddUserVerification(guide.UserName);

            var emailSender = new SendVerificationCodeEmail(new NetworkCredential("kanchhiking@gmail.com", "kanchhiking2018"));

            emailSender.Send(guide.Email, code.ToString());

        }

        // PUT: api/GuideVerification/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{userName}")]
        public void Delete(string userName)
        {
            this.dataAccesLayer.DeleteGuideVerification(userName);
        }
    }
}
