using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.Model
{
    public class CompletedCampingTripContext
    {
        private readonly IMongoDatabase database;

        public CompletedCampingTripContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                database = client.GetDatabase(settings.Value.Database1);
        }

        public IMongoCollection<CompletedCampingTrip> CampingTrips
        {
            get
            {
                return database.GetCollection<CompletedCampingTrip>("CompletedCampingTrip");
            }
        }
    }
}
