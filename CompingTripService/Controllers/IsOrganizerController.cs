using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CampingTripService.DataManagement.CampingTripBLL;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/IsOrganizer")]
    public class IsOrganizerController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public IsOrganizerController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }
        // GET: api/IsOrganizer/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<bool> Get(int id)
        {
            return await campingTripRepository.IsOrganizerAsync(id);
        }
    }
}
