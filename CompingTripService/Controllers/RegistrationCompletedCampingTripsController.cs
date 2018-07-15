using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/RegistrationCompletedCampingTrips")]
    public class RegistrationCompletedCampingTripsController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public RegistrationCompletedCampingTripsController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // GET: api/RegistrationCompletedCampingTrips
        [HttpGet]
        public async Task<IEnumerable<CampingTripFull>> Get()
        {
            return await campingTripRepository.GetAllRegistartionCompletedCampingTrips();
        }
    }
}
