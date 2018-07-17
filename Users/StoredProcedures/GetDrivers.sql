CREATE PROCEDURE [dbo].[GetDrivers]
AS
	SELECT UserId, FirstName, LastName, Gender, DateOfBirth, Email, PhoneNumber, Picture, UserName, DrivingLicencePicBack, DrivingLicencePicFront, KnowledgeOfLanguages, Rating, NumberOfAppraisers,
	Car.Id as CarId, Car.Brand as Brand, NumberOfSeats, FuelType, CarPicture1, CarPicture2, CarPicture3, LicensePlate, HasWiFi, HasAirConditioner, HasKitchen, HasMicrophone, HasToilet
	FROM Users JOIN Drivers ON Users.Id = Drivers.UserId
	Join Car On Drivers.CarId=Car.Id
RETURN 0