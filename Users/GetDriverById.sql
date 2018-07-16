CREATE PROCEDURE [dbo].[GetDriverById]
@userId int
AS
	SELECT UserId, FirstName, LastName, DateOfBirth, Email, PhoneNumber, Picture, UserName, [Password], IsActive, 
	DrivingLicencePicBack, DrivingLicencePicFront, KnowledgeOfLanguages, Rating, 
	Car.Id as CarId, Car.Brand as Brand, CarPicture1, CarPicture2, CarPicture3, FuelType, Car.HasAirConditioner, Car.HasKitchen, Car.HasMicrophone, Car.HasToilet
	FROM Users JOIN Drivers ON Users.Id = Drivers.UserId
	Join Car On Drivers.CarId=Car.Id
	WHERE Users.Id=@userId

RETURN 0