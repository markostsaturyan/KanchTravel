using System;
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
        //[Authorize(Policy ="For Admin")]
        [HttpGet(Name ="GetRegistrationCompleted")]
        public async Task<IEnumerable<CampingTrip>> GetRegistrationCompleted()
        {
            return await campingTripRepository.GetAllRegistartionCompletedCampingTrips();
        }

        // GET: api/CampingTrips
        //[Authorize(Policy ="For Admin")]
        [HttpGet]
        public async Task<IEnumerable<CampingTrip>> Get()
        {
            return await campingTripRepository.GetAllRegistartionNotCompletedCampingTrips();
        }

        // GET: api/CampingTrips/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<CampingTripFull> Get(string id)
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

        [HttpPut("{id}",Name ="PutDepartureDate")]
        public void PutDepartureDate(string id, [FromBody]DateTime value)
        {
            campingTripRepository.UpdateDepartureDate(id, value);
        }

        [HttpPut("{id}",Name ="PutCountOfMembers")]
        public void PutCountOfMembers(string id,[FromBody]int count)
        {
            campingTripRepository.UpdateCountOfMembers(id, count);
        }

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
