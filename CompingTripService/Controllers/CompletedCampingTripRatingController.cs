using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.CampingTripBLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/CompletedCampingTripRating")]
    public class CompletedCampingTripRatingController : Controller
    {
        private readonly ICompletedCampingTripRepository campingTripRepository;

        public CompletedCampingTripRatingController(ICompletedCampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // PUT: api/CompletedCampingTripRating/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody]double rating)
        {
            await this.campingTripRepository.UpdateRaiting(id, rating);
        }
    }
}
