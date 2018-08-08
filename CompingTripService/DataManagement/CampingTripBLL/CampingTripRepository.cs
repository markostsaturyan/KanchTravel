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
using CampingTripService.Utility;

namespace CampingTripService.DataManagement.CampingTripBLL
{
    public class CampingTripRepository : ICampingTripRepository
    {
        private readonly CampingTripContext campingTripContext;

        private DiscoveryResponse discoveryResponse;

        public CampingTripRepository(IOptions<Settings> settings)
        {
            campingTripContext = new CampingTripContext(settings);
            discoveryResponse = settings.Value.DiscoveryResponse;
        }

        public async Task AddCampingTripAsync(CampingTripFull item)
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
            var tokenClinet = new TokenClient(discoveryResponse.TokenEndpoint, "campingTrip", "secret");

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
            var tokenClinet = new TokenClient(discoveryResponse.TokenEndpoint, "campingTrip", "secret");

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
            var tokenClinet = new TokenClient(discoveryResponse.TokenEndpoint, "campingTrip", "secret");

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
            var tokenClinet = new TokenClient(discoveryResponse.TokenEndpoint, "campingTrip", "secret");

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

        public async Task UpdateUserRegistredCampingTripAsync(string campingTripId, int orginizerId, CampingTripFull campingTrip)
        {
            var tripNonFull = new CampingTrip(campingTrip);

            await campingTripContext.CampingTrips
                          .ReplaceOneAsync(n => n.ID.Equals(campingTripId) && n.OrganzierID == orginizerId, tripNonFull, new UpdateOptions { IsUpsert = true });
        }

        public async Task RemoveUserRegistredCampingTripAsync(string campingTripId, int userId)
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
                    var filter = Builders<ServiceRequest>.Filter.Eq(request => request.CampingTripId == trip.ID, true);

                    var filterResponse = Builders<ServiceRequestResponse>.Filter.Eq(response => response.CampingTripId == trip.ID, true);

                    var requests = await campingTripContext.ServiceRequests.Find(filter).ToListAsync();

                    var responses = await campingTripContext.ServiceRequestResponses.Find(filterResponse).ToListAsync();

                    if (requests.Count == 0 && responses.Count == 0)
                    {
                        campingTrips.Add(await GetCampingTripMembersAndDPGForAdmin(trip, false));
                    }
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

