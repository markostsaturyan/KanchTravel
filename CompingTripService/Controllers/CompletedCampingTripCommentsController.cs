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
    [Route("api/CompletedCampingTripComments")]
    public class CompletedCampingTripCommentsController : Controller
    {
        private readonly ICompletedCampingTripRepository campingTripRepository;

        public CompletedCampingTripCommentsController(ICompletedCampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // POST: api/CompletedCampingTripComments
        [HttpPost("{id}")]
        public async void Post(string id,[FromBody]Comment comment)
        {
            await this.campingTripRepository.UpdateComments(id, comment);
        }
       
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id,[FromBody]string campingTripId)
        {
            this.campingTripRepository.DeleteComment(campingTripId, id);
        }
    }
}
