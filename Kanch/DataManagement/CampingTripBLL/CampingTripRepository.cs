using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kanch.DataManagement.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace Kanch.DataManagement.CampingTripBLL
{
    public class CampingTripRepository : ICampingTripRepository
    {
        private readonly CampingTripContext campingTripContext;

        public CampingTripRepository(IOptions<Settings> settings)
        {
            campingTripContext = new CampingTripContext(settings);
        }

        public async Task AddCampingTrip(CampingTrip item)
        {
            await campingTripContext.CampingTrips.InsertOneAsync(item);
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllRegistartionCompletedCampingTrips()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.IsRegistrationCompleted, true);
            var filter1 = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);
            var trips= await campingTripContext.CampingTrips.Find(filter & filter1).ToListAsync();
            var campingTripsFull = new List<CampingTripFull>();
            foreach(var trip in trips)
            {
                campingTripsFull.Add(new CampingTripFull(trip));
            }
            return campingTripsFull;
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllRegistartionNotCompletedCampingTrips()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.IsRegistrationCompleted, false);
            var filter1 = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);
            var trips = await campingTripContext.CampingTrips.Find(filter & filter1).ToListAsync();
            var campingTripsFull = new List<CampingTripFull>();
            foreach (var trip in trips)
            {
                campingTripsFull.Add(new CampingTripFull(trip));
            }
            return campingTripsFull;
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllCompletedCampingTrips()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);
            var trips = await campingTripContext.CampingTrips.Find(filter).ToListAsync();
            var completedCampingTrips = new List<CampingTripFull>();
            foreach (var trip in trips)
            {
                completedCampingTrips.Add(new CampingTripFull(trip));
            }
            return completedCampingTrips;
        }

        public async Task<IEnumerable<CampingTripFull>> GetCampingTrips()
        {
            var trips = await campingTripContext.CampingTrips.Find(_ => true).ToListAsync();
            var campingTrips = new List<CampingTripFull>();
            foreach (var trip in trips)
            {
                campingTrips.Add(new CampingTripFull(trip));
            }
            return campingTrips;
        }

        public async Task<CampingTripFull> GetCampingTrip(string id)
        {
            var trip= await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();

            return new CampingTripFull(trip);
            
        }

        public async Task<DeleteResult> RemoveCampingTrip(string id)
        {
            return await campingTripContext.CampingTrips.DeleteOneAsync(
                 Builders<CampingTrip>.Filter.Eq("Id", id));
        }

        public async Task<ReplaceOneResult> UpdateCampingTrip(string id, CampingTrip trip)
        {
            return await campingTripContext.CampingTrips
                          .ReplaceOneAsync(n => n.ID.Equals(id)
                                            , trip
                                            , new UpdateOptions { IsUpsert = true });
        }
    }
}
