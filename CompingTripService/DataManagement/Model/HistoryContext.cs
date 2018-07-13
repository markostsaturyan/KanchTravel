using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.Model
{
    public class HistoryContext
    {
        private readonly IMongoDatabase database = null;

        public HistoryContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<History> CampingTrips
        {
            get
            {
                return database.GetCollection<History>("History");
            }
        }
    }
}
