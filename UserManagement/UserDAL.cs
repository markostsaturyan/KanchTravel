using System;
using System.Data.SqlClient;
using UserManagement.DAO;

namespace UserManagement
{
    public static class UserDAL
    {
        /// <summary>
        /// Connection string
        /// </summary>
        private static readonly string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog = UsersDB;Integrated Security=True;Pooling=False;Connect Timeout=30";

        /// <summary>
        /// Adds user to database
        /// </summary>
        /// <param name="user"> The user full info </param>
        /// <returns> User's id after inserting </returns>
        public static int AddUser(UserFull user)    
        {
            var userGuid = Guid.NewGuid();

            string hashedPassword = SecurityForPassword.HashSHA1(user.Password + userGuid.ToString());

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
                command.Parameters.AddWithValue("@isActive", 1);

                // TODO sendEmail method isn't correct
                SendVerificationLinkEmail.SendEmail();

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

            string hashedPassword = SecurityForPassword.HashSHA1(guide.Password + userGuid.ToString());

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
                    CommandText = "INSERT INTO UsersDB.dbo.Guide VAULES(@userId, @educationGrade, @profession, @knowledgeOfLanguages, @workExperience, @rating, @approved)"
                };

                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@educationGrade", guide.EducationGrade);
                command.Parameters.AddWithValue("@profession", guide.Profession);
                command.Parameters.AddWithValue("@knowledgeOfLanguages", guide.KnowledgeOfLanguages);
                command.Parameters.AddWithValue("@workExperience", guide.WorkExperience);
                command.Parameters.AddWithValue("@rating", guide.Raiting);
                command.Parameters.AddWithValue("@approved", 0);

                command.ExecuteNonQuery();


                // TODO sendEmail method isn't correct
                SendVerificationLinkEmail.SendEmail();
            }
        }

        /// <summary>
        /// Adds Driver into database
        /// </summary>
        /// <param name="driver"> Driver full info </param>
        public static void AddDriver(DriverFull driver)
        {
            var userGuid = Guid.NewGuid();

            string hashedPassword = SecurityForPassword.HashSHA1(driver.Password + userGuid.ToString());

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
        /// Adds Photodrapher into database
        /// </summary>
        /// <param name="photographer"> Photographer full info </param>
        public static void AddPhotographer(PhotographerFull photographer)
        {
            var userGuid = Guid.NewGuid();

            string hashedPassword = SecurityForPassword.HashSHA1(photographer.Password + userGuid.ToString());

            var userFullInfo = new UserFull
            {
                FirstName = photographer.FirstName,
                LastName = photographer.LastName,
                Sex = photographer.Sex,
                DataOfBirth = photographer.DataOfBirth,
                PhoneNumber = photographer.PhoneNumber,
                Email = photographer.Email,
                Image = photographer.Image,
                UserName = photographer.UserName,
                Password = hashedPassword,
                IsActive = true,
                UserGuid = userGuid.ToString()
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
        public static int AddCamera(Camera camera)
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
    }
}
