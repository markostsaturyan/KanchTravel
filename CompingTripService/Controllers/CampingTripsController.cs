﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Authorization;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/CampingTrips")]
    public class CampingTripsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public CampingTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // GET: api/CampingTrips
        //[Authorize(Policy ="For Sevak")]
        
        [HttpGet]
        public async Task<IEnumerable<CampingTrip>> Get()
        {
            return await campingTripRepository.GetAllCampingTrips();
        }

        // GET: api/CampingTrips/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<CampingTrip> Get(string id)
        {
            return await campingTripRepository.GetCampingTrip(id);
        }

        // POST: api/CampingTrips
        [Authorize(Policy = "Users")]
        [HttpPost]
        public void Post([FromBody]string value)
        {
            campingTripRepository.AddCampingTrip(ReadToObject(value));
        }
        
        // PUT: api/CampingTrips/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {
            campingTripRepository.UpdateCampingTrip(id, ReadToObject(value));
        }

       /* [HttpPut("{id}",Name ="PutDepartureDate")]
        public void PutDepartureDate(string id, [FromBody]string value)
        {
            var deserializedDate = new DateTime();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(value));
            var ser = new DataContractJsonSerializer(deserializedDate.GetType());
            deserializedDate = ser.ReadObject(ms) as DateTime;
            ms.Close();
            campingTripRepository.UpdateDepartureDate(id,)
        }*/

        // DELETE: api/ApiWithActions/5
        [Authorize(Policy = "Organizer Or Admin")]
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            campingTripRepository.RemoveCampingTrip(id);
        }

        public static CampingTrip ReadToObject(string json)
        {
            CampingTrip deserializedUser = new CampingTrip();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as CampingTrip;
            ms.Close();
            return deserializedUser;
        }
    }
}