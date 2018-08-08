using IdentityModel.Client;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UserManagement.DataManagement.DataAccesLayer.Models;
using UserManagement.DataManagement.Exceptions;
using UserManagement.DataManagement.Security;

namespace UserManagement.DataManagement.DataAccesLayer
{
    public class DataAccesLayer
    {
        #region Constructors and fields

        /// <summary>
        /// Connection string
        /// </summary>
        private readonly string sqlConnectionString;

        private TokenClient tokenClient;

        private HttpClient httpClient;

        public DataAccesLayer(string sqlConnectionString,string authenticationService,string campingTripBaseAddress)
        {
            this.sqlConnectionString = sqlConnectionString;

            var discoveryResponse = DiscoveryClient.GetAsync(authenticationService).Result;

            tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "userManagement", "secret");

            httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(campingTripBaseAddress);
        }
        
        #endregion Constructors and fields

        #region Adding
        /// <summary>
        /// Adds user to database
        /// </summary>
        /// <param name="user"> The user full info </param>
        /// <returns> User's id after inserting </returns>
        public int AddUser(UserInfo user,string userRole="User")
        {
            var userGuid = Guid.NewGuid().ToString();

            string hashedPassword = SecurityForPassword.HashSHA1(user.Password + userGuid);

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertUser"
                };

