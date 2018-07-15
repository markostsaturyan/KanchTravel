using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Veldrid.ImageSharp;
using UsersDataAccesLayer.DalDataModel;


namespace UsersDataAccesLayer
{
    /// <summary>
    /// Users data access layer class
    /// </summary>
    public static class UsersDAL
    {
        /// <summary>
        /// Connection string
        /// </summary>
        private static readonly string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog = UsersDB;Integrated Security=True;Pooling=False;Connect Timeout=30";

        /// <summary>
        /// Stored procedures from json file
        /// </summary>
        private static readonly IConfiguration storedProcedures = (IConfiguration)new ConfigurationBuilder()
                                                         .SetBasePath(Directory.GetCurrentDirectory())
                                                         .AddJsonFile("storedProcedures.json");
        
        /// <summary>
        /// Adds user to database
        /// </summary>
        /// <param name="user"> The user full info </param>
        /// <returns> User's id after inserting </returns>
        public static int AddUser(UserFull user)
        {
            var userGuid = Guid.NewGuid();

            string hashedPassword = Security.HashSHA1(user.Password + userGuid.ToString());

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "INSERT INTO UsersDB.dbo.Users output INSERTED.Id VAULES(@firstName, @lastName, @sex, @dateOfBirth, @phoneNumber, @email, @picture, @username, @password, @isActive)"
                };

                command.Parameters.AddWithValue("@firstName", user.FirstName);
                command.Parameters.AddWithValue("@lastName", user.LastName);
                command.Parameters.AddWithValue("@sex", user.Sex);
                command.Parameters.AddWithValue("@dateOfBirth", user.DataOfBirth);
                command.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@picture", user.Image);
                command.Parameters.AddWithValue("@username", user.UserName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@userguid", userGuid.ToString());
                command.Parameters.AddWithValue("@isActive", user.IsActive);

                return (int)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Adds Guide to database
        /// </summary>
        /// <param name="guide"> The guide full info </param>
        public static void AddGuide(GuideFull guide)
        {
            var userGuid = Guid.NewGuid();

            string hashedPassword = Security.HashSHA1(guide.Password + userGuid.ToString());

            var userFullInfo = new UserFull
            {
                FirstName = guide.FirstName,
                LastName = guide.LastName,
                Sex = guide.Sex,
                DataOfBirth = guide.DataOfBirth,
                PhoneNumber = guide.PhoneNumber,
                Email = guide.Email,
                Image = guide.Image,
                UserName = guide.UserName,
                Password = hashedPassword,
                IsActive = guide.IsActive,
                UserGuid = userGuid.ToString()
            };

            var userId = AddUser(userFullInfo);

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "INSERT INTO UsersDB.dbo.Guide VAULES(@userId, @educationGrade, @profession, @knowledgeOfLanguages, @workExperience, @rating)"
                };

                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@educationGrade", guide.EducationGrade);
                command.Parameters.AddWithValue("@profession", guide.Profession);
                command.Parameters.AddWithValue("@knowledgeOfLanguages", guide.KnowledgeOfLanguages);
                command.Parameters.AddWithValue("@workExperience", guide.WorkExperience);
                command.Parameters.AddWithValue("@rating", guide.Raiting);

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Adds Driver into database
        /// </summary>
        /// <param name="driver"> Driver full info </param>
        public static void AddDriver(DriverFull driver)
        {
            var userGuid = Guid.NewGuid();

            string hashedPassword = Security.HashSHA1(driver.Password + userGuid.ToString());

            var userFullInfo = new UserFull
            {
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Sex = driver.Sex,
                DataOfBirth = driver.DataOfBirth,
                PhoneNumber = driver.PhoneNumber,
                Email = driver.Email,
                Image = driver.Image,
                UserName = driver.UserName,
                Password = hashedPassword,
                IsActive = driver.IsActive,
                UserGuid = userGuid.ToString()
            };

            var userId = AddUser(userFullInfo);
            var carId = AddCar(driver.Car);

            using (var connection = new SqlConnection(connectionString))
            {
                var commandForInsertDriver = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "INSERT INTO UsersDB.dbo.Drivers VAULES(@userId, @carId, @drivingLicencePicFront, @drivingLicencePicBack, @knowledgeOfLanguages, @rating)"
                };

                commandForInsertDriver.Parameters.AddWithValue("@userId", userId);
                commandForInsertDriver.Parameters.AddWithValue("@carId", carId);
                commandForInsertDriver.Parameters.AddWithValue("@drivingLicencePicFront", driver.DrivingLicencePicFront);
                commandForInsertDriver.Parameters.AddWithValue("@drivingLicencePicBack", driver.DrivingLicencePicBack);
                commandForInsertDriver.Parameters.AddWithValue("@knowledgeOfLanguages", driver.KnowledgeOfLanguages);
                commandForInsertDriver.Parameters.AddWithValue("@rating", driver.Raiting);

                commandForInsertDriver.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Adds Driver's car to database
        /// </summary>
        /// <param name="car"> Car full info </param>
        /// <returns> Car's id after inserting </returns>
        public static int AddCar(Car car)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var commandForInsertCar = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "INSERT INTO UsersDB.dbo.Car output INSERTED.Id VAULES(@brand, @numberOfSeats, @fuelType, @carPicture1, @carPicture2, @carPicture3, @licensePlate, @hasWiFi, @hasMicrophone, @hasAirConditioner, @hasKitchen, @hasToilet)"
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


                return (int)commandForInsertCar.ExecuteScalar();
            }
        }

