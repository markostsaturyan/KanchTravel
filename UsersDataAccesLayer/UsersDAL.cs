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

        public static Dictionary<string,string> GetAllUserNameAndPasswords()
        {
            var userNameAndPasswords = new Dictionary<string, string>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = storedProcedures["GetAllUserNameAndPasswords"]
                };

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        userNameAndPasswords.Add((string)dataReader["UserName"], (string)dataReader["Password"]);
                    }
                }
            }

            return userNameAndPasswords;
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
                            Password = (string)dataReader["Password"]
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
                            Raiting = (double)dataReader["Raiting"]
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
                        guides.Add(new GuideFull
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
                            Raiting = (double)dataReader["Raiting"]
                        });
                    }
                }
            }

            return guides;
        }

    }


}