        public async Task<IEnumerable<ServiceRequest>> GetAllInProgresServiceRequestsAsync()
        {
            var requestIsValidFilter= Builders<ServiceRequest>.Filter.Eq(request=>request.RequestValidityPeriod>DateTime.Now, true);

            return await campingTripContext.ServiceRequests.Find(requestIsValidFilter).ToListAsync();
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceProvidersAllInProgresServiceRequests(int providerId)
        {
            var requestIsValidFilter = Builders<ServiceRequest>.Filter.Eq(request => request.RequestValidityPeriod > DateTime.Now, true);

            var filterByProviderId = Builders<ServiceRequest>.Filter.Eq(request => request.ProviderId == providerId, true);

            return await campingTripContext.ServiceRequests.Find(requestIsValidFilter & filterByProviderId).ToListAsync();
        }

        public async Task<ServiceRequest> GetServiceRequestByIdAsync(string id)
        {
            var requestIsValidFilter = Builders<ServiceRequest>.Filter.Eq(request => request.RequestValidityPeriod > DateTime.Now, true);

            var filterByRequestId = Builders<ServiceRequest>.Filter.Eq(request => request.Id == id, true);

            return await campingTripContext.ServiceRequests.Find(requestIsValidFilter & filterByRequestId).FirstOrDefaultAsync();
        }

        public async Task<ServiceRequest> GetServiceRequestByIdAndProviderIdAsync(string id,int providerId)
        {
            var requestIsValidFilter = Builders<ServiceRequest>.Filter.Eq(request => request.RequestValidityPeriod > DateTime.Now, true);

            var filterByRequestId = Builders<ServiceRequest>.Filter.Eq(request => request.Id == id, true);

            var filterByProviderId = Builders<ServiceRequest>.Filter.Eq(request => request.ProviderId == providerId, true);

            return await campingTripContext.ServiceRequests.Find(requestIsValidFilter & filterByRequestId & filterByProviderId).FirstOrDefaultAsync();
        }

        public async Task AddServiceRequestAsync(ServiceRequest request)
        {
            await campingTripContext.ServiceRequests.InsertOneAsync(request);
        }

        public async Task RemoveServiceRequestAsync(string id)
        {
            var filterByRequestId = Builders<ServiceRequest>.Filter.Eq("Id", id);

            await campingTripContext.ServiceRequests.DeleteOneAsync(filterByRequestId);
        }

        public async Task RemoveServiceRequestByIdAndProviderIdAsync(string id, int providerId)
        {
            var filterByRequestId = Builders<ServiceRequest>.Filter.Eq("Id", id);

            var filterByProviderId = Builders<ServiceRequest>.Filter.Eq(request => request.ProviderId==providerId,true);

            await campingTripContext.ServiceRequests.DeleteOneAsync(filterByRequestId & filterByProviderId);
        }

        public async Task<IEnumerable<ServiceRequestResponse>> GetAllServiceRequestResponsesAsync()
        {
            var isValid= Builders<ServiceRequestResponse>.Filter.Eq(response=>response.ResponseValidityPeriod>DateTime.Now,true);

            return await campingTripContext.ServiceRequestResponses.Find(isValid).ToListAsync();
        }

        public async Task<IEnumerable<ServiceRequestResponse>> GetAllServiceRequestResponsesByProviderIdAsync(int providerId)
        {
            var isValid = Builders<ServiceRequestResponse>.Filter.Eq(response => response.ResponseValidityPeriod > DateTime.Now, true);

            var filterByProviderId= Builders<ServiceRequestResponse>.Filter.Eq(response => response.ProviderId== providerId, true);

            return await campingTripContext.ServiceRequestResponses.Find(isValid & filterByProviderId).ToListAsync();
        }

        public async Task<ServiceRequestResponse> GetServiceRequestResponseByIdAsync(string id)
        {
            var isValid = Builders<ServiceRequestResponse>.Filter.Eq(response => response.ResponseValidityPeriod > DateTime.Now, true);

            var filterById= Builders<ServiceRequestResponse>.Filter.Eq(response => response.Id==id, true);

            return await campingTripContext.ServiceRequestResponses.Find(isValid & filterById).FirstOrDefaultAsync();
        }

        public async Task<ServiceRequestResponse> GetServiceRequestResponsesByIdAndProviderIdAsync(string id, int providerId)
        {
            var isValid = Builders<ServiceRequestResponse>.Filter.Eq(response => response.ResponseValidityPeriod > DateTime.Now, true);

            var filterByProviderId = Builders<ServiceRequestResponse>.Filter.Eq(response => response.ProviderId == providerId, true);

            var filterById = Builders<ServiceRequestResponse>.Filter.Eq(response => response.Id == id, true);

            return await campingTripContext.ServiceRequestResponses.Find(isValid & filterById & filterByProviderId).FirstOrDefaultAsync();
        }

        public async Task AddServiceRequestResponceAsync(ServiceRequestResponse response)
        {
            await campingTripContext.ServiceRequestResponses.InsertOneAsync(response);
        }

        public async Task UpdateServiceRequestResponseAsync(string id, ServiceRequestResponse response)
        {
            var filterById = Builders<ServiceRequestResponse>.Filter.Eq("Id",id);

            await campingTripContext.ServiceRequestResponses
                          .ReplaceOneAsync(filterById, response, new UpdateOptions { IsUpsert = true });
        }

        public async Task RemoveServiceRequestResponseAsync(string id)
        {
            var filterByRequestId = Builders<ServiceRequestResponse>.Filter.Eq("Id", id);

            await campingTripContext.ServiceRequestResponses.DeleteOneAsync(filterByRequestId);
        }

        public async Task RemoveServiceRequestResponseByIdAndProviderIdAsync(string id, int providerId)
        {
            var filterByRequestId = Builders<ServiceRequestResponse>.Filter.Eq("Id", id);

            var filterByProviderId = Builders<ServiceRequestResponse>.Filter.Eq(response => response.ProviderId == providerId, true);

            await campingTripContext.ServiceRequestResponses.DeleteOneAsync(filterByRequestId & filterByProviderId);
        }

        public async Task<bool> IsOrganizerAsync(int id)
        {
            var filterByRequestId = Builders<CampingTrip>.Filter.Eq(trip => trip.OrganzierID == id, true);

            var trips = await campingTripContext.CampingTrips.Find(filterByRequestId).ToListAsync();

            if (trips.Count > 0) return true;

            return false;
        }

        public async Task SendingServiceRequests(CampingTripFull campingTrip)
        {
            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "campingTrip", "secret");

            var tokenResponce = await tokenClient.RequestClientCredentialsAsync("userManagement", "secret");

            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("http://localhost:5000/");

            httpClient.SetBearerToken(tokenResponce.AccessToken);

            var membersCount = campingTrip.MembersOfCampingTrip.Count;


            if (campingTrip.HasGuide)
            {
                var responseGuide = await httpClient.GetAsync("api/Guide");

                if (responseGuide.IsSuccessStatusCode)
                {
                    var content = responseGuide.Content;

                    var guidesJson = await content.ReadAsStringAsync();

                    var guides = JsonConvert.DeserializeObject<List<Guide>>(guidesJson);

                    if (guides != null)
                    {
                        foreach (var guide in guides)
                        {
                            await AddServiceRequestAsync(new ServiceRequest
                            {
                                CampingTripId = campingTrip.ID,
                                ProviderId = guide.Id,
                                RequestValidityPeriod = campingTrip.DepartureDate
                            });
                        }
                    }
                }

                membersCount++;
            }

            if (campingTrip.HasPhotographer)
            {
                var responsePhotographer= await httpClient.GetAsync("api/Photographer");

                if (responsePhotographer.IsSuccessStatusCode)
                {
                    var content = responsePhotographer.Content;

                    var photographersJson = await content.ReadAsStringAsync();

                    var photographers = JsonConvert.DeserializeObject<List<Photographer>>(photographersJson);

                    if (photographers != null)
                    {
                        foreach(var photographer in photographers)
                        {
                            await AddServiceRequestAsync(new ServiceRequest
                            {
                                CampingTripId = campingTrip.ID,
                                ProviderId = photographer.Id,
                                RequestValidityPeriod = campingTrip.DepartureDate
                            });
                        }
                    }
                }

                membersCount++;
            }

            var response = await httpClient.GetAsync($"api/Car/{membersCount}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                var carsJson = await content.ReadAsStringAsync();

                var cars = JsonConvert.DeserializeObject<List<CarWithDriverId>>(carsJson);

                if (cars != null)
                {
                    foreach (var car in cars)
                    {
                        await AddServiceRequestAsync(new ServiceRequest
                        {
                            CampingTripId = campingTrip.ID,
                            ProviderId = car.DriverId,
                            RequestValidityPeriod = campingTrip.DepartureDate
                        });
                    }
                }
            }
        }

        public async Task RemoveUserRegistredCampingTripAndSendingEmail(string campingTripId)
        {
            var filterById = Builders<CampingTrip>.Filter.Eq("Id", campingTripId);

            var trip = await campingTripContext.CampingTrips.Find(filterById).FirstAsync();

            var user = await GetUserAsync(trip.OrganzierID);

            if (user != null)
            {
                var emailService = new EmailService(new System.Net.NetworkCredential("kanchhiking", "kanchhiking2018"));

                emailService.Send("Kanch", trip.ToString());

            }

            await RemoveCampingTripAsync(campingTripId);
        }

        private async Task<User> GetUserAsync(int id)
        {
            var tokenClinet = new TokenClient(discoveryResponse.TokenEndpoint, "campingTrip", "secret");

            var tokenResponse = tokenClinet.RequestClientCredentialsAsync("userManagement", "secret").Result;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            User user = null;

            var response = await httpClient.GetAsync($"api/User/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                var userJson = await content.ReadAsStringAsync();

                user = JsonConvert.DeserializeObject<User>(userJson);

            }

            return user;
        }
    }
}