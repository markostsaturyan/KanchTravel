using System.Collections.Generic;
using System.Data.SqlClient;
using CampingTripService.DataManagement.Model.Users;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CampingTripService.DataManagement.Model.UsersDAL
{
    public class UsersDal
    {
        public string ConnectionString { get; set; }


        public User GetUser(int id)
        {
            var user = new User();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUser"
                };

                command.Parameters.AddWithValue("@userId", id);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        user.Id = (int)dataReader["Id"];
                        user.FirstName = (string)dataReader["FirstName"];
                        user.LastName = (string)dataReader["LastName"];
                        user.Age = (int)dataReader["Age"];
                        user.Gender = (string)dataReader["Gender"];
                        user.PhoneNumber = (string)dataReader["PhoneNumber"];
                        user.UserName = (string)dataReader["UserName"];
                        user.Email = (string)dataReader["Email"];
                        user.Img = ByteArrayToImage(dataReader["Picture"]);
                    }
                }
            }
            return user;
        }

        public List<User> GetMembersOfTheCampingTrip(string campingTripId)
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetMembersOfTheCampingTrip"
                };

                command.Parameters.AddWithValue("@campingTripId", campingTripId);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        users.Add(new User()
                        {
                            Id = (int)dataReader["Id"],
                            FirstName = (string)dataReader["FirstName"],
                            LastName = (string)dataReader["LastName"],
                            Age = (int)dataReader["Age"],
                            Gender = (string)dataReader["Gender"],
                            PhoneNumber = (string)dataReader["PhoneNumber"],
                            UserName = (string)dataReader["UserName"],
                            Email = (string)dataReader["Email"],
                            Img = ByteArrayToImage(dataReader["Picture"])
                        });
                    }
                }
            }
            return users;
        }
        public Driver GetDriver(int id)
        {

            var driver = new Driver();

            using (var connection = new SqlConnection(ConnectionString))
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
                    if (dataReader.Read())
                    {
                        driver.Id = (int)dataReader["Id"];
                        driver.FirstName = (string)dataReader["FirstName"];
                        driver.LastName = (string)dataReader["LastName"];
                        driver.Age = (int)dataReader["Age"];
                        driver.Email = (string)dataReader["Email"];
                        driver.Gender = (string)dataReader["Gender"];
                        driver.KnowledgeOfLanguages = (string)dataReader["KnowlegeOfLanguages"];
                        driver.Car = new Car()
                        {
                            Id = (int)dataReader["carId"],
                            Brand = (string)dataReader["Brand"],
                            NumberOfSeats = (int)dataReader["NumberOfSears"],
                            FuelType = (string)dataReader["FuelType"],
                            LicensePlate = (string)dataReader["LicensePlate"],
                            HasWiFi = (bool)dataReader["HasWiFi"],
                            HasMicrophone = (bool)dataReader["HasMicrophone"],
                            HasAirConditioner = (bool)dataReader["HasAirConditioner"],
                            HasKitchen = (bool)dataReader["HasKitchen"],
                            HasToilet = (bool)dataReader["HasToilet"],
                            CarPicture1 = ByteArrayToImage(dataReader["CarPicture1"]),
                            CarPicture2 = ByteArrayToImage(dataReader["CarPicture2"]),
                            CarPicture3 = ByteArrayToImage(dataReader["CarPicture3"])
                        };
                        driver.DrivingLicencePicBack = ByteArrayToImage(dataReader["DrivingLicencePicBack"]);
                        driver.DrivingLicencePicFront = ByteArrayToImage(dataReader["DrivingLicencePicFront"]);
                        driver.Rating = (double)dataReader["Raiting"];
                        driver.Img = ByteArrayToImage(dataReader["Picture"]);
                        driver.PhoneNumber = (string)dataReader["PhoneNumber"];
                        driver.UserName = (string)dataReader["UserName"];
                    }
                }

                return driver;

            }
        }
        public Guide GetGuide(int id)
        {

            var guide = new Guide();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetGuideById"
                };

                command.Parameters.AddWithValue("@guideId", id);

                connection.Open();
                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        guide.Id = (int)dataReader["Id"];
                        guide.FirstName = (string)dataReader["FirstName"];
                        guide.LastName = (string)dataReader["LastName"];
                        guide.Age = (int)dataReader["Age"];
                        guide.Email = (string)dataReader["Email"];
                        guide.Gender = (string)dataReader["Gender"];
                        guide.KnowledgeOfLanguages = (string)dataReader["KnowlegeOfLanguages"];
                        guide.EducationGrade = (string)dataReader["EducationGrade"];
                        guide.PhoneNumber = (string)dataReader["PhoneNumber"];
                        guide.Img = ByteArrayToImage(dataReader["Picture"]);
                        guide.UserName = (string)dataReader["UserName"];
                        guide.WorkExperience = (string)dataReader["WorkExperience"];
                        guide.Rating = (double)dataReader["Raiting"];
                        guide.Profession = (string)dataReader["Profession"];
                        if (dataReader.NextResult())
                        {
                            guide.Places = new List<string>();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    guide.Places.Add((string)dataReader["Places"]);
                                }
                            }
                        }
                    }
                }

                return guide;
            }
        }
        public List<string> GetUserRegisteredCampingTripsId(int userId)
        {
            var campingTrips = new List<string>();
            
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetUserRegisteredCampingTrips"
                };

                command.Parameters.AddWithValue("@userId", userId);

                connection.Open();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        campingTrips.Add((string)dataReader["CampingTripID"]);
                    }
                }
            }

            return campingTrips;

        }

        public Photographer GetPhotographer(int id)
        {

            var photographer = new Photographer();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "GetPhotographerById"
                };

                command.Parameters.AddWithValue("@photographerId", id);

                connection.Open();
                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        photographer.Id = (int)dataReader["Id"];
                        photographer.FirstName = (string)dataReader["FirstName"];
                        photographer.LastName = (string)dataReader["LastName"];
                        photographer.Age = (int)dataReader["Age"];
                        photographer.Email = (string)dataReader["Email"];
                        photographer.Gender = (string)dataReader["Gender"];
                        photographer.KnowledgeOfLanguages = (string)dataReader["KnowlegeOfLanguages"];
                        photographer.PhoneNumber = (string)dataReader["PhoneNumber"];
                        photographer.Img = ByteArrayToImage(dataReader["Picture"]);
                        photographer.UserName = (string)dataReader["UserName"];
                        photographer.WorkExperience = (string)dataReader["WorkExperience"];
                        photographer.Rating = (double)dataReader["Raiting"];
                        photographer.Profession = (string)dataReader["Profession"];
                        photographer.HasDron = (bool)dataReader["HasDron"];
                        photographer.HasCameraStabilizator = (bool)dataReader["HasCameraStabilizator"];
                        photographer.HasGopro = (bool)dataReader["HasGopro"];
                        photographer.Camera = new Camera()
                        {
                            Id = (int)dataReader["CameraId"],
                            IsProfessional = (bool)dataReader["IsProfessional"],
                            Model = (string)dataReader["Model"]
                        };
                    }
                }

                return photographer;
            }
        }

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

        public void SignUpForTheCamping(int id, string campingTripID)
        {

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "SignUpForTheTrip"
                };

                command.Parameters.AddWithValue("@CampingTripID", campingTripID);
                command.Parameters.AddWithValue("@UserID", id);

                connection.Open();

                var dataReader = command.ExecuteReader();
            }
        }

        public void RemoveMemberFromTheTrip(int id, string campingTripID)
        {

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "DeleteMemberFromTheTrip"
                };

                command.Parameters.AddWithValue("@campingTripId", campingTripID);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                var dataReader = command.ExecuteReader();
            }
        }
    }
}