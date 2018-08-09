using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CampingTripService.DataManagement.Model.Users;
using System.Collections.Generic;
using System;
using IdentityModel.Client;
using System.Net.Http;
using Newtonsoft.Json;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class SignUpForTheTrip : ISignUpForTheTrip
    {
        private readonly CampingTripContext campingTripContext;

        private readonly CampingTripRepository campingTripRepository;

        private DiscoveryResponse discovery;

        public SignUpForTheTrip(IOptions<Settings> settings)
        {
            this.campingTripContext = new CampingTripContext(settings);
            this.campingTripRepository = new CampingTripRepository(settings);
            this.discovery = settings.Value.DiscoveryResponse;
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
            if (trip != null)
            {
                return await GetTripDriverAsync(trip.DriverID);
            }
            else
            {
                return null;
            }
        }

        public async Task<Guide> GetGuide(string id)
        {
            var trip = await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();
            if (trip != null)
            {
                return await GetTripGuideAsync(trip.GuideID);
            }
            else
            {
                return null;
            }
        }

        public async Task<Photographer> GetPhotographer(string id)
        {
            var trip = await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();

            if (trip != null)
            {
                return await GetTripPhotographerAsync(trip.PhotographerID);
            }
            else
            {
                return null;
            }
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

        public async Task<Status> AsMemberAsync(int id, string campingTripID)
        {
            var campingTrip = await campingTripRepository.GetTripAsync(campingTripID);

            var user =await GetUserAsync(id);

            if (user == null) return new Status()
            {
                IsOk = false,
                StatusCode = 4001,
                Message = "User not found"
            };

            var zeroTime = new DateTime(1, 1, 1);

            var span = DateTime.Now - user.DateOfBirth;

            var userAge = (zeroTime + span).Year - 1;

            if (campingTrip != null)
            {
                if (campingTrip.MinAge <= userAge && campingTrip.MaxAge >= userAge)
                {
                    campingTrip.MembersOfCampingTrip.Add(id);
                    campingTrip.CountOfMembers++;

                    if (campingTrip.CountOfMembers == campingTrip.MaxCountOfMembers)
                    {
                        campingTrip.IsRegistrationCompleted = true;
                    }

                    await campingTripRepository.UpdateCampingTrip(campingTripID, campingTrip);

                    return new Status
                    {
                        IsOk = true,
                        StatusCode = 1000,
                        Message = "Member registered for the campaign"
                    };
                }
                else
                {
                    return new Status
                    {
                        IsOk = false,
                        StatusCode = 5001,
                        Message = "Your age in not corresponds"
                    };
                }
            }
            else
            {
                return new Status
                {
                    IsOk = false,
                    StatusCode = 5003,
                    Message = "The trip not found"
                };
            }
        }

        public async Task<Status> RemoveMemberFromTheTripAsync(int id, string campingTripID)
        {
            var campingTrip = await campingTripRepository.GetTripAsync(campingTripID);

            if (campingTrip != null)
            {
                if (campingTrip.MembersOfCampingTrip.Contains(id))
                {
                    campingTrip.MembersOfCampingTrip.Remove(id);

                    if (campingTrip.IsRegistrationCompleted == true)
                    {
                        campingTrip.IsRegistrationCompleted = false;
                        campingTrip.CountOfMembers--;
                    }

                    await campingTripRepository.UpdateCampingTrip(campingTripID, campingTrip);

                    return new Status
                    {
                        IsOk = true,
                        StatusCode = 1000,
                        Message = "The member has been removed"
                    };
                }
                else
                {
                    return new Status
                    {
                        IsOk = false,
                        StatusCode = 5002,
                        Message = "The user is not registered for this campaign"
                    };
                }
            }
            else
            {
                return new Status
                {
                    IsOk = false,
                    StatusCode = 5003,
                    Message = "The trip not found"
                };
            }
        }

        private async Task<User> GetUserAsync(int id)
        {
            var tokenClinet = new TokenClient(this.discovery.TokenEndpoint, "campingTrip", "secret");

            var tokenResponse = await tokenClinet.RequestClientCredentialsAsync("userManagement", "secret");

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await httpClient.GetAsync("api/User/" + id);

            var user = new User();

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                var userJson = await content.ReadAsStringAsync();

                user = JsonConvert.DeserializeObject<User>(userJson);

            }

            return user;
        }

        private async Task<Photographer> GetTripPhotographerAsync(int id)
        {
            var tokenClinet = new TokenClient(discovery.TokenEndpoint, "campingTrip", "secret");

            var tokenResponse = tokenClinet.RequestClientCredentialsAsync("userManagement", "secret").Result;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var photographer = new Photographer();

            var response = await httpClient.GetAsync("api/photographer/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                var userJson = await content.ReadAsStringAsync();

                photographer = JsonConvert.DeserializeObject<Photographer>(userJson);
            }

            return photographer;
        }

        private async Task<Driver> GetTripDriverAsync(int id)
        {
            var tokenClinet = new TokenClient(discovery.TokenEndpoint, "campingTrip", "secret");

            var tokenResponse = tokenClinet.RequestClientCredentialsAsync("userManagement", "secret").Result;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var driver = new Driver();

            var response = await httpClient.GetAsync("api/driver/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                var userJson = await content.ReadAsStringAsync();

                driver = JsonConvert.DeserializeObject<Driver>(userJson);
            }

            return driver;
        }

        private async Task<Guide> GetTripGuideAsync(int id)
        {
            var tokenClinet = new TokenClient(discovery.TokenEndpoint, "campingTrip", "secret");

            var tokenResponse = tokenClinet.RequestClientCredentialsAsync("userManagement", "secret").Result;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var guide = new Guide();

            var response = await httpClient.GetAsync("api/guide/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                var userJson = await content.ReadAsStringAsync();

                guide = JsonConvert.DeserializeObject<Guide>(userJson);
            }

            return guide;
        }

        public async Task<IEnumerable<string>> GetTripsByMemberId(int id)
        {
            var filterByMemberId = Builders<CampingTrip>.Filter.Eq(trip=>trip.MembersOfCampingTrip.Contains(id),true);

            var trips = await campingTripContext.CampingTrips.Find(filterByMemberId).ToListAsync();

            var campingTrips = new List<string>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTrips.Add(trip.ID);
                }
            }

            return campingTrips;
        }
    }
}
