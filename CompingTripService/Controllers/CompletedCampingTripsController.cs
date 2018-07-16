using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/CompletedCampingTrips")]
    public class CompletedCampingTripsController : Controller
    {
        private readonly ICompletedCampingTripRepository campingTripRepository;

        public CompletedCampingTripsController(ICompletedCampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }
        // GET: api/CompletedCampingTrips
        [HttpGet]
        public async Task<IEnumerable<CompletedCampingTripFull>> Get()
        {
            return await campingTripRepository.GetAllCampingTrips();
        }

        // GET: api/CompletedCampingTrips/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<CompletedCampingTripFull> Get(string id)
        {
            return await campingTripRepository.GetCampingTrip(id);
        }
        
        // POST: api/CompletedCampingTrips
        [HttpPost]
        public async void Post([FromBody]string value)
        {
            await campingTripRepository.AddCampingTrip(ReadToObject(value));
        }
        
        // PUT: api/CompletedCampingTrips/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody]CompletedCampingTrip value)
        {
            await campingTripRepository.UpdateCampingTrip(id, value);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await campingTripRepository.RemoveCampingTrip(id);
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
