using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.DataManagnment.DataAccesLayer.Models;
using UserManagement.DataManagnment.Security;
using UserManagement.Verification;
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
        public int AddUser(UserFull user)
        {
            var userGuid = Guid.NewGuid().ToString();

            string hashedPassword = SecurityForPassword.HashSHA1(user.Password + userGuid);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertUser"
                };

                command.Parameters.AddWithValue("@firstName", user.FirstName);
                command.Parameters.AddWithValue("@lastName", user.LastName);
                command.Parameters.AddWithValue("@sex", user.Gender);
                command.Parameters.AddWithValue("@dateOfBirth", user.DataOfBirth);
                command.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@picture", user.Image);
                command.Parameters.AddWithValue("@username", user.UserName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@userguid", userGuid);
                command.Parameters.AddWithValue("@isActive", 1);
                command.Parameters.AddWithValue("@isApproved", 0);

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
                    CommandText = "InsertUserVerification"
                };

                var rand = new Random();

                var randNumber = rand.Next(1000, 9999);

                verifCommand.Parameters.AddWithValue("@userId", id);
                verifCommand.Parameters.AddWithValue("@code", randNumber);

                verifCommand.ExecuteNonQuery();

                return randNumber;
            }
        }

        /// <summary>
        /// Adds Guide to database
        /// </summary>
        /// <param name="guide"> The guide full info </param>
        public int AddGuide(GuideFull guide)
        {
            var userFullInfo = new UserFull
            {
                FirstName = guide.FirstName,
                LastName = guide.LastName,
                Gender = guide.Gender,
                DataOfBirth = guide.DataOfBirth,
                PhoneNumber = guide.PhoneNumber,
                Email = guide.Email,
                Image = guide.Image,
                UserName = guide.UserName,
                Password=guide.Password
            };

            var userId = AddUser(userFullInfo);

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType=System.Data.CommandType.StoredProcedure,
                    CommandText = "InsertGuideInfo"
                };

                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@educationGrade", guide.EducationGrade);
                command.Parameters.AddWithValue("@profession", guide.Profession);
                command.Parameters.AddWithValue("@knowledgeOfLanguages", guide.KnowledgeOfLanguages);
                command.Parameters.AddWithValue("@workExperience", guide.WorkExperience);
                command.Parameters.AddWithValue("@rating", guide.Raiting);

                connection.Open();

                command.ExecuteNonQuery();
            }

            return userId;
        }

        /// <summary>
        /// Adds Driver into database
        /// </summary>
        /// <param name="driver"> Driver full info </param>
        public void AddDriver(DriverFull driver)
        {
            var userGuid = Guid.NewGuid();

            string hashedPassword = SecurityForPassword.HashSHA1(driver.Password + userGuid.ToString());

            var userFullInfo = new UserFull
            {
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Gender = driver.Gender,
                DataOfBirth = driver.DataOfBirth,
                PhoneNumber = driver.PhoneNumber,
                Email = driver.Email,
                Image = driver.Image,
                UserName = driver.UserName,
                Password = hashedPassword,
            };

            var userId = AddUser(userFullInfo);
            var carId = AddCar(driver.Car);

            using (var connection = new SqlConnection(connectionString))
            {
                var commandForInsertDriver = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "INSERT INTO UsersDB.dbo.Drivers VAULES(@userId, @carId, @drivingLicencePicFront, @drivingLicencePicBack, @knowledgeOfLanguages, @rating, @isApproved)"
                };

                commandForInsertDriver.Parameters.AddWithValue("@userId", userId);
                commandForInsertDriver.Parameters.AddWithValue("@carId", carId);
                commandForInsertDriver.Parameters.AddWithValue("@drivingLicencePicFront", driver.DrivingLicencePicFront);
                commandForInsertDriver.Parameters.AddWithValue("@drivingLicencePicBack", driver.DrivingLicencePicBack);
                commandForInsertDriver.Parameters.AddWithValue("@knowledgeOfLanguages", driver.KnowledgeOfLanguages);
                commandForInsertDriver.Parameters.AddWithValue("@rating", driver.Raiting);
                commandForInsertDriver.Parameters.AddWithValue("@isApproved", 0);

                commandForInsertDriver.ExecuteNonQuery();
            }
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
        /// Adds Photodrapher into database
        /// </summary>
        /// <param name="photographer"> Photographer full info </param>
        public void AddPhotographer(PhotographerFull photographer)
        {
            var userFullInfo = new UserFull
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
                    CommandText = "INSERT INTO UsersDB.dbo.Photographer VAULES(@userId, @cameraId, @profession, @knowledgeOfLanguages, @workExperience, @hasDron, @hasCameraStabilizator, @hasGopro, @rating, @isApproved)"
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
                commandForInsertPhotographer.Parameters.AddWithValue("@approved", 0);

                commandForInsertPhotographer.ExecuteNonQuery();
            }
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
                    CommandText = "INSERT INTO UsersDB.dbo.Camera output INSERTED.Id VAULES(@isProfessional, @model)"
                };

                commandForInsertCamera.Parameters.AddWithValue("@isProfessional", camera.IsProfessional);
                commandForInsertCamera.Parameters.AddWithValue("@model", camera.Model);


                return (int)commandForInsertCamera.ExecuteScalar();
            }
        }
        #endregion Adding

