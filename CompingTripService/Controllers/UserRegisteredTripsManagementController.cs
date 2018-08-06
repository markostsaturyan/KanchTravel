using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampingTripService.DataManagement.Model;
using CampingTripService.DataManagement.CampingTripBLL;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/UserRegisteredTripsManagement")]
    public class UserRegisteredTripsManagementController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public UserRegisteredTripsManagementController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }


        // GET: api/UserRegisteredTripsManagement
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            return await campingTripRepository.GetAllUnconfirmedTrips();
        }

        // GET: api/UserRegisteredTripsManagement/5
        [HttpGet("{campingtripId}")]
        public async Task<CampingTripFull> Get(string campingtripId)
        {
            return await campingTripRepository.GetUnconfirmedTripById(campingtripId);
        }
    }
}
