using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.DataManagnment.Security;
using Veldrid.ImageSharp;

namespace UserManagement.DataManagnment.DataAccesLayer
{
    public class DataAccesLayer
    {
        #region Constructors and fields

        /// <summary>
        /// Connection string
        /// </summary>
        private readonly string connectionString;

        public DataAccesLayer(string connectionString)
        {
            this.connectionString = connectionString;
        }



        #endregion Constructors and fields

        #region Adding
        /// <summary>
        /// Adds user to database
        /// </summary>
        /// <param name="user"> The user full info </param>
        /// <returns> User's id after inserting </returns>
        public int AddUser(UserInfo user)
        {
            var userGuid = Guid.NewGuid().ToString();

            string hashedPassword = SecurityForPassword.HashSHA1(user.Password + userGuid);

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertUser"
                };

                command.Parameters.AddWithValue("@firstName", user.FirstName);
                command.Parameters.AddWithValue("@lastName", user.LastName);
                command.Parameters.AddWithValue("@gander", user.Gender);
                command.Parameters.AddWithValue("@dateOfBirth", user.DataOfBirth);
                command.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@picture", ImageToByteArray(user.Image));
                command.Parameters.AddWithValue("@username", user.UserName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@userguid", userGuid);

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
        public int AddUserVerification(int id)
        {
            using (var connection = new SqlConnection())
            {
                var verifCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertUserVerificationCode"
                };

                var rand = new Random();

                var randNumber = rand.Next(1000, 9999);

                verifCommand.Parameters.AddWithValue("@userId", id);
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
                DataOfBirth = guide.DataOfBirth,
                PhoneNumber = guide.PhoneNumber,
                Email = guide.Email,
                Image = guide.Image,
                UserName = guide.UserName,
                Password = guide.Password
            };

            var userId = AddUser(userInfo);

            AddGuidePlaces(guide.Id, guide.Places);

            using (var connection = new SqlConnection(connectionString))
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
            using (var connection = new SqlConnection(this.connectionString))
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
                    command.Parameters.AddWithValue("@places", places[i]);

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
                DataOfBirth = driver.DataOfBirth,
                PhoneNumber = driver.PhoneNumber,
                Email = driver.Email,
                Image = driver.Image,
                UserName = driver.UserName,
                Password = driver.Password,
            };

            var userId = AddUser(userFullInfo);
            var carId = AddCar(driver.Car);

            using (var connection = new SqlConnection(connectionString))
            {
                var commandForInsertDriver = new SqlCommand
                {
                    Connection = connection,
                    CommandType=System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertDriverInfo"
                };

                commandForInsertDriver.Parameters.AddWithValue("@userId", userId);
                commandForInsertDriver.Parameters.AddWithValue("@carId", carId);
                commandForInsertDriver.Parameters.AddWithValue("@drivingLicencePicFront", ImageToByteArray(driver.DrivingLicencePicFront));
                commandForInsertDriver.Parameters.AddWithValue("@drivingLicencePicBack", ImageToByteArray(driver.DrivingLicencePicBack));
                commandForInsertDriver.Parameters.AddWithValue("@knowledgeOfLanguages", driver.KnowledgeOfLanguages);

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
            using (var connection = new SqlConnection(connectionString))
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
                commandForInsertCar.Parameters.AddWithValue("@carPicture1", ImageToByteArray(car.CarPicture1));
                commandForInsertCar.Parameters.AddWithValue("@carPicture2", ImageToByteArray(car.CarPicture2));
                commandForInsertCar.Parameters.AddWithValue("@carPicture3", ImageToByteArray(car.CarPicture3));
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
                DataOfBirth = photographer.DataOfBirth,
                PhoneNumber = photographer.PhoneNumber,
                Email = photographer.Email,
                Image = photographer.Image,
                UserName = photographer.UserName,
                Password = photographer.Password
            };

            var userId = AddUser(userFullInfo);
            var cameraId = AddCamera(photographer.Camera);

            using (var connection = new SqlConnection(connectionString))
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
                commandForInsertPhotographer.Parameters.AddWithValue("@raiting", photographer.Raiting);

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
            using (var connection = new SqlConnection(connectionString))
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