#region Getting
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                        Image = (ImageSharpTexture)dataReader["Image"],
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
                        Image = (ImageSharpTexture)dataReader["Image"],
                        UserName = (string)dataReader["UserName"],
                        Gender = (string)dataReader["Gender"],
                        EducationGrade = (string)dataReader["EducationGrade"],
                        KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                        Profession = (string)dataReader["Profession"],
                        Raiting = (double)dataReader["Raiting"],
                        WorkExperience = (string)dataReader["WorkExperience"]
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
        public List<GuideInfo> GetAllUsers()
        {
            var users = new List<GuideInfo>();

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
                        users.Add(new GuideInfo
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            DataOfBirth = (DateTime)dataReader["DataOfBirth"],
                            Email = (string)dataReader["Email"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            Image = (ImageSharpTexture)dataReader["Image"],
                            UserName = (string)dataReader["UserName"],

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
        public List<DriverFull> GetAllDrivers()
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
                            DrivingLicencePicFront = (ImageSharpTexture)dataReader["DrivingLicencePicFront"],
                            DrivingLicencePicBack = (ImageSharpTexture)dataReader["DrivingLicencePicBack"],
                            KnowledgeOfLanguages = (string)dataReader["KnowledgeOfLanguages"],
                            Raiting = (double)dataReader["Raiting"],
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
        public List<GuideFull> GetAllGuides()
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
        private List<string> GetGuidePalces(int id)
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
        public List<PhotographerFull> GetAllPhotographers()
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
                                Model = (string)dataReader["Model"],
                                IsProfessional = (bool)dataReader["IsProfessional"]
                            },
                            Profession = (string)dataReader["Profession"],
                            WorkExperience = (string)dataReader["WorkExperiance"],
                        };

                        photographers.Add(photographer);
                    }
                }
            }

            return photographers;
        }
        #endregion Getting

#region Updating
        public void UpdateUserInfo(UserInfo user)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateUserFullInfo"
                };

                updateCommand.Parameters.AddWithValue("@id", user.Id);
                updateCommand.Parameters.AddWithValue("@firstName", user.FirstName);
                updateCommand.Parameters.AddWithValue("@lastName", user.LastName);
                updateCommand.Parameters.AddWithValue("@gender", user.Gender);
                updateCommand.Parameters.AddWithValue("@dateOfBirth", user.DataOfBirth);
                updateCommand.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                updateCommand.Parameters.AddWithValue("@email", user.Email);
                updateCommand.Parameters.AddWithValue("@picture", user.Image);
                updateCommand.Parameters.AddWithValue("@userName", user.UserName);

                connection.Open();

                updateCommand.ExecuteNonQuery();
            }
        }

        public void UpdateUserInfo(UserFull user)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                var updateCommand = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "UpdateUserFullInfo"
                };

                updateCommand.Parameters.AddWithValue("@id", user.Id);
                updateCommand.Parameters.AddWithValue("@firstName", user.FirstName);
                updateCommand.Parameters.AddWithValue("@lastName", user.LastName);
                updateCommand.Parameters.AddWithValue("@gender", user.Gender);
                updateCommand.Parameters.AddWithValue("@dateOfBirth", user.DataOfBirth);
                updateCommand.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                updateCommand.Parameters.AddWithValue("@email", user.Email);
                updateCommand.Parameters.AddWithValue("@picture", user.Image);
                updateCommand.Parameters.AddWithValue("@userName", user.UserName);
                updateCommand.Parameters.AddWithValue("@password", user.Password);

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
#endregion Updating

#region Deleting

        public void DeleteUser(int id)
        {
            using(var connection = new SqlConnection(this.connectionString))
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

        #endregion Deleting

        #region Validating

        public bool UsarNameValidating(string userName)
        {
            using(var connection = new SqlConnection(this.connectionString))
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