        /// <summary>
        /// Gets all user identifiers from database
        /// </summary>
        /// <returns> List of identifiers </returns>
        public static List<UserIdentifiers> GetAllUserIdentifiers()
        {
            var usersIdentifiers = new List<UserIdentifiers>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllUserIdentifiers"
                };

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        usersIdentifiers.Add(new UserIdentifiers
                        {
                            Id = (int)dataReader["Id"],
                            FirstName=(string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            UserName = (string)dataReader["UserName"],
                            Password = (string)dataReader["Password"],
                            Email = (string)dataReader["Email"],
                            Role = (string)dataReader["Role"]
                        });
                    }
                }
            }

            return usersIdentifiers;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns> List of Users </returns>
        public static List<UserFull> GetAllUsers()
        {
            var users = new List<UserFull>();

            using (var connection = new SqlConnection(connectionString))
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
                        users.Add(new UserFull
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = (ImageSharpTexture)dataReader["Image"],
                            UserName = (string)dataReader["UserName"],
                            Password = (string)dataReader["Password"],
                            Role = (string)dataReader["Role"],
                            IsActive = (bool)dataReader["IsActive"]

                        });
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Gets all drivers
        /// </summary>
        /// <returns> List of drivers </returns>
        public static List<DriverFull> GetAllDrivers()
        {
            var drivers = new List<DriverFull>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllDrivers"
                };

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
                            CarPicture1 = (ImageSharpTexture)dataReader["CarPicture1"],
                            CarPicture2 = (ImageSharpTexture)dataReader["CarPicture2"],
                            CarPicture3 = (ImageSharpTexture)dataReader["CarPicture3"],
                            LicensePlate = (string)dataReader["LicensePlate"],
                            HasWiFi = (bool)dataReader["HasWiFi"],
                            HasMicrophone = (bool)dataReader["HasMicrophone"],
                            HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                            HasKitchen = (bool)dataReader["HasKitchen"],
                            HasToilet = (bool)dataReader["HasToilet"]
                        };

                        drivers.Add(new DriverFull
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = (ImageSharpTexture)dataReader["Image"],
                            UserName = (string)dataReader["UserName"],
                            Password = (string)dataReader["Password"],
                            Car = car,
                            DrivingLicencePicFront=(ImageSharpTexture)dataReader["DrivingLicencePicFront"],
                            DrivingLicencePicBack = (ImageSharpTexture)dataReader["DrivingLicencePicBack"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Raiting = (double)dataReader["Raiting"],
                            IsActive = (bool)dataReader["IsActive"]
                        });
                    }
                }
            }

            return drivers;
        }

        /// <summary>
        /// Gets all guides
        /// </summary>
        /// <returns> List of guides </returns>
        public static List<GuideFull> GetAllGuides()
        {
            var guides = new List<GuideFull>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllGuides"
                };

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var guide = new GuideFull
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = (ImageSharpTexture)dataReader["Image"],
                            UserName = (string)dataReader["UserName"],
                            Password = (string)dataReader["Password"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Raiting = (double)dataReader["Raiting"],
                            IsActive = (bool)dataReader["IsActive"]
                        };

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
        private static List<string> GetGuidePalces(int id)
        {
            var places = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetGuidePlaces"
                };

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
        public static List<PhotographerFull> GetAllPhotographers()
        {
            var photographers = new List<PhotographerFull>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetAllPhotographers"
                };

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        var photographer = new PhotographerFull
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = (ImageSharpTexture)dataReader["Image"],
                            UserName = (string)dataReader["UserName"],
                            Password = (string)dataReader["Password"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Raiting = (double)dataReader["Raiting"],
                            HasCameraStabilizator = (bool)dataReader["HasCameraStabilizator"],
                            HasDron = (bool)dataReader["HasDron"],
                            HasGopro = (bool)dataReader["HasGopro"],
                            Camera = new Camera
                            {
                                Id = (int)dataReader["CameraId"],
                                Model=(string)dataReader["Model"],
                                IsProfessional =(bool)dataReader["IsProfessional"]
                            },
                            Profession = (string)dataReader["Profession"],
                            WorkExperience = (string)dataReader["WorkExperiance"],
                            IsActive = (bool)dataReader["IsActive"]
                        };

                        photographers.Add(photographer);
                    }
                }
            }

            return photographers;
        }

        /// <summary>
        /// Gets user by username from database
        /// </summary>
        /// <param name="userName"> The username </param>
        /// <returns> User with that username </returns>
        public static UserIdentifiers GetByUserName(string userName)
        {
            var user = new UserIdentifiers();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetByUserName"
                };

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        user.Id = (int)dataReader["Id"];
                        user.FirstName = (string)dataReader["FirstName"];
                        user.LastName = (string)dataReader["LastName"];
                        user.Email = (string)dataReader["Email"];
                        user.UserName = (string)dataReader["UserName"];
                        user.Password = (string)dataReader["Password"];
                        user.IsActive = (bool)dataReader["IsActive"];
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// Gets user by id from database
        /// </summary>
        /// <param name="id"> User's id </param>
        /// <returns> User with that id </returns>
        public static UserIdentifiers GetByUserID(long id)
        {
            var user = new UserIdentifiers();
            using (var connection = new SqlConnection(connectionString))
            {
                
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetByUserId"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        user.Id = (int)dataReader["Id"];
                        user.FirstName = (string)dataReader["FirstName"];
                        user.LastName = (string)dataReader["LastName"];
                        user.Email = (string)dataReader["Email"];
                        user.UserName = (string)dataReader["UserName"];
                        user.Password = (string)dataReader["Password"];
                        user.IsActive = (bool)dataReader["IsActive"];
                    }
                }
            }
            return user;
        }
    }
}
