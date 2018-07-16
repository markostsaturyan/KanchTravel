using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class CampingTripRepository : ICampingTripRepository
    {
        private readonly CampingTripContext campingTripContext = null;

        public CampingTripRepository(IOptions<Settings> settings)
        {
            campingTripContext = new CampingTripContext(settings);
        }

        public async Task AddCampingTrip(CampingTrip item)
        {
            await campingTripContext.CampingTrips.InsertOneAsync(item);
        }

        public async Task<IEnumerable<CampingTrip>> GetAllCampingTrips()
        {
            return await campingTripContext.CampingTrips.Find(_ => true).ToListAsync();
        }

        public async Task<CampingTrip> GetCampingTrip(string id)
        {
            return await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();
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

        public async Task<UpdateResult> UpdateDriver(string id, int driverId)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, id);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.DriverID, driverId);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdateGuide(string id, int guideId)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, id);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.GuideID, guideId);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdatePhotographer(string id, int photographerId)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, id);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.PhotographerID, photographerId);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

    }
}
