﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Kanch.DataManagement.Model
{
    public class CampingTripContext
    {
        private readonly IMongoDatabase database;

        public CampingTripContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<CampingTrip> CampingTrips
        {
            get
            {
                return database.GetCollection<CampingTrip>("CampingTrips");
            }
        }
    }
}
