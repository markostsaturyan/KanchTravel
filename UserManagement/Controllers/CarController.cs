using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.DataManagement.DataAccesLayer;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/car")]
    public class CarController : Controller
    {
        private readonly DataAccesLayer dataAccesLayer;

        public CarController(DataAccesLayer dataAccesLayer)
        {
            this.dataAccesLayer = dataAccesLayer;
        }

        // GET: api/Car
        [HttpGet]
        public IEnumerable<CarInfo> Get()
        {
            return this.dataAccesLayer.GetAllCars();
        }

        // GET: api/Car/5
        [HttpGet("{id}")]
        public IEnumerable<CarInfo> Get(int numberOfSeats)
        {
            return this.dataAccesLayer.GetCarByNumberOfSeats(numberOfSeats);
        }
    }
}