                command.Parameters.AddWithValue("@firstName", user.FirstName);
                command.Parameters.AddWithValue("@lastName", user.LastName);
                command.Parameters.AddWithValue("@gender", user.Gender);
                command.Parameters.AddWithValue("@dateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@picture", user.Image);
                command.Parameters.AddWithValue("@username", user.UserName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@userGuid", userGuid);
                command.Parameters.AddWithValue("@userRole", userRole);

                connection.Open();

                var reader = command.ExecuteReader();

                reader.Read();

                return (int)reader["Id"];
            }
        }

        

        /// <summary>
        /// Add userId and registration code in UserVerification table
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>The registration code</returns>
        public int AddUserVerification(string userName)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var verifCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertUserVerificationCode"
                };

                var rand = new Random();

                var randNumber = rand.Next(1000, 9999);

                verifCommand.Parameters.AddWithValue("@userName", userName);
                verifCommand.Parameters.AddWithValue("@code", randNumber);

                connection.Open();

                verifCommand.ExecuteNonQuery();

                return randNumber;
            }
        }

        /// <summary>
        /// Adds Guide to database
        /// </summary>
        /// <param name="guide"> The guide full info </param>
        public int AddGuide(GuideInfo guide)
        {
            var userInfo = new UserInfo
            {
                FirstName = guide.FirstName,
                LastName = guide.LastName,
                Gender = guide.Gender,
                DateOfBirth = guide.DateOfBirth,
                PhoneNumber = guide.PhoneNumber,
                Email = guide.Email,
                Image = guide.Image,
                UserName = guide.UserName,
                Password = guide.Password
            };

            var userId = AddUser(userInfo,"Guide");

            AddGuidePlaces(userId, guide.Places);

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertGuideInfo"
                };

                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@educationGrade", guide.EducationGrade);
                command.Parameters.AddWithValue("@profession", guide.Profession);
                command.Parameters.AddWithValue("@knowledgeOfLanguages", guide.KnowledgeOfLanguages);
                command.Parameters.AddWithValue("@workExperience", guide.WorkExperience);

                connection.Open();

                command.ExecuteNonQuery();
            }

            return userId;
        }

        public void AddGuidePlaces(int id, List<string> places)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                for (int i = 0; i < places.Count; i++)
                {
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "InsertGuidePlaces"
                    };

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@place", places[i]);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Adds Driver into database
        /// </summary>
        /// <param name="driver"> Driver full info </param>
        public int AddDriver(DriverInfo driver)
        {
            var userFullInfo = new UserInfo
            {
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Gender = driver.Gender,
                DateOfBirth = driver.DateOfBirth,
                PhoneNumber = driver.PhoneNumber,
                Email = driver.Email,
                Image = driver.Image,
                UserName = driver.UserName,
                Password = driver.Password,
            };

            var userId = AddUser(userFullInfo,"Driver");
            var carId = AddCar(driver.Car);

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var commandForInsertDriver = new SqlCommand
                {
                    Connection = connection,
                    CommandType=System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertDriverInfo"
                };

                commandForInsertDriver.Parameters.AddWithValue("@userId", userId);
                commandForInsertDriver.Parameters.AddWithValue("@carId", carId);
                commandForInsertDriver.Parameters.AddWithValue("@drivingLicencePicFront", driver.DrivingLicencePicFront);
                commandForInsertDriver.Parameters.AddWithValue("@drivingLicencePicBack", driver.DrivingLicencePicBack);
                commandForInsertDriver.Parameters.AddWithValue("@knowledgeOfLanguages", driver.KnowledgeOfLanguages);
                connection.Open();
                commandForInsertDriver.ExecuteNonQuery();
            }

            return userId;
        }

        /// <summary>
        /// Adds Driver's car to database
        /// </summary>
        /// <param name="car"> Car full info </param>
        /// <returns> Car's id after inserting </returns>
        public int AddCar(Car car)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var commandForInsertCar = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertCar"
                };

                commandForInsertCar.Parameters.AddWithValue("@brand", car.Brand);
                commandForInsertCar.Parameters.AddWithValue("@numberOfSeats", car.NumberOfSeats);
                commandForInsertCar.Parameters.AddWithValue("@fuelType", car.FuelType);
                commandForInsertCar.Parameters.AddWithValue("@carPicture1", car.CarPicture1);
                commandForInsertCar.Parameters.AddWithValue("@carPicture2", car.CarPicture2);
                commandForInsertCar.Parameters.AddWithValue("@carPicture3", car.CarPicture3);
                commandForInsertCar.Parameters.AddWithValue("@licensePlate", car.LicensePlate);
                commandForInsertCar.Parameters.AddWithValue("@hasWiFi", car.HasWiFi);
                commandForInsertCar.Parameters.AddWithValue("@hasMicrophone", car.HasMicrophone);
                commandForInsertCar.Parameters.AddWithValue("@hasAirConditioner", car.HasAirConditioner);
                commandForInsertCar.Parameters.AddWithValue("@hasKitchen", car.HasKitchen);
                commandForInsertCar.Parameters.AddWithValue("@hasToilet", car.HasToilet);

                connection.Open();

                return (int)commandForInsertCar.ExecuteScalar();
            }
        }

        /// <summary>
        /// Adds Photodrapher into database
        /// </summary>
        /// <param name="photographer"> Photographer full info </param>
        public int AddPhotographer(PhotographerInfo photographer)
        {
            var userFullInfo = new UserInfo
            {
                FirstName = photographer.FirstName,
                LastName = photographer.LastName,
                Gender = photographer.Gender,
                DateOfBirth = photographer.DateOfBirth,
                PhoneNumber = photographer.PhoneNumber,
                Email = photographer.Email,
                Image = photographer.Image,
                UserName = photographer.UserName,
                Password = photographer.Password
            };

            var userId = AddUser(userFullInfo,"Photographer");
            var cameraId = AddCamera(photographer.Camera);

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var commandForInsertPhotographer = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertPhotographer"
                };

                commandForInsertPhotographer.Parameters.AddWithValue("@userId", userId);
                commandForInsertPhotographer.Parameters.AddWithValue("@cameraId", cameraId);
                commandForInsertPhotographer.Parameters.AddWithValue("@profession", photographer.Profession);
                commandForInsertPhotographer.Parameters.AddWithValue("@knowledgeOfLanguages", photographer.KnowledgeOfLanguages);
                commandForInsertPhotographer.Parameters.AddWithValue("@workExperience", photographer.WorkExperience);
                commandForInsertPhotographer.Parameters.AddWithValue("@hasDron", photographer.HasDron);
                commandForInsertPhotographer.Parameters.AddWithValue("@hasCameraStabilizator", photographer.HasCameraStabilizator);
                commandForInsertPhotographer.Parameters.AddWithValue("@hasGopro", photographer.HasGopro);

                connection.Open();

                commandForInsertPhotographer.ExecuteNonQuery();
            }

            return userId;
        }

        /// <summary>
        /// Adds photographer's Camera into dataabase
        /// </summary>
        /// <param name="camera"> Camera full info </param>
        /// <returns> Camera's id after inserting </returns>
        public int AddCamera(Camera camera)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var commandForInsertCamera = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertCamera"
                };

                commandForInsertCamera.Parameters.AddWithValue("@isProfessional", camera.IsProfessional);
                commandForInsertCamera.Parameters.AddWithValue("@model", camera.Model);

                connection.Open();
                return (int)commandForInsertCamera.ExecuteScalar();
            }
        }

        public void AddServiceRequest(ServiceRequest serviceRequest)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var commandForInsertServiceRequest = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertServiceRequest"
                };

                commandForInsertServiceRequest.Parameters.AddWithValue("@userId", serviceRequest.UserId);
                commandForInsertServiceRequest.Parameters.AddWithValue("@campingTripId", serviceRequest.CampingTripId);

                connection.Open();

                commandForInsertServiceRequest.ExecuteNonQuery();
            }
        }

        public void AddServiceRequestResponse(ServiceRequestResponse response)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var commandForInsert = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertServiceRequestResponse"
                };

                commandForInsert.Parameters.AddWithValue("@userId", response.UserId);
                commandForInsert.Parameters.AddWithValue("@campingTripId", response.CampingTripId);
                commandForInsert.Parameters.AddWithValue("@price", response.Price);

                connection.Open();

                commandForInsert.ExecuteNonQuery();
            }
        }

        #endregion Adding

        #region Getting
        /// <summary>
        /// Gets User by id from database
        /// </summary>
        /// <param name="id"> User id </param>
        /// <returns> UserInfo </returns>
        public UserInfo GetUserById(int id)
        {
            var user = new UserInfo();

            using (var connection = new SqlConnection(sqlConnectionString))
            {

                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetFullUserInfoById"
                };

                command.Parameters.AddWithValue("@userId", id);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    user = new UserInfo
                    {
                        Id = (int)dataReader["Id"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        DateOfBirth = (DateTime)dataReader["DateOfBirth"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        UserName = (string)dataReader["UserName"],
                        Gender = (string)dataReader["Gender"]
                    };

                    if (dataReader["Picture"] == DBNull.Value)
                    {
                        user.Image = null;
                    }
                    else
                    {
                        user.Image = (byte[])dataReader["Picture"];
                    }

                }
            }

            return user;
        }

        public GuideInfo GetGuideById(int id)
        {
            var guide = new GuideInfo();

            using (var connection = new SqlConnection(sqlConnectionString))
            {

                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetGuideInfoById"
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    guide = new GuideInfo
                    {
                        Id = (int)dataReader["Id"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        UserName = (string)dataReader["UserName"],
                        Gender = (string)dataReader["Gender"],
                        EducationGrade = (string)dataReader["EducationGrade"],
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        Profession = (string)dataReader["Profession"],
                        Rating = (double)dataReader["Raiting"],
                        WorkExperience = (string)dataReader["WorkExperience"],
                        NumberOfAppraisers = (int)dataReader["NumberOfAppraisers"],
                    };
                    if(dataReader["Image"] != DBNull.Value)
                    {
                        guide.Image = (byte[])dataReader["Image"];
                    }
                    guide.Places = GetGuidePalces(id);

                }
            }

            return guide;
        }

        public string GetUserEmailById(int id)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUserEmailById"
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                var dataReader = command.ExecuteReader();

                dataReader.Read();

                return (string)dataReader["Email"];
            }
        }

        public string GetUserPasswordById(int id, out string guide)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUserPasswordAndGuideById"
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                var dataReader = command.ExecuteReader();

                dataReader.Read();

                guide = (string)dataReader["UserGuide"];

                return (string)dataReader["Password"];
            }
        }

        public string GetUserNameById(int id)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUserNameById"
                };

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                var dataReader = command.ExecuteReader();

                dataReader.Read();

                return (string)dataReader["UserName"];
            }
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns> List of Users </returns>
        public List<UserInfo> GetAllUsers()
        {
            var users = new List<UserInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllUsers"
                };

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        
                        var user = new UserInfo
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            UserName = (string)dataReader["UserName"],
                            Gender = (string)dataReader["Gender"]
                        };
                        if (dataReader["Picture"] != DBNull.Value)
                        {
                            user.Image = (byte[])dataReader["Picture"];
                        }
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Gets all drivers
        /// </summary>
        /// <returns> List of drivers </returns>
        public List<DriverInfo> GetAllDrivers()
        {
            var drivers = new List<DriverInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllDrivers"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Car car = new Car
                        {
                            Id = (int)dataReader["CarId"],
                            Brand = (string)dataReader["Brand"],
                            NumberOfSeats = (int)dataReader["NumberOfSeats"],
                            FuelType = (string)dataReader["FuelType"],
                            LicensePlate = (string)dataReader["LicensePlate"],
                            HasWiFi = (bool)dataReader["HasWiFi"],
                            HasMicrophone = (bool)dataReader["HasMicrophone"],
                            HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                            HasKitchen = (bool)dataReader["HasKitchen"],
                            HasToilet = (bool)dataReader["HasToilet"],
                        };
                        if (dataReader["CarPicture1"] != DBNull.Value)
                        {
                            car.CarPicture1 = (byte[])dataReader["CarPicture1"];
                        }
                        if (dataReader["CarPicture2"] != DBNull.Value)
                        {
                            car.CarPicture2 = (byte[])dataReader["CarPicture2"];
                        }
                        if (dataReader["CarPicture3"] != DBNull.Value)
                        {
                            car.CarPicture3 = (byte[])dataReader["CarPicture3"];
                        }

                        var driver = new DriverInfo
                        {
                            Id = (int)dataReader["UserId"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            Gender = (string)dataReader["Gender"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            UserName = (string)dataReader["UserName"],
                            Car = car,
                            DrivingLicencePicFront = (byte[])dataReader["DrivingLicencePicFront"],
                            DrivingLicencePicBack = (byte[])dataReader["DrivingLicencePicBack"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Rating = (double)dataReader["Rating"],
                            NumberOfAppraisers = (int)dataReader["NumberOfAppraisers"]
                        };

                        if (dataReader["Picture"] != DBNull.Value)
                        {
                            driver.Image = (byte[])dataReader["Picture"];
                        }
                        drivers.Add(driver);

                        
                    }
                }
            }

            return drivers;
        }

        /// <summary>
        /// Gets driver by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DriverInfo GetDriverById(int id)
        {
            var driver = new DriverInfo();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetDriverById"
                };
                command.Parameters.AddWithValue("@driverId", id);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    var car = new Car
                    {
                        Id = (int)dataReader["CarId"],
                        Brand = (string)dataReader["Brand"],
                        NumberOfSeats = (int)dataReader["NumberOfSeats"],
                        FuelType = (string)dataReader["FuelType"],
                        HasWiFi = (bool)dataReader["HasWiFi"],
                        HasMicrophone = (bool)dataReader["HasMicrophone"],
                        HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                        HasKitchen = (bool)dataReader["HasKitchen"],
                        HasToilet = (bool)dataReader["HasToilet"]
                    };


                    if (dataReader["CarPicture1"] != DBNull.Value)
                    {
                        car.CarPicture1 = (byte[])dataReader["CarPicture1"];
                    }
                    if (dataReader["CarPicture2"] != DBNull.Value)
                    {
                        car.CarPicture2 = (byte[])dataReader["CarPicture2"];
                    }
                    if (dataReader["CarPicture3"] != DBNull.Value)
                    {
                        car.CarPicture3 = (byte[])dataReader["CarPicture3"];
                    }

                    driver = new DriverInfo
                    {
                        Id = (int)dataReader["UserId"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        UserName = (string)dataReader["UserName"],
                        Car = car,
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        Rating = (double)dataReader["Rating"],
                        Gender=(string)dataReader["Gender"],
                        NumberOfAppraisers=(int)dataReader["NumberOfAppraisers"]
                    };

                    if (dataReader["Picture"] != DBNull.Value)
                    {
                        driver.Image = (byte[])dataReader["Picture"];
                    }


                }
            }

            return driver;
        }

        /// <summary>
        /// Gets all guides
        /// </summary>
        /// <returns> List of guides </returns>
        public List<GuideInfo> GetAllGuides()
        {
            var guides = new List<GuideInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllGuides"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var guide = new GuideInfo
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            UserName = (string)dataReader["UserName"],
                            Password = (string)dataReader["Password"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Gender = (string)dataReader["Gender"],
                            EducationGrade = (string)dataReader["EducationGrade"],
                            Profession = (string)dataReader["Profession"],
                            WorkExperience=(string)dataReader["WorkExperience"],
                            Rating=(double)dataReader["Rating"],
                            NumberOfAppraisers=(int)dataReader["NumberOfAppraisers"]
                        };
                        if (dataReader["Picture"] != DBNull.Value)
                        {
                            guide.Image = (byte[])dataReader["Picture"];
                        }
                        guide.Places = GetGuidePalces(guide.Id);

                        guides.Add(guide);
                    }
                }
            }

            return guides;
        }

        /// <summary>
        /// Gets all places where guide's been 
        /// </summary>
        /// <param name="id"> id of guide</param>
        /// <returns> List of places </returns>
        public List<string> GetGuidePalces(int id)
        {
            var places = new List<string>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetGuidePlaces"
                };

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        places.Add((string)dataReader["Places"]);
                    }
                }
            }

            return places;
        }

        /// <summary>
        /// Gets all photographers
        /// </summary>
        /// <returns> List of photographers </returns>
        public List<PhotographerInfo> GetAllPhotographers()
        {
            var photographers = new List<PhotographerInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllPhotographers"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var photographer = new PhotographerInfo
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            Gender = (string)dataReader["Gender"],
                            DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            UserName = (string)dataReader["UserName"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Raiting = (double)dataReader["Raiting"],
                            HasCameraStabilizator = (bool)dataReader["HasCameraStabilizator"],
                            HasDron = (bool)dataReader["HasDron"],
                            HasGopro = (bool)dataReader["HasGopro"],
                            Camera = new Camera
                            {
                                Id = (int)dataReader["CameraId"],
                                Model = (string)dataReader["Model"],
                                IsProfessional = (bool)dataReader["IsProfessional"]
                            },
                            Profession = (string)dataReader["Profession"],
                            WorkExperience = (string)dataReader["WorkExperiance"],
                            NumberOfAppraisers = (int)dataReader["NumberOfAppraisers"]
                        };
                        if (dataReader["Picture"] != DBNull.Value)
                        {
                            photographer.Image = (byte[])dataReader["Picture"];
                        }
                        photographers.Add(photographer);
                    }
                }
            }

            return photographers;
        }

        /// <summary>
        /// Gets Photographer by id from database
        /// </summary>
        /// <param name="id"> Photographer id </param>
        /// <returns> Photographer full info </returns>
        public PhotographerInfo GetPhotographerById(int id)
        {
            var photographer = new PhotographerInfo();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetPhotographerById"
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    var camera = new Camera
                    {
                        Id = (int)dataReader["CameraId"],
                        IsProfessional = (bool)dataReader["IsProfessional"],
                        Model = (string)dataReader["Model"]
                    };

                    photographer = new PhotographerInfo
                    {
                        Id = (int)dataReader["Id"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        Gender = (string)dataReader["Gender"],
                        DateOfBirth = (DateTime)dataReader["Age"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        UserName = (string)dataReader["UserName"],
                        Camera = camera,
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        Raiting = (double)dataReader["Raiting"],
                        HasCameraStabilizator = (bool)dataReader["HasCameraStabilizator"],
                        HasDron = (bool)dataReader["HasDron"],
                        HasGopro = (bool)dataReader["HasGopro"],
                        Profession = (string)dataReader["Profession"],
                        WorkExperience = (string)dataReader["WorkExperience"],
                        NumberOfAppraisers = (int)dataReader["NumberOfAppraisers"]
                    };
                    if (dataReader["Picture"] != DBNull.Value)
                    {
                        photographer.Image = (byte[])dataReader["Picture"];
                    }
                }
            }

            return photographer;
        }

        public IEnumerable<DriverInfo> GetAllNonApprovedDrivers()
        {
            var drivers = new List<DriverInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllNonApprovedDrivers"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Car car = new Car
                        {
                            Id = (int)dataReader["CarId"],
                            Brand = (string)dataReader["Brand"],
                            NumberOfSeats = (int)dataReader["NumberOfSeats"],
                            FuelType = (string)dataReader["FuelType"],
                            LicensePlate = (string)dataReader["LicensePlate"],
                            HasWiFi = (bool)dataReader["HasWiFi"],
                            HasMicrophone = (bool)dataReader["HasMicrophone"],
                            HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                            HasKitchen = (bool)dataReader["HasKitchen"],
                            HasToilet = (bool)dataReader["HasToilet"],
                        };

                        if (dataReader["CarPicture1"] != DBNull.Value)
                        {
                            car.CarPicture1 = (byte[])dataReader["CarPicture1"];
                        }
                        if (dataReader["CarPicture2"] != DBNull.Value)
                        {
                            car.CarPicture2 = (byte[])dataReader["CarPicture2"];
                        }
                        if (dataReader["CarPicture3"] != DBNull.Value)
                        {
                            car.CarPicture3 = (byte[])dataReader["CarPicture3"];
                        }


                        var driver = new DriverInfo
                        {
                            Id = (int)dataReader["UserId"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DateOfBirth = (DateTime)dataReader["DateOfBirth"],
                            Email = (string)dataReader["Email"],
                            Gender = (string)dataReader["Gender"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            UserName = (string)dataReader["UserName"],
                            Car = car,
                            DrivingLicencePicFront = (byte[])dataReader["DrivingLicencePicFront"],
                            DrivingLicencePicBack = (byte[])dataReader["DrivingLicencePicBack"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        };

                        if(dataReader["Picture"] != System.DBNull.Value)
                        {
                            driver.Image = (byte[])dataReader["Picture"];
                        }
                        drivers.Add(driver);
                    }
                }
            }

            return drivers;
        }

        public DriverInfo GetNonApprovedDriverById(int id)
        {
            var driver = new DriverInfo();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetNonApprovedDriverById"
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    var car = new Car
                    {
                        Id = (int)dataReader["CarId"],
                        Brand = (string)dataReader["Brand"],
                        NumberOfSeats = (int)dataReader["NumberOfSeats"],
                        FuelType = (string)dataReader["FuelType"],
                        HasWiFi = (bool)dataReader["HasWiFi"],
                        HasMicrophone = (bool)dataReader["HasMicrophone"],
                        HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                        HasKitchen = (bool)dataReader["HasKitchen"],
                        HasToilet = (bool)dataReader["HasToilet"]
                    };
                    if (dataReader["CarPicture1"] != DBNull.Value)
                    {
                        car.CarPicture1 = (byte[])dataReader["CarPicture1"];
                    }
                    if (dataReader["CarPicture2"] != DBNull.Value)
                    {
                        car.CarPicture2 = (byte[])dataReader["CarPicture2"];
                    }
                    if (dataReader["CarPicture3"] != DBNull.Value)
                    {
                        car.CarPicture3 = (byte[])dataReader["CarPicture3"];
                    }

                    driver = new DriverInfo
                    {
                        Id = (int)dataReader["UserId"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        UserName = (string)dataReader["UserName"],
                        Car = car,
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        Gender = (string)dataReader["Gender"],
                    };
                    if (dataReader["Picture"] != System.DBNull.Value)
                    {
                        driver.Image = (byte[])dataReader["Picture"];
                    }
                }
            }

            return driver;
        }

        public IEnumerable<GuideInfo> GetAllNonApprovedGuides()
        {
            var guides = new List<GuideInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllNonApprovedGuides"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var guide = new GuideInfo
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            UserName = (string)dataReader["UserName"],
                            Password = (string)dataReader["Password"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Gender = (string)dataReader["Gender"],
                            EducationGrade = (string)dataReader["EducationGrade"],
                            Profession = (string)dataReader["Profession"],
                            WorkExperience = (string)dataReader["WorkExperience"],
                        };
                        if (dataReader["Picture"] != System.DBNull.Value)
                        {
                            guide.Image = (byte[])dataReader["Picture"];
                        }
                        guide.Places = GetGuidePalces(guide.Id);

                        guides.Add(guide);
                    }
                }
            }

            return guides;

        }

        public GuideInfo GetNonApprovedGuideById(int id)
        {
            var guide = new GuideInfo();

            using (var connection = new SqlConnection(sqlConnectionString))
            {

                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetNonApprovedGuideById"
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    guide = new GuideInfo
                    {
                        Id = (int)dataReader["Id"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        UserName = (string)dataReader["UserName"],
                        Gender = (string)dataReader["Gender"],
                        EducationGrade = (string)dataReader["EducationGrade"],
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        Profession = (string)dataReader["Profession"],
                        WorkExperience = (string)dataReader["WorkExperience"],
                    };
                    if (dataReader["Picture"] != System.DBNull.Value)
                    {
                        guide.Image = (byte[])dataReader["Picture"];
                    }
                    guide.Places = GetGuidePalces(id);
                }
            }

            return guide;
        }

        public IEnumerable<PhotographerInfo> GetAllNonApprovedPhotographers()
        {
            var photographers = new List<PhotographerInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllNonApprovedPhotographers"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var photographer = new PhotographerInfo
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            Gender = (string)dataReader["Gender"],
                            DateOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            UserName = (string)dataReader["UserName"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            HasCameraStabilizator = (bool)dataReader["HasCameraStabilizator"],
                            HasDron = (bool)dataReader["HasDron"],
                            HasGopro = (bool)dataReader["HasGopro"],
                            Camera = new Camera
                            {
                                Id = (int)dataReader["CameraId"],
                                Model = (string)dataReader["Model"],
                                IsProfessional = (bool)dataReader["IsProfessional"]
                            },
                            Profession = (string)dataReader["Profession"],
                            WorkExperience = (string)dataReader["WorkExperiance"],
                        };
                        if (dataReader["Picture"] != System.DBNull.Value)
                        {
                            photographer.Image = (byte[])dataReader["Picture"];
                        }
                        photographers.Add(photographer);
                    }
                }
            }

            return photographers;
        }

        public PhotographerInfo GetNonApprovedPhotographerById(int id)
        {
            var photographer = new PhotographerInfo();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetNonApprovedPhotographerById"
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    var camera = new Camera
                    {
                        Id = (int)dataReader["CameraId"],
                        IsProfessional = (bool)dataReader["IsProfessional"],
                        Model = (string)dataReader["Model"]
                    };

                    photographer = new PhotographerInfo
                    {
                        Id = (int)dataReader["Id"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        Gender = (string)dataReader["Gender"],
                        DateOfBirth = (DateTime)dataReader["Age"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        UserName = (string)dataReader["UserName"],
                        Camera = camera,
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        HasCameraStabilizator = (bool)dataReader["HasCameraStabilizator"],
                        HasDron = (bool)dataReader["HasDron"],
                        HasGopro = (bool)dataReader["HasGopro"],
                        Profession = (string)dataReader["Profession"],
                        WorkExperience = (string)dataReader["WorkExperience"],
                    };

                    if (dataReader["Picture"] != System.DBNull.Value)
                    {
                        photographer.Image = (byte[])dataReader["Picture"];
                    }
                }
            }

            return photographer;
        }

        public IEnumerable<CarInfo> GetAllCars()
        {
            var cars = new List<CarInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllCars"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var car = new CarInfo
                        {
                            Id = (int)dataReader["CarId"],
                            DriverId = (int)dataReader["UserId"],
                            Brand = (string)dataReader["Brand"],
                            NumberOfSeats = (int)dataReader["NumberOfSeats"],
                            FuelType = (string)dataReader["FuelType"],
                            HasWiFi = (bool)dataReader["HasWiFi"],
                            HasMicrophone = (bool)dataReader["HasMicrophone"],
                            HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                            HasKitchen = (bool)dataReader["HasKitchen"],
                            HasToilet = (bool)dataReader["HasToilet"],
                        };
                        if (dataReader["CarPicture1"] != DBNull.Value)
                        {
                            car.CarPicture1 = (byte[])dataReader["CarPicture1"];
                        }
                        if (dataReader["CarPicture2"] != DBNull.Value)
                        {
                            car.CarPicture2 = (byte[])dataReader["CarPicture2"];
                        }
                        if (dataReader["CarPicture3"] != DBNull.Value)
                        {
                            car.CarPicture3 = (byte[])dataReader["CarPicture3"];
                        }
                        cars.Add(car);
                    }
                }
            }

            return cars;
        }

        public IEnumerable<CarInfo> GetCarByNumberOfSeats(int numberOfSeats)
        {
            var cars = new List<CarInfo>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetCarsByNumberOfSeats",
                };
                command.Parameters.AddWithValue("@NumberOfSeats", numberOfSeats);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var car = new CarInfo
                        {
                            Id = (int)dataReader["CarId"],
                            DriverId = (int)dataReader["UserId"],
                            Brand = (string)dataReader["Brand"],
                            NumberOfSeats = (int)dataReader["NumberOfSeats"],
                            FuelType = (string)dataReader["FuelType"],
                            HasWiFi = (bool)dataReader["HasWiFi"],
                            HasMicrophone = (bool)dataReader["HasMicrophone"],
                            HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                            HasKitchen = (bool)dataReader["HasKitchen"],
                            HasToilet = (bool)dataReader["HasToilet"],
                            LicensePlate=(string)dataReader["LicensePlate"]
                        };
                        if (dataReader["CarPicture1"] != DBNull.Value)
                        {
                            car.CarPicture1 = (byte[])dataReader["CarPicture1"];
                        }
                        if (dataReader["CarPicture2"] != DBNull.Value)
                        {
                            car.CarPicture2 = (byte[])dataReader["CarPicture2"];
                        }
                        if (dataReader["CarPicture3"] != DBNull.Value)
                        {
                            car.CarPicture3 = (byte[])dataReader["CarPicture3"];
                        }
                        cars.Add(car);
                    }
                }
            }

            return cars;
        }

        public IEnumerable<ServiceRequest> GetAllServicesRequests()
        {
            var serviceRequests = new List<ServiceRequest>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllServicesRequests"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        serviceRequests.Add(new ServiceRequest
                        {
                            Id = (int)dataReader["Id"],
                            UserId = (int)dataReader["UserId"],
                            CampingTripId = (string)dataReader["CampingTripId"]
                        });
                    }
                }
            }

            return serviceRequests;
        }

        public IEnumerable<ServiceRequest> GetServiceRequestsByUserId(int userid)
        {
            var serviceRequests = new List<ServiceRequest>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetServiceRequestsByUserId"
                };
                command.Parameters.AddWithValue("@userId", userid);
                connection.Open();

                

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        serviceRequests.Add(new ServiceRequest
                        {
                            Id=(int)dataReader["Id"],
                            UserId = userid,
                            CampingTripId = (string)dataReader["CampingTripId"]
                        });
                    }
                }
            }

            return serviceRequests;
        }

        public IEnumerable<ServiceRequestResponse> GetAllServicesRequestResponses()
        {
            var serviceRequestResponses = new List<ServiceRequestResponse>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllServicesRequestResponses"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        serviceRequestResponses.Add(new ServiceRequestResponse
                        {
                            Id = (int)dataReader["Id"],
                            UserId = (int)dataReader["UserId"],
                            CampingTripId = (string)dataReader["CampingTripId"],
                            Price = (double)dataReader["Price"]
                        });
                    }
                }
            }

            return serviceRequestResponses;
        }

        public IEnumerable<ServiceRequestResponse> GetServicesRequestResponsesByCampingTripId(string campingTripId)
        {
            var serviceRequestResponses = new List<ServiceRequestResponse>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetServicesRequestResponsesByCampingTripId"
                };
                command.Parameters.AddWithValue("@campingTripId", campingTripId);
                connection.Open();

                

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        serviceRequestResponses.Add(new ServiceRequestResponse
                        {
                            Id = (int)dataReader["Id"],
                            UserId = (int)dataReader["UserId"],
                            CampingTripId = campingTripId,
                            Price = (double)dataReader["Price"]
                        });
                    }
                }
            }

            return serviceRequestResponses;
        }

        public IEnumerable<ServiceRequestResponse> GetServicesRequestResponsesByUserId(int userId)
        {
            var serviceRequestResponses = new List<ServiceRequestResponse>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetServicesRequestResponsesByUserId"
                };

                connection.Open();

                command.Parameters.AddWithValue("@userId", userId);

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        serviceRequestResponses.Add(new ServiceRequestResponse
                        {
                            Id = (int)dataReader["Id"],
                            UserId = userId,
                            CampingTripId = (string)dataReader["CampingTripId"],
                            Price = (double)dataReader["Price"]
                        });
                    }
                }
            }

            return serviceRequestResponses;
        }
        
        #endregion Getting

        #region Updating
        /// <summary>
        /// Updates User info
        /// </summary>
        /// <param name="user"> User new info </param>
        public void UpdateUserInfo(UserInfo user)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateUserInfo"
                };

                updateCommand.Parameters.AddWithValue("@id", user.Id);
                updateCommand.Parameters.AddWithValue("@firstName", user.FirstName);
                updateCommand.Parameters.AddWithValue("@lastName", user.LastName);
                updateCommand.Parameters.AddWithValue("@gender", user.Gender);
                updateCommand.Parameters.AddWithValue("@dateOfBirth", user.DateOfBirth);
                updateCommand.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                updateCommand.Parameters.AddWithValue("@picture", user.Image);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates Driver info
        /// </summary>
        /// <param name="driver"> new Driver </param>
        public void UpdateDriverInfo(DriverInfo driver)
        {
            var user = new UserInfo
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Gender = driver.Gender,
                DateOfBirth = driver.DateOfBirth,
                PhoneNumber = driver.PhoneNumber,
                Image = driver.Image
            };

            UpdateUserInfo(user);

            UpdateCarInfo(driver.Car);

            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateDriverInfo"
                };

                updateCommand.Parameters.AddWithValue("@userId", driver.Id);
                updateCommand.Parameters.AddWithValue("@drivingLicencePicFront", driver.DrivingLicencePicFront);
                updateCommand.Parameters.AddWithValue("@drivingLicencePicBack", driver.DrivingLicencePicBack);
                updateCommand.Parameters.AddWithValue("@knowledgeOfLanguages", driver.KnowledgeOfLanguages);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        public void UpdateCarInfo(Car car)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateCarInfo"
                };

                updateCommand.Parameters.AddWithValue("@carId", car.Id);
                updateCommand.Parameters.AddWithValue("@barnd", car.Brand);
                updateCommand.Parameters.AddWithValue("@fuelType", car.FuelType);
                updateCommand.Parameters.AddWithValue("@numberOfSeats", car.NumberOfSeats);
                updateCommand.Parameters.AddWithValue("@licensePlate", car.LicensePlate);
                updateCommand.Parameters.AddWithValue("@carPicture1", car.CarPicture1);
                updateCommand.Parameters.AddWithValue("@carPicture2", car.CarPicture2);
                updateCommand.Parameters.AddWithValue("@carPicture3", car.CarPicture3);
                updateCommand.Parameters.AddWithValue("@hasAirConditioner", car.HasAirConditioner);
                updateCommand.Parameters.AddWithValue("@hasWiFi", car.HasWiFi);
                updateCommand.Parameters.AddWithValue("@hasToilet", car.HasToilet);
                updateCommand.Parameters.AddWithValue("@hasMicrophone", car.HasMicrophone);
                updateCommand.Parameters.AddWithValue("@hasKitchen", car.HasKitchen);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        public void UpdateApproveValue(string userName, bool value)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var approveUpdateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateApproveValue"
                };

                approveUpdateCommand.Parameters.AddWithValue("@userName", userName);
                approveUpdateCommand.Parameters.AddWithValue("@approveNewValue", value);

                connection.Open();

                approveUpdateCommand.ExecuteNonQuery();
            }
        }

        public void UpdateGuideInfo(GuideInfo guide)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateGuideInfo"
                };

                updateCommand.Parameters.AddWithValue("@id", guide.Id);
                updateCommand.Parameters.AddWithValue("@firstName", guide.FirstName);
                updateCommand.Parameters.AddWithValue("@lastName", guide.LastName);
                updateCommand.Parameters.AddWithValue("@gender", guide.Gender);
                updateCommand.Parameters.AddWithValue("@dateOfBirth", guide.DateOfBirth);
                updateCommand.Parameters.AddWithValue("@phoneNumber", guide.PhoneNumber);
                updateCommand.Parameters.AddWithValue("@picture", guide.Image);
                updateCommand.Parameters.AddWithValue("@workExperience", guide.WorkExperience);
                updateCommand.Parameters.AddWithValue("@profession", guide.Profession);
                updateCommand.Parameters.AddWithValue("@knowledgeOfLanguages", guide.KnowledgeOfLanguages);
                updateCommand.Parameters.AddWithValue("@educationGrade", guide.EducationGrade);
                
                connection.Open();

                updateCommand.ExecuteNonQuery();
            }

            UpadateGuidePlaces(guide.Id, guide.Places);
        }

        public void UpadateGuidePlaces(int id, List<string> places)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                for(int i = 0; i < places.Count; i++)
                {
                    var command = new SqlCommand()
                    {
                        Connection = connection,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "UpdateGuidePlaces"
                    };

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@places", places[i]);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Updates Photographer info
        /// </summary>
        /// <param name="photographer"> New Photographer </param>
        public void UpdatePhotographerInfo(PhotographerInfo photographer)
        {
            var user = new UserInfo
            {
                Id = photographer.Id,
                FirstName = photographer.FirstName,
                LastName = photographer.LastName,
                Gender = photographer.Gender,
                DateOfBirth = photographer.DateOfBirth,
                PhoneNumber = photographer.PhoneNumber,
                Image = photographer.Image
            };

            UpdateUserInfo(user);

            UpdateCamera(photographer.Camera);

            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdatePhotographerInfo"
                };

                updateCommand.Parameters.AddWithValue("@userId", photographer.Id);
                updateCommand.Parameters.AddWithValue("@profession", photographer.FirstName);
                updateCommand.Parameters.AddWithValue("@knowledgeOfLanguages", photographer.LastName);
                updateCommand.Parameters.AddWithValue("@workExperience", photographer.Gender);
                updateCommand.Parameters.AddWithValue("@hasDron", photographer.DateOfBirth);
                updateCommand.Parameters.AddWithValue("@hasCameraStabilizator", photographer.PhoneNumber);
                updateCommand.Parameters.AddWithValue("@hasGopro", photographer.Email);
                updateCommand.Parameters.AddWithValue("@rating", photographer.Raiting);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Updates camera info
        /// </summary>
        /// <param name="camera"> New Camera </param>
        public void UpdateCamera(Camera camera)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateCameraInfo"
                };

                updateCommand.Parameters.AddWithValue("@id", camera.Id);
                updateCommand.Parameters.AddWithValue("@isProfessional", camera.IsProfessional);
                updateCommand.Parameters.AddWithValue("@model", camera.Model);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        public void UpdateServiceRequestResponse(int id, ServiceRequestResponse response)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateServiceRequestResponse"
                };

                updateCommand.Parameters.AddWithValue("@id", response.Id);
                updateCommand.Parameters.AddWithValue("@price", response.Price);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        #endregion Updating

        #region Deleting

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <param name="id"> User id </param>
        public void DeleteUser(int id)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteUser"
                };

                deleteCommand.Parameters.AddWithValue("@id", id);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        public void DeleteGuide(int id)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteGuide"
                };

                deleteCommand.Parameters.AddWithValue("@id", id);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes Driver and  his car from database
        /// </summary>
        /// <param name="id"> Driver id </param>
        public void DeleteDriver(int id)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteDriver"
                };

                deleteCommand.Parameters.AddWithValue("@id", id);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes Photographer and  his camera from database
        /// </summary>
        /// <param name="id"> Photographer id </param>
        public void DeletePhotographer(int id)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeletePhotographer"
                };

                deleteCommand.Parameters.AddWithValue("@id", id);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        public void DeleteUserFromCampingTrips(int id)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteUserFromCampingTrips"
                };

                deleteCommand.Parameters.AddWithValue("@id", id);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        public void DeleteDriverFromCampingTrips(int id)
        {
            /*var client = new MongoClient(mongoDbConnectionString);
            if (client != null)
            {
                var database = client.GetDatabase(mongoDataBase);
                var campingTrips = database.GetCollection<CampingTrip>("CampingTrips");

                var filter = Builders<CampingTrip>.Filter.Eq(s => s.DriverID, id);

                var trips = campingTrips.Find(filter).ToListAsync();

                foreach(var trip in trips.Result)
                {
                    trip.DriverID = 0;
                }
            }*/
        }

        public void DeletePhotographerFromCampingTrips(int id)
        {
           /* var client = new MongoClient(mongoDbConnectionString);
            if (client != null)
            {
                var database = client.GetDatabase(mongoDataBase);
                var campingTrips = database.GetCollection<CampingTrip>("CampingTrips");

                var filter = Builders<CampingTrip>.Filter.Eq(s => s.PhotographerID, id);

                var trips = campingTrips.Find(filter).ToListAsync();

                foreach (var trip in trips.Result)
                {
                    trip.PhotographerID = 0;
                }
            }*/
        }

        public void DeleteGuideFromCampingTrips(int id)
        {
            /*var client = new MongoClient(mongoDbConnectionString);
            if (client != null)
            {
                var database = client.GetDatabase(mongoDataBase);
                var campingTrips = database.GetCollection<CampingTrip>("CampingTrips");

                var filter = Builders<CampingTrip>.Filter.Eq(s => s.GuideID, id);

                var trips = campingTrips.Find(filter).ToListAsync();

                foreach (var trip in trips.Result)
                {
                    trip.GuideID = 0;
                }
            }*/
        }

        public void DeleteUserVerification(string userName)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteUserVerification"
                };

                deleteCommand.Parameters.AddWithValue("@userName", userName);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        public void DeleteDriverVerification(string userName)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteDriverVerification"
                };

                deleteCommand.Parameters.AddWithValue("@userName", userName);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        public void DeleteGuideVerification(string userName)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteGuideVerification"
                };

                deleteCommand.Parameters.AddWithValue("@userName", userName);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        public void DeletePhotographerVerification(string userName)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeletePhotographerVerification"
                };

                deleteCommand.Parameters.AddWithValue("@userName", userName);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        public void DeleteServicesRequests(int id)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteServiceRequest"
                };

                deleteCommand.Parameters.AddWithValue("@id", id);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        public void DeleteServiceRequestResponse(int id)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var deleteCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteServiceRequestResponse"
                };

                deleteCommand.Parameters.AddWithValue("@id", id);

                connection.Open();

                deleteCommand.ExecuteNonQuery();
            }
        }

        #endregion Deleting

        #region Validating

        public async Task<bool> IsOrganaizer(int id)
        {
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("campingTrip", "secret");

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await httpClient.GetAsync($"IsOrganizer/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                var isOrganizerJson = await content.ReadAsStringAsync();

                var isOrganizer = JsonConvert.DeserializeObject<bool>(isOrganizerJson);

                return isOrganizer;
            }
            else
            {
                throw new Exception("Camping Trip Api not responded");
            }
        }

        public bool CodeIsValid(VerificationInfo verification)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetVerificationCode"
                };

                command.Parameters.AddWithValue("@userName", verification.UserName);

                connection.Open();

                var reader = command.ExecuteReader();

                if (!reader.HasRows) return false;

                reader.Read();

                if ((int)reader["Code"] == verification.Code) return true;

                return false;
            }
        }

        public bool IsValidUserName(string userName)
        {
            using (var connection = new SqlConnection(this.sqlConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UserNameCount"
                };

                command.Parameters.AddWithValue("@userName", userName);

                connection.Open();

                var count = (int)command.ExecuteScalar();

                if (count == 0) return true;

                return false;
            }
        }

        #endregion Validating
    }
}