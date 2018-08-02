using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/campingtrips")]
    public class CampingTripsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public CampingTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        //[Authorize(Policy ="For Admin")]
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            var queryString = this.HttpContext.Request.Query;
            if(!queryString.TryGetValue("status", out StringValues status))
            {
                status = string.Empty;
            }

             if (status == "RegistrationCompleted")
            {
                return await campingTripRepository.GetAllRegistartionCompletedCampingTrips();
            }
            else if(status == "Completed")
            {
                return await campingTripRepository.GetAllCompletedCampingTrips();
            }
            else if(status == "InProgress")
            {
                return await campingTripRepository.GetAllRegistartionNotCompletedCampingTrips();
            }
            else
            {
                return await campingTripRepository.GetCampingTrips();
            }
            
        }

        // GET: api/CampingTrips/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<CampingTripFull> Get(string id)
        {
            return await campingTripRepository.GetCampingTrip(id);
        }

        // POST: api/CampingTrips
        [Authorize]
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
