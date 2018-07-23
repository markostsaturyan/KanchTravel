using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.DataManagement.DataAccesLayer;
using System.Security.Claims;

namespace UserManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Car")]
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
        [HttpGet("{numberOfSeats}", Name = "Get")]
        public IEnumerable<CarInfo> Get(int numberOfSeats)
        {
            return this.dataAccesLayer.GetCarByNumberOfSeats(numberOfSeats);
        }
    }
}
