using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using CampingTripService.DataManagement.Model.Users;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace CampingTripService.DataManagement.CampingTripBLL
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
            var trips= await campingTripContext.CampingTrips.Find(filter).ToListAsync();
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
            var trips = await campingTripContext.CampingTrips.Find(filter).ToListAsync();
            var campingTripsFull = new List<CampingTripFull>();
            foreach (var trip in trips)
            {
                campingTripsFull.Add(new CampingTripFull(trip));
            }
            return campingTripsFull;
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

        public async Task<UpdateResult> UpdateDepartureDate(string id, DateTime departureDate)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, id);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.DepartureDate, departureDate);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdateCountOfMembers(string id, int count)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, id);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.CountOfMembers, count);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

    }
}
