using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class CompletedCampingTripRepository : ICompletedCampingTripRepository
    {
        private readonly CompletedCampingTripContext completedCampingTripContext;

        public CompletedCampingTripRepository(IOptions<Settings> settings)
        {
            completedCampingTripContext = new CompletedCampingTripContext(settings);
        }

        public async Task AddCampingTrip(CampingTrip campingTrip)
        {
            var completedCampingTrip = new CompletedCampingTrip(campingTrip);
            await completedCampingTripContext.CampingTrips.InsertOneAsync(completedCampingTrip);
        }

        public async Task<IEnumerable<CompletedCampingTripFull>> GetAllCampingTrips()
        {
            var trips = await completedCampingTripContext.CampingTrips.Find(_ => true).ToListAsync();
            var completedCampingTrips = new List<CompletedCampingTripFull>();
            foreach(var trip in trips)
            {
                completedCampingTrips.Add(new CompletedCampingTripFull(trip));
            }
            return completedCampingTrips;
        }

        public async Task<CompletedCampingTripFull> GetCampingTrip(string id)
        {
            var trip = await completedCampingTripContext.CampingTrips.Find(Builders<CompletedCampingTrip>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
            return new CompletedCampingTripFull(trip);
        }


        public async Task<DeleteResult> RemoveCampingTrip(string id)
        {
            return await completedCampingTripContext.CampingTrips.DeleteOneAsync(Builders<CompletedCampingTrip>.Filter.Eq("Id", id));
        }

        public async Task<ReplaceOneResult> UpdateCampingTrip(string id, CompletedCampingTrip campingTrip)
        {
            return await completedCampingTripContext.CampingTrips
                          .ReplaceOneAsync(n => n.ID.Equals(id)
                                            , campingTrip
                                            , new UpdateOptions { IsUpsert = true });
        }

        /*public async Task<UpdateResult> UpdateComments(string id, Comment comment)
        {
            var filter = Builders<CompletedCampingTrip>.Filter.Eq(s => s.ID, id);

            var campingTrip = await GetCampingTrip(id);

            campingTrip.Comments.Add(comment);

            var update = Builders<CompletedCampingTrip>.Update
                            .Set(s => s.Comments, campingTrip.Comments);

            return await completedCampingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async  Task<UpdateResult> UpdateRaiting(string id, double raiting)
        {
            var filter = Builders<CompletedCampingTrip>.Filter.Eq(s => s.ID, id);
            var campingTrip = await GetCampingTrip(id);
            campingTrip.CurrentRaiting.CurrentRaiting++;
            campingTrip.CurrentRaiting.CurrentRaiting = (campingTrip.CurrentRaiting.CurrentRaiting + raiting) / campingTrip.CurrentRaiting.CountOfAppraisers;
            var update = Builders<CompletedCampingTrip>.Update
                            .Set(s => s.CurrentRaiting, campingTrip.CurrentRaiting);

            return await completedCampingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }*/
    }
}
