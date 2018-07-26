﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.DataManagement.DataAccesLayer;
using UserManagement.Verification;

namespace UserManagement.Controllers
{
    [Authorize(Policy ="OnlyForAdmin")]
    [Produces("application/json")]
    [Route("api/DriverVerification")]
    public class DriverVerificationController : Controller
    {
        private readonly DataAccesLayer dataAccesLayer;

        public DriverVerificationController(DataAccesLayer dataAccesLayer)
        {
            this.dataAccesLayer = dataAccesLayer;
        }

        // GET: api/DriverVerification
        [HttpGet]
        public IEnumerable<DriverInfo> Get()
        {
            return this.dataAccesLayer.GetAllNonApprovedDrivers();
        }

        // GET: api/DriverVerification/5
        [HttpGet("{id}", Name = "Get")]
        public DriverInfo Get(int id)
        {
            return this.dataAccesLayer.GetNonApprovedDriverById(id);
        }
        
        // POST: api/DriverVerification
        [HttpPost]
        public void Post([FromBody]DriverInfo driver)
        {
            var code = this.dataAccesLayer.AddUserVerification(driver.UserName);

            SendVerificationLinkEmail.SendEmail(driver.Email, code);
        }
        
        // PUT: api/DriverVerification/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{userName}")]
        public void Delete(string userName)
        {
            dataAccesLayer.DeleteDriverVerification(userName);
        }
    }
}