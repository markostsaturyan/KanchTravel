using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly HistoryContext historyContext = null;

        public HistoryRepository(IOptions<Settings> settings)
        {
            historyContext = new HistoryContext(settings);
        }

        public async Task AddCampingTrip(CampingTrip item)
        {
            await historyContext.CampingTrips.InsertOneAsync((History)item);
        }

        public async Task<IEnumerable<History>> GetAllCampingTrips()
        {
            return await historyContext.CampingTrips.Find(_ => true).ToListAsync();
        }

        public async Task<History> GetCampingTrip(string id)
        {
            return await historyContext.CampingTrips.Find(Builders<History>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task<DeleteResult> RemoveCampingTrip(string id)
        {
            return await historyContext.CampingTrips.DeleteOneAsync(Builders<History>.Filter.Eq("Id", id));
        }

        public async Task<ReplaceOneResult> UpdateCampingTrip(string id, History campingTrip)
        {
            return await historyContext.CampingTrips
                          .ReplaceOneAsync(n => n.ID.Equals(id)
                                            , campingTrip
                                            , new UpdateOptions { IsUpsert = true });
        }

        public async Task<UpdateResult> UpdateComments(string id, Comment comment)
        {
            var filter = Builders<History>.Filter.Eq(s => s.ID, id);

            var history = await GetCampingTrip(id);

            history.Comments.Add(comment);

            var update = Builders<History>.Update
                            .Set(s => s.Comments, history.Comments);

            return await historyContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public Task<UpdateResult> UpdateRaiting(string id, double raiting)
        {
            
        }
    }
}
