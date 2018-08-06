using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampingTripService.DataManagement.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using IdentityModel.Client;
using CampingTripService.DataManagement.Model.Users;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class CampingTripRepository : ICampingTripRepository
    {
        private readonly CampingTripContext campingTripContext;

        private IOptions<Settings> settings;

        public CampingTripRepository(IOptions<Settings> settings)
        {
            campingTripContext = new CampingTripContext(settings);
            this.settings = settings;
        }

        public async Task AddCampingTrip(CampingTripFull item)
        {
            await campingTripContext.CampingTrips.InsertOneAsync(new CampingTrip(item));
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllRegistartionCompletedCampingTripsAsync()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.IsRegistrationCompleted, true);

            var filter1 = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);

            var orgineizerIsAdmin = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByAdmin, true);

            var trips= await campingTripContext.CampingTrips.Find(filter & filter1 & orgineizerIsAdmin).ToListAsync();

            var campingTripsFull = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTripsFull.Add(await GetCampingTripMembersAndDPGForAdmin(trip));
                }
            }

            return campingTripsFull;
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllRegistartionNotCompletedCampingTripsAsync()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.IsRegistrationCompleted, false);

            var filter1 = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);

            var orgineizerIsAdmin = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByAdmin, true);

            var trips = await campingTripContext.CampingTrips.Find(filter & filter1 & orgineizerIsAdmin).ToListAsync();

            var campingTripsFull = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTripsFull.Add(await GetCampingTripMembersAndDPGForAdmin(trip));
                }
            }

            return campingTripsFull;
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllCompletedCampingTripsAsync()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);

            var orgineizerIsAdmin = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByAdmin, true);

            var trips = await campingTripContext.CampingTrips.Find(filter & orgineizerIsAdmin).ToListAsync();

            var completedCampingTrips = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    completedCampingTrips.Add(await GetCampingTripMembersAndDPGForAdmin(trip));
                }
            }

            return completedCampingTrips;
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllCompletedCampingTripsForUserAsync()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);

            var orgineizerIsAdmin = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByAdmin, true);

            var trips = await campingTripContext.CampingTrips.Find(filter & orgineizerIsAdmin).ToListAsync();

            var completedCampingTrips = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    completedCampingTrips.Add(await GetCampingTripMembersAndDPGForUser(trip));
                }
            }

            return completedCampingTrips;
        }

        public async Task<CampingTripFull> GetCampingTripAsync(string id)
        {
            var trip = await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();
            if (trip != null)
            {
                return await GetCampingTripMembersAndDPGForAdmin(trip);
            }
            else
            {
                return null;
            }
        }

        public async Task<CampingTripFull> GetCampingTripForUserAsync(string id)
        {
            var trip = await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", id))
                            .FirstOrDefaultAsync();
            if (trip != null)
            {
                return await GetCampingTripMembersAndDPGForUser(trip);
            }
            else
            {
                return null;
            }
        }

        public async Task<DeleteResult> RemoveCampingTripAsync(string id)
        {
            return await campingTripContext.CampingTrips.DeleteOneAsync(
                                            Builders<CampingTrip>.Filter.Eq("Id", id));
        }

        public async Task<ReplaceOneResult> UpdateCampingTrip(string id, CampingTripFull trip)
        {
            var tripNonFull = new CampingTrip(trip);

            return await campingTripContext.CampingTrips
                          .ReplaceOneAsync(n => n.ID.Equals(id), tripNonFull, new UpdateOptions { IsUpsert = true });
        }

        public async Task<ReplaceOneResult> UpdateCampingTrip(string id, CampingTrip trip)
        {
            return await campingTripContext.CampingTrips
                          .ReplaceOneAsync(n => n.ID.Equals(id), trip, new UpdateOptions { IsUpsert = true });
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllRegistartionCompletedCampingTripsForUserAsync()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.IsRegistrationCompleted, true);

            var filter1 = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);

            var orgineizerIsAdmin = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByAdmin, true);

            var trips = await campingTripContext.CampingTrips.Find(filter & filter1 & orgineizerIsAdmin).ToListAsync();

            var campingTripsFull = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTripsFull.Add(await GetCampingTripMembersAndDPGForUser(trip));
                }
            }
            return campingTripsFull;
        }

        private async Task<List<User>> GetCampingTripMembersAsync(List<int> membersId)
        {
            var tokenClinet = new TokenClient(settings.Value.DiscoveryResponse.TokenEndpoint, "campingTrip", "secret");

            var tokenResponse = tokenClinet.RequestClientCredentialsAsync("userManagement", "secret").Result;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var members = new List<User>();

            foreach (var id in membersId)
            {
                var response = await httpClient.GetAsync("api/User/" + id);

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;

                    var userJson = await content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<User>(userJson);

                    members.Add(user);
                }
            }

            return members;
        }

        private async Task<Photographer> GetTripPhotographerAsync(int id)
        {
            var tokenClinet = new TokenClient(settings.Value.DiscoveryResponse.TokenEndpoint, "campingTrip", "secret");

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
            var tokenClinet = new TokenClient(settings.Value.DiscoveryResponse.TokenEndpoint, "campingTrip", "secret");

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
            var tokenClinet = new TokenClient(settings.Value.DiscoveryResponse.TokenEndpoint, "campingTrip", "secret");

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

        public async Task<CampingTripFull> GetCompletedCampingTripAsync(string campinTripId)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);

            var orgineizerIsAdmin = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByAdmin, true);

            var filterById = Builders<CampingTrip>.Filter.Eq(tr => tr.ID == campinTripId, true);

            var trip = await campingTripContext.CampingTrips.Find(filter & orgineizerIsAdmin & filterById).FirstOrDefaultAsync();

            if (trip != null)
            {
                return await GetCampingTripMembersAndDPGForAdmin(trip);
            }
            else
            {
                return null;
            }
        }

        public async Task<CampingTripFull> GetCompletedCampingTripForUserAsync(string campinTripId)
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);

            var orgineizerIsAdmin = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByAdmin, true);

            var filterById = Builders<CampingTrip>.Filter.Eq(tr => tr.ID == campinTripId, true);

            var trip = await campingTripContext.CampingTrips.Find(filter & orgineizerIsAdmin & filterById).FirstOrDefaultAsync();

            if (trip != null)
            {
                return await GetCampingTripMembersAndDPGForUser(trip);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllRegistartionNotCompletedCampingTripsForUserAsync()
        {
            var filter = Builders<CampingTrip>.Filter.Eq(s => s.IsRegistrationCompleted, false);

            var filter1 = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);

            var orgineizerIsAdmin = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByAdmin, true);

            var trips = await campingTripContext.CampingTrips.Find(filter & filter1 & orgineizerIsAdmin).ToListAsync();

            var campingTripsFull = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTripsFull.Add(await GetCampingTripMembersAndDPGForUser(trip));
                }
            }
            return campingTripsFull;
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredTripsAsync()
        {
            var tripNotCopmleted = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);

            var orgineizerIsUser = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByUser, true);

            var trips = await campingTripContext.CampingTrips.Find(tripNotCopmleted & orgineizerIsUser).ToListAsync();

            var campingTripsFull = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTripsFull.Add(await GetCampingTripMembersAndDPGForAdmin(trip,false));
                }
            }

            return campingTripsFull;
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredTripsForUserAsync(int userId)
        {
            var tripNotCopmleted = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);

            var orgineizerIsUser = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByUser, true);

            var iAmOrganizer= Builders<CampingTrip>.Filter.Eq(tr => tr.OrganzierID==userId, true);

            var trips = await campingTripContext.CampingTrips.Find(tripNotCopmleted & orgineizerIsUser & iAmOrganizer).ToListAsync();

            var campingTripsFull = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTripsFull.Add(await GetCampingTripMembersAndDPGForUser(trip, false));
                }
            }
            return campingTripsFull;
        }

        public async Task<CampingTripFull> GetUserRegisteredTripAsync(string campingTripId)
        {
            var tripNotCopmleted = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);

            var orgineizerIsUser = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByUser, true);

            var campingTripWithId = Builders<CampingTrip>.Filter.Eq(tr => tr.ID == campingTripId, true);

            var trip = await campingTripContext.CampingTrips.Find(tripNotCopmleted & orgineizerIsUser & campingTripWithId).FirstOrDefaultAsync();

            if(trip != null)
            {
                return await GetCampingTripMembersAndDPGForUser(trip, false);
            }
            else
            {
                return null;
            }
        }

        public async Task<CampingTripFull> GetUserRegisteredTripsForUserAsync(string campingTripId, int userId)
        {
            var tripNotCopmleted = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, true);

            var orgineizerIsUser = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByUser, true);

            var isOrganizer = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganzierID == userId, true);

            var campingTripWithId = Builders<CampingTrip>.Filter.Eq(tr => tr.ID == campingTripId, true);

            var trip = await campingTripContext.CampingTrips.Find(tripNotCopmleted & orgineizerIsUser & isOrganizer & campingTripWithId).FirstOrDefaultAsync();

            if (trip != null)
            {
                return await GetCampingTripMembersAndDPGForUser(trip, false);
            }
            else
            {
                return null;
            }
        }

        public async void UpdateUserRegistredCampingTrip(string campingTripId, int orginizerId, CampingTripFull campingTrip)
        {
            var tripNonFull = new CampingTrip(campingTrip);

            await campingTripContext.CampingTrips
                          .ReplaceOneAsync(n => n.ID.Equals(campingTripId) && n.OrganzierID == orginizerId, tripNonFull, new UpdateOptions { IsUpsert = true });
        }

        public async void RemoveUserRegistredCampingTripAsync(string campingTripId, int userId)
        {
            await campingTripContext.CampingTrips.DeleteOneAsync(
                                            Builders<CampingTrip>.Filter.Eq("Id", campingTripId) & Builders<CampingTrip>.Filter.Eq(trip=>trip.OrganzierID==userId,true));
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredCompletedTripsAsync()
        {
            var tripCopmleted = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);

            var orgineizerIsUser = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByUser, true);

            var trips = await campingTripContext.CampingTrips.Find(tripCopmleted & orgineizerIsUser).ToListAsync();

            var campingTripsFull = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTripsFull.Add(await GetCampingTripMembersAndDPGForAdmin(trip,false));
                }
            }

            return campingTripsFull;
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllUserRegisteredCompletedTripsForUserAsync()
        {
            var tripCopmleted = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);

            var orgineizerIsUser = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByUser, true);

            var trips = await campingTripContext.CampingTrips.Find(tripCopmleted & orgineizerIsUser).ToListAsync();

            var campingTripsFull = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach (var trip in trips)
                {
                    campingTripsFull.Add(await GetCampingTripMembersAndDPGForUser(trip, false));
                }
            }

            return campingTripsFull;
        }

        public async Task<CampingTripFull> GetUserRegisteredCompletedTripAsync(string campingTripId)
        {
            var tripCopmleted = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);

            var orgineizerIsUser = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByUser, true);

            var filterCampingTripById= Builders<CampingTrip>.Filter.Eq(s => s.ID==campingTripId, false);

            var trip = await campingTripContext.CampingTrips.Find(tripCopmleted & orgineizerIsUser & filterCampingTripById).FirstOrDefaultAsync();

            if (trip != null)
            {
                return await GetCampingTripMembersAndDPGForAdmin(trip, false);
            }
            else
            {
                return null;
            }
        }

        public async Task<CampingTripFull> GetUserRegisteredCompletedTripForUserAsync(string campingTripId)
        {
            var tripCopmleted = Builders<CampingTrip>.Filter.Eq(s => s.ArrivalDate > DateTime.Now, false);

            var orgineizerIsUser = Builders<CampingTrip>.Filter.Eq(tr => tr.OrganizationType == TypeOfOrganization.orderByUser, true);

            var filterCampingTripById = Builders<CampingTrip>.Filter.Eq(s => s.ID == campingTripId, false);

            var trip = await campingTripContext.CampingTrips.Find(tripCopmleted & orgineizerIsUser & filterCampingTripById).FirstOrDefaultAsync();

            if (trip != null)
            {
                return await GetCampingTripMembersAndDPGForUser(trip, false);
            }
            else
            {
                return null;
            }
        }

        private async Task<CampingTripFull> GetCampingTripMembersAndDPGForAdmin(CampingTrip trip, bool getMembers = true)
        {
            var fullTrip = new CampingTripFull(trip);

            if (getMembers)
            {
                fullTrip.MembersOfCampingTrip = await GetCampingTripMembersAsync(trip.MembersOfCampingTrip);
            }

            if (trip.HasPhotographer)
            {
                fullTrip.Photographer = await GetTripPhotographerAsync(trip.PhotographerID);
            }

            if (trip.DriverID != 0)
            {
                fullTrip.Driver = await GetTripDriverAsync(trip.DriverID);
            }

            if (trip.HasGuide)
            {
                fullTrip.Guide = await GetTripGuideAsync(trip.GuideID);
            }

            return fullTrip;
        }

        private async Task<CampingTripFull> GetCampingTripMembersAndDPGForUser(CampingTrip trip,bool getMembers = true)
        {
            var fullTrip = new CampingTripFull(trip);

            if (getMembers)
            {
                foreach (var member in await GetCampingTripMembersAsync(trip.MembersOfCampingTrip))
                {
                    fullTrip.MembersOfCampingTrip.Add(new User
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Gender = member.Gender,
                        Img = member.Img
                    });
                }
            }

            if (trip.HasPhotographer)
            {
                var potographer = await GetTripPhotographerAsync(trip.PhotographerID);

                fullTrip.Photographer = new Photographer
                {
                    Id = potographer.Id,
                    FirstName = potographer.FirstName,
                    LastName = potographer.LastName,
                    Gender = potographer.Gender,
                    Camera = potographer.Camera,
                    KnowledgeOfLanguages = potographer.KnowledgeOfLanguages,
                    HasCameraStabilizator = potographer.HasCameraStabilizator,
                    HasDron = potographer.HasDron,
                    HasGopro = potographer.HasGopro,
                    Profession = potographer.Profession,
                    Img = potographer.Img,
                    Rating = potographer.Rating,
                    WorkExperience = potographer.WorkExperience,
                    NumberOfAppraisers = potographer.NumberOfAppraisers
                };
            }

            if (trip.DriverID != 0)
            {
                var driver = await GetTripDriverAsync(trip.DriverID);

                fullTrip.Driver = new Driver
                {
                    Id = driver.Id,
                    FirstName = driver.FirstName,
                    LastName = driver.LastName,
                    Gender = driver.Gender,
                    Img = driver.Img,
                    Car = driver.Car,
                    KnowledgeOfLanguages = driver.KnowledgeOfLanguages,
                    NumberOfAppraisers = driver.NumberOfAppraisers,
                    Rating = driver.Rating,
                };
            }

            if (trip.HasGuide)
            {
                var guide = await GetTripGuideAsync(trip.GuideID);

                fullTrip.Guide = new Guide
                {
                    Id = guide.Id,
                    FirstName = guide.FirstName,
                    LastName = guide.LastName,
                    Gender = guide.Gender,
                    Img = guide.Img,
                    EducationGrade = guide.EducationGrade,
                    KnowledgeOfLanguages = guide.KnowledgeOfLanguages,
                    NumberOfAppraisers = guide.NumberOfAppraisers,
                    Places = guide.Places,
                    Profession = guide.Profession,
                    Rating = guide.Rating,
                    WorkExperience = guide.WorkExperience
                };
            }
            
            return fullTrip;
        }

        public async Task<CampingTrip> GetTripAsync(string campingTripId)
        {
            return await campingTripContext.CampingTrips
                            .Find(Builders<CampingTrip>.Filter.Eq("Id", campingTripId))
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CampingTripFull>> GetAllUnconfirmedTrips()
        {
            var driverIsNull = Builders<CampingTrip>.Filter.Eq(trip => trip.DriverID == 0, true);
            var guideIdNull = Builders<CampingTrip>.Filter.Eq(trip => trip.HasGuide && trip.GuideID == 0, true);
            var photographerIsNull = Builders<CampingTrip>.Filter.Eq(trip => trip.HasPhotographer && trip.PhotographerID == 0, true);
            var organizedByUser = Builders<CampingTrip>.Filter.Eq(trip => trip.OrganizationType == TypeOfOrganization.orderByUser,true);

            var trips = await campingTripContext.CampingTrips.Find(driverIsNull | guideIdNull | photographerIsNull & organizedByUser).ToListAsync();

            var campingTrips = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach(var trip in trips)
                {
                    campingTrips.Add(await GetCampingTripMembersAndDPGForAdmin(trip, false));
                }
            }

            return campingTrips;
        }

        public async Task<CampingTripFull> GetUnconfirmedTripById(string campingTripId)
        {
            var driverIsNull = Builders<CampingTrip>.Filter.Eq(trip => trip.DriverID == 0, true);
            var guideIdNull = Builders<CampingTrip>.Filter.Eq(trip => trip.HasGuide && trip.GuideID == 0, true);
            var photographerIsNull = Builders<CampingTrip>.Filter.Eq(trip => trip.HasPhotographer && trip.PhotographerID == 0, true);
            var organizedByUser = Builders<CampingTrip>.Filter.Eq(trip => trip.OrganizationType == TypeOfOrganization.orderByUser, true);
            var campingTripById = Builders<CampingTrip>.Filter.Eq(trip => trip.ID == campingTripId, true);

            var campingTrip = await campingTripContext.CampingTrips.Find(driverIsNull | guideIdNull | photographerIsNull & organizedByUser).FirstOrDefaultAsync();

            if (campingTrip != null)
            {
                return await GetCampingTripMembersAndDPGForAdmin(campingTrip, false);
            }

            return null;
        }

        public async Task<IEnumerable<CampingTripFull>> GetDriverTripsAsync(int userId)
        {
            var tripsByDriverId = Builders<CampingTrip>.Filter.Eq(trip => trip.DriverID == userId, true);
            var inProgres = Builders<CampingTrip>.Filter.Eq(trip => trip.ArrivalDate > DateTime.Now, true);

            var trips = await campingTripContext.CampingTrips.Find(tripsByDriverId & inProgres).ToListAsync();

            var campingTrips = new List<CampingTripFull>();

            if (trips != null)
            {
                foreach(var trip in trips)
                {
                    campingTrips.Add(await GetCampingTripMembersAndDPGForUser(trip));
                }
            }

            return campingTrips;
        }
    }
}
