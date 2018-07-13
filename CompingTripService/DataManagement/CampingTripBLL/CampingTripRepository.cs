using System;
using System.Collections.Generic;
using System.Linq;
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
            var filter = Builders<CampingTrip>.Filter.Eq("Id", id);
            return await campingTripContext.CampingTrips
                            .Find(filter)
                            .FirstOrDefaultAsync();
        }

        public async Task<DeleteResult> RemoveCampingTrip(string id)
        {
            return await campingTripContext.CampingTrips.DeleteOneAsync(
                 Builders<CampingTrip>.Filter.Eq("Id", id));
        }

        public async Task<UpdateResult> UpdateCampingTrip(int id, DateTime departureDate)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, id);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.DepartureDate, departureDate);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }
    }
}
