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
        public void Post([FromBody]GuideFull guide)
        {
            var id = this.dataAccessLayer.AddGuide(guide);

            var emailValidator = new EmailValidation();

            if (!emailValidator.IsValidEmail(guide.Email)) return;

            var code = this.dataAccessLayer.AddUserVerification(id);

            SendVerificationLinkEmail.SendEmail(guide.Email, code);

        }
        
        // PUT: api/Guide/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]GuideFullWithConfirmation guide)
        {
            var currentGuideName = this.dataAccessLayer.GetUserNamePasswordGuideAndEmailById(guide.Id, out string guideCurrentPassword, out string guideForHashing, out string userCurrentEmail);

            if (guide.ConfirmationPassword != null)
            {
                var confirmationHashedPassword = (guide.ConfirmationPassword + guideForHashing).HashSHA1();

                if (guide.UserName != currentGuideName && guideCurrentPassword == confirmationHashedPassword && this.dataAccessLayer.UsarNameValidating(guide.UserName))
                {
                    this.dataAccessLayer.UpdateUserInfo(new GuideInfo
                    {
                        Id = guide.Id,
                        FirstName = guide.FirstName,
                        LastName = guide.LastName,
                        Gender = guide.Gender,
                        DataOfBirth = guide.DataOfBirth,
                        Email = guide.Email,
                        Image = guide.Image,
                        PhoneNumber = guide.PhoneNumber,
                        UserName = guide.UserName,
                        EducationGrade=guide.EducationGrade,
                        KnowledgeOfLanguages=guide.KnowledgeOfLanguages,
                        Places=guide.Places,
                        Profession=guide.Profession,
                        WorkExperience = guide.WorkExperience,
                    });
                }

                if (guide.Password != null)
                {
                    if (guideCurrentPassword == confirmationHashedPassword)
                    {
                        this.dataAccessLayer.UpdateUserInfo(new GuideFull
                        {
                            Id = guide.Id,
                            FirstName = guide.FirstName,
                            LastName = guide.LastName,
                            Gender = guide.Gender,
                            DataOfBirth = guide.DataOfBirth,
                            Email = guide.Email,
                            Image = guide.Image,
                            Password = guide.Password.HashSHA1(),
                            PhoneNumber = guide.PhoneNumber,
                            UserName = guide.UserName,
                            EducationGrade = guide.EducationGrade,
                            KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                            Places = guide.Places,
                            Profession = guide.Profession,
                            WorkExperience = guide.WorkExperience
                        });
                    }
                }

                var emailValidator = new EmailValidation();

                if (guide.Email != userCurrentEmail && guideCurrentPassword == confirmationHashedPassword && emailValidator.IsValidEmail(guide.Email))
                {
                    this.dataAccessLayer.UpdateUserInfo(new GuideInfo
                    {
                        Id = guide.Id,
                        FirstName = guide.FirstName,
                        LastName = guide.LastName,
                        Gender = guide.Gender,
                        DataOfBirth = guide.DataOfBirth,
                        Email = guide.Email,
                        Image = guide.Image,
                        PhoneNumber = guide.PhoneNumber,
                        UserName = guide.UserName,
                        EducationGrade = guide.EducationGrade,
                        KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                        Places = guide.Places,
                        Profession = guide.Profession,
                        WorkExperience = guide.WorkExperience
                    });
                }
            }
            else
            {
                if (currentGuideName != guide.UserName || userCurrentEmail != guide.Email || guideCurrentPassword != guide.Password) return;

                this.dataAccessLayer.UpdateUserInfo(new GuideInfo
                {
                    Id = guide.Id,
                    FirstName = guide.FirstName,
                    LastName = guide.LastName,
                    Gender = guide.Gender,
                    DataOfBirth = guide.DataOfBirth,
                    Email = guide.Email,
                    Image = guide.Image,
                    PhoneNumber = guide.PhoneNumber,
                    UserName = guide.UserName,
                    EducationGrade = guide.EducationGrade,
                    KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                    Places = guide.Places,
                    Profession = guide.Profession,
                    WorkExperience = guide.WorkExperience
                });
            }
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
