using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampingTripService.DataManagement.CampingTripBLL;
using CampingTripService.DataManagement.Model;

namespace CampingTripService.Controllers
{
    [Produces("application/json")]
    [Route("api/System")]
    public class SystemController : Controller
    {
        private readonly ICampingTripRepository campingTripRepository;

        public SystemController(ICampingTripRepository campingTripRepository)
        {
            this.campingTripRepository = campingTripRepository;
        }

        // Call an initialization - api/system/init
        [HttpGet("{setting}")]
        public string Get(string setting)
        {
            if (setting == "init")
            {
                //_noteRepository.RemoveAllNotes();
                campingTripRepository.AddCampingTrip(new CampingTrip()
                {
                    Place = "Stepanavan",
                    DepartureDate = new DateTime(2018, 07, 14),
                    ArrivalDate = new DateTime(2018, 07, 14),
                    Direction = new List<string>()
                    {
                        "Dendropark"
                    },
                    TypeOfTrip = TypeOfCampingTrip.campingTrip,
                    OrganizationType = TypeOfOrganization.orderByUser,
                    CountOfMembers = 25,
                    DriverID = 1,
                    GuideID = 0,
                    PhotographerID = 0,
                    Food=new List<Food>()
                    {
                        new Food
                        {
                            Name="Bread",
                            Measure=2
                        },
                        new Food
                        {
                            Name="Cheese",
                            Measure=0.5,
                            MeasurementUnit="kg"
                        }
                    },
                    PriceOfTrip=8000
                });

                return "Done";
            }

            return "Unknown";
        }
    }
}
