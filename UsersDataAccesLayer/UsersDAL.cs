using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data.Common;
using Veldrid.ImageSharp;
using UsersDataAccesLayer.DalDataModel;

namespace UsersDataAccesLayer
{
    public static class UsersDAL
    {
        private static string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog = UsersDB;Integrated Security=True;Pooling=False;Connect Timeout=30";

        private static IConfiguration storedProcedures = (IConfiguration)new ConfigurationBuilder()
                                                         .SetBasePath(Directory.GetCurrentDirectory())
                                                         .AddJsonFile("storedProcedures.json");

        public static List<UserIdentifiers> GetAllUserIdentifiers()
        {
            var usersIdentifiers = new List<UserIdentifiers>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = storedProcedures["GetAllUserIdentifiers"]
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

        public static List<UserFull> GetAllUsers()
        {
            var users = new List<UserFull>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = storedProcedures["GetAllUsers"]
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

        public static List<DriverFull> GetAllDrivers()
        {
            var drivers = new List<DriverFull>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = storedProcedures["GetAllDrivers"]
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

        public static List<GuideFull> GetAllGuides()
        {
            var guides = new List<GuideFull>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = storedProcedures["GetAllGuides"]
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

        private static List<string> GetGuidePalces(int id)
        {
            var places = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = storedProcedures["GetGuidePlaces"]
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

        public static List<PhotographerFull> GetAllPhotographers()
        {
            var photographers = new List<PhotographerFull>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = storedProcedures["GetAllPhotographers"]
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

    }
}
