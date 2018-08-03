using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CampingTripService.DataManagement.Model.UsersDAL;
using CampingTripService.DataManagement.Model.Users;
using System.Collections.Generic;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class SignUpForTheTrip : ISignUpForTheTrip
    {
        private readonly CampingTripContext campingTripContext;

        private readonly CampingTripRepository campingTripRepository;

        private readonly UsersDal usersDal;

        public SignUpForTheTrip(IOptions<Settings> settings)
        {
            this.campingTripContext = new CampingTripContext(settings);
            this.campingTripRepository = new CampingTripRepository(settings);
            this.usersDal = new UsersDal();
        }

        public async Task<UpdateResult> AsDriver(int id,string campingTripID)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, campingTripID);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.DriverID, id);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async  Task<Driver> GetDriver(string id)
        {
            var trip = await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();
            var userDal = new UsersDal();
            return userDal.GetDriver(trip.DriverID);
        }

        public async Task<Guide> GetGuide(string id)
        {
            var trip = await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();
            var userDal = new UsersDal();
            return userDal.GetGuide(trip.GuideID);
        }

        public async Task<Photographer> GetPhotographer(string id)
        {
            var trip = await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();
            var userDal = new UsersDal();
            return userDal.GetPhotographer(trip.PhotographerID);
        }
        public async Task<UpdateResult> RemoveDriverFromTheTrip(string campingTripID)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, campingTripID);
            var update = Builders<CampingTrip>.Update.Set(s => s.DriverID, 0);
            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> AsGuide(int id, string campingTripID)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, campingTripID);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.GuideID, id);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> RemoveGuideFromTheTrip(string campingTripID)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, campingTripID);
            var update = Builders<CampingTrip>.Update.Set(s => s.GuideID, 0);
            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> AsPhotographer(int id, string campingTripID)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, campingTripID);
            var update = Builders<CampingTrip>.Update
                            .Set(s => s.PhotographerID, id);

            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> RemovePhotographerFromTheTrip(string campingTripID)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ID, campingTripID);
            var update = Builders<CampingTrip>.Update.Set(s => s.PhotographerID, 0);
            return await campingTripContext.CampingTrips.UpdateOneAsync(filter, update);
        }

        public async Task AsMember(int id, string campingTripID)
        {
            var campingTripFull = await campingTripRepository.GetCampingTrip(campingTripID);
            var campingTrip = new CampingTrip(campingTripFull);
            var userContext = new UserContext();
            var user = userContext.GetUser(id);
            if(campingTrip.MinAge <= user.Age && campingTrip.MaxAge >= user.Age)
            {
                userContext.SignUpForTheCamping(id, campingTripID);
                campingTrip.CountOfMembers++;
                if (campingTrip.CountOfMembers == campingTrip.MaxCountOfMembers)
                {
                    campingTrip.IsRegistrationCompleted = true;
                }

                await campingTripRepository.UpdateCampingTrip(campingTripID, campingTrip);

            }
            else
            {
                await new Task<Status>(() => new Status
                {
                    IsOk = false,
                    StatusCode = 5001,
                    Message = "Your age in not corresponds"
                });
            }
        }

        public void RemoveMemberFromTheTrip(int id, string campingTripID)
        {
            this.usersDal.RemoveMemberFromTheTrip(id, campingTripID);
        }

        public List<User> GetMembersOfCampingTrip(string campingTripId)
        {
            return this.usersDal.GetMembersOfTheCampingTrip(campingTripId);
        }

        public List<CampingTripFull> GetUserRegisteredCampingTrips(int userId)
        {
            var campingTripsId = this.usersDal.GetUserRegisteredCampingTripsId(userId);
            var campingTrips = new List<CampingTripFull>();
            foreach(var campingTrip in campingTripsId)
            {
                campingTrips.Add(this.campingTripRepository.GetCampingTrip(campingTrip).Result);
            }
            return campingTrips;
        }
    }
}