            using (var connection = new SqlConnection(connectionString))
            {

                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetFullUserInfoById"
                };

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
                        DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        Image = ByteArrayToImage(dataReader["Image"]),
                        UserName = (string)dataReader["UserName"],
                        Gender = (string)dataReader["Gender"]
                    };

                }
            }

            return user;
        }

        public GuideInfo GetGuideById(int id)
        {
            var guide = new GuideInfo();

            using (var connection = new SqlConnection(connectionString))
            {

                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetGuideInfoById"
                };

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
                        DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        Image = ByteArrayToImage(dataReader["Image"]),
                        UserName = (string)dataReader["UserName"],
                        Gender = (string)dataReader["Gender"],
                        EducationGrade = (string)dataReader["EducationGrade"],
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        Profession = (string)dataReader["Profession"],
                        Rating = (double)dataReader["Raiting"],
                        WorkExperience = (string)dataReader["WorkExperience"],
                        NumberOfAppraisers = (int)dataReader["NumberOfAppraisers"],
                    };

                    guide.Places = GetGuidePalces(id);

                }
            }

            return guide;
        }

        public string GetUserEmailById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUserEmailById"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                dataReader.Read();

                return (string)dataReader["Email"];
            }
        }

        public string GetUserPasswordById(int id, out string guide)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUserPasswordAndGuideById"
                };

                connection.Open();

                var dataReader = command.ExecuteReader();

                dataReader.Read();

                guide = (string)dataReader["UserGuide"];

                return (string)dataReader["Password"];
            }
        }

        public string GetUserNamePasswordGuideAndEmailById(int id, out string password, out string guide, out string email)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUserNamePasswordGuideAndEmailById"
                };

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                var dataReader = command.ExecuteReader();

                dataReader.Read();

                guide = (string)dataReader["UserGuide"];
                password = (string)dataReader["Password"];
                email = (string)dataReader["Email"];

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
                        users.Add(new UserInfo
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = ByteArrayToImage(dataReader["Picture"]),
                            UserName = (string)dataReader["UserName"],
                            Gender = (string)dataReader["Gender"]
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
        public List<DriverInfo> GetAllDrivers()
        {
            var drivers = new List<DriverInfo>();

            using (var connection = new SqlConnection(connectionString))
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
                            CarPicture1 = ByteArrayToImage(dataReader["CarPicture1"]),
                            CarPicture2 = ByteArrayToImage(dataReader["CarPicture2"]),
                            CarPicture3 = ByteArrayToImage(dataReader["CarPicture3"]),
                            LicensePlate = (string)dataReader["LicensePlate"],
                            HasWiFi = (bool)dataReader["HasWiFi"],
                            HasMicrophone = (bool)dataReader["HasMicrophone"],
                            HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                            HasKitchen = (bool)dataReader["HasKitchen"],
                            HasToilet = (bool)dataReader["HasToilet"],
                        };

                        drivers.Add(new DriverInfo
                        {
                            Id = (int)dataReader["UserId"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            Gender=(string)dataReader["Gender"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = ByteArrayToImage(dataReader["Picture"]),
                            UserName = (string)dataReader["UserName"],
                            Car = car,
                            DrivingLicencePicFront = ByteArrayToImage(dataReader["DrivingLicencePicFront"]),
                            DrivingLicencePicBack = ByteArrayToImage(dataReader["DrivingLicencePicBack"]),
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Rating = (double)dataReader["Rating"],
                            NumberOfAppraisers = (int)dataReader["NumberOfAppraisers"]
                        });
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

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetDriverById"
                };

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
                        CarPicture1 = ByteArrayToImage(dataReader["CarPicture1"]),
                        CarPicture2 = ByteArrayToImage(dataReader["CarPicture2"]),
                        CarPicture3 = ByteArrayToImage(dataReader["CarPicture3"]),
                        HasWiFi = (bool)dataReader["HasWiFi"],
                        HasMicrophone = (bool)dataReader["HasMicrophone"],
                        HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                        HasKitchen = (bool)dataReader["HasKitchen"],
                        HasToilet = (bool)dataReader["HasToilet"]
                    };

                    driver = new DriverInfo
                    {
                        Id = (int)dataReader["UserId"],
                        FirstName = (string)dataReader["FirstName"],
                        LastName = (string)dataReader["LastName"],
                        DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        Image = ByteArrayToImage(dataReader["Image"]),
                        UserName = (string)dataReader["UserName"],
                        Car = car,
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        Rating = (double)dataReader["Rating"],
                        Gender=(string)dataReader["Gender"],
                        NumberOfAppraisers=(int)dataReader["NumberOfAppraisers"]
                    };
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

            using (var connection = new SqlConnection(connectionString))
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
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = ByteArrayToImage(dataReader["Picture"]),
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

            using (var connection = new SqlConnection(connectionString))
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

            using (var connection = new SqlConnection(connectionString))
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
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = ByteArrayToImage(dataReader["Image"]),
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

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetPhotographerById"
                };

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
                        DataOfBirth = (DateTime)dataReader["Age"],
                        Email = (string)dataReader["Email"],
                        PhoneNumber = (string)dataReader["PhoneNumber"],
                        Image = ByteArrayToImage(dataReader["Picture"]),
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
                }
            }

            return photographer;
        }
        #endregion Getting

        #region Updating
        /// <summary>
        /// Updates User info
        /// </summary>
        /// <param name="user"> User new info </param>
        public void UpdateUserInfo(UserInfo user)
        {
            using (var connection = new SqlConnection(this.connectionString))
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
                updateCommand.Parameters.AddWithValue("@dateOfBirth", user.DataOfBirth);
                updateCommand.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                updateCommand.Parameters.AddWithValue("@picture", ImageToByteArray(user.Image));

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
                DataOfBirth = driver.DataOfBirth,
                PhoneNumber = driver.PhoneNumber,
                Image = driver.Image
            };

            UpdateUserInfo(user);

            UpdateCarInfo(driver.Car);

            using (var connection = new SqlConnection(this.connectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateDriverInfo"
                };

                updateCommand.Parameters.AddWithValue("@userId", driver.Id);
                updateCommand.Parameters.AddWithValue("@drivingLicencePicFront", ImageToByteArray(driver.DrivingLicencePicFront));
                updateCommand.Parameters.AddWithValue("@drivingLicencePicBack", ImageToByteArray(driver.DrivingLicencePicBack));
                updateCommand.Parameters.AddWithValue("@knowledgeOfLanguages", driver.KnowledgeOfLanguages);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        public void UpdateCarInfo(Car car)
        {
            using (var connection = new SqlConnection(this.connectionString))
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
                updateCommand.Parameters.AddWithValue("@carPicture1", ImageToByteArray(car.CarPicture1));
                updateCommand.Parameters.AddWithValue("@carPicture2", ImageToByteArray(car.CarPicture2));
                updateCommand.Parameters.AddWithValue("@carPicture3", ImageToByteArray(car.CarPicture3));
                updateCommand.Parameters.AddWithValue("@hasAirConditioner", car.HasAirConditioner);
                updateCommand.Parameters.AddWithValue("@hasWiFi", car.HasWiFi);
                updateCommand.Parameters.AddWithValue("@hasToilet", car.HasToilet);
                updateCommand.Parameters.AddWithValue("@hasMicrophone", car.HasMicrophone);
                updateCommand.Parameters.AddWithValue("@hasKitchen", car.HasKitchen);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        public void UpdateApproveValue(int id, int value)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                var setApproveFalseCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateApproveValue"
                };

                setApproveFalseCommand.Parameters.AddWithValue("@id", id);
                setApproveFalseCommand.Parameters.AddWithValue("@approveNewValue", value);

                connection.Open();

                setApproveFalseCommand.ExecuteNonQuery();
            }
        }

        public void UpdateGuideInfo(GuideInfo guide)
        {
            using (var connection = new SqlConnection(this.connectionString))
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
                updateCommand.Parameters.AddWithValue("@dateOfBirth", guide.DataOfBirth);
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
            using (var connection = new SqlConnection(this.connectionString))
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
                DataOfBirth = photographer.DataOfBirth,
                PhoneNumber = photographer.PhoneNumber,
                Image = photographer.Image
            };

            UpdateUserInfo(user);

            UpdateCamera(photographer.Camera);

            using (var connection = new SqlConnection(this.connectionString))
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
                updateCommand.Parameters.AddWithValue("@hasDron", photographer.DataOfBirth);
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
            using (var connection = new SqlConnection(this.connectionString))
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

        #endregion Updating

        #region Deleting

        /// <summary>
        /// Deletes user from database
        /// </summary>
        /// <param name="id"> User id </param>
        public void DeleteUser(int id)
        {
            using (var connection = new SqlConnection(this.connectionString))
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
            using (var connection = new SqlConnection(this.connectionString))
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
            using (var connection = new SqlConnection(this.connectionString))
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
            using (var connection = new SqlConnection(this.connectionString))
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

        #endregion Deleting

        #region Validating

        public bool UsarNameValidating(string userName)
        {
            using (var connection = new SqlConnection(this.connectionString))
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

        #region Utility

        public Image ByteArrayToImage(object picture)
        {
            Image img = null;
            if (picture != null)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, picture);
                    var bytesArr = ms.ToArray();
                    var memstr = new MemoryStream(bytesArr);
                    img = Image.FromStream(memstr);
                }
            }
            return img;
        }

        public byte[] ImageToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        #endregion Utility
    }
}
