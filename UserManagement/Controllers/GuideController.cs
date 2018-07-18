using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UserManagement.DataManagnment.DataAccesLayer;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.DataManagnment.Security;
using UserManagement.Validation;
using UserManagement.Verification;

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
        [HttpGet]
        public IEnumerable<GuideInfo> Get()
        {
            return dataAccessLayer.GetAllGuides();
        }

        // GET: api/Guide/5
        [HttpGet("{id}", Name = "Get")]
        public GuideInfo Get(int id)
        {
            return this.dataAccessLayer.GetGuideById(id);
        }
        
        // POST: api/Guide
        [HttpPost]
        public void Post([FromBody]GuideInfo guide)
        {
            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(guide.Email)) return;

            var id = this.dataAccessLayer.AddGuide(guide);

            var code = this.dataAccessLayer.AddUserVerification(id);

            SendVerificationLinkEmail.SendEmail(guide.Email, code);

        }
        
        // PUT: api/Guide/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]GuideInfo guide)
        {
           
                this.dataAccessLayer.UpdateGuideInfo(guide);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.dataAccessLayer.DeleteGuide(id);

            //Ջնջել ընթացիկ արշավներից
        }
    }
}
