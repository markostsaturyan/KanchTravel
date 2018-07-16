CREATE PROCEDURE [dbo].[GetDriverById]
	@driverId int 
AS
	SELECT UserId as Id, FirstName, LastName, DATEDIFF(hour,DateOfBirth,GETDATE())/8766 AS Age, Email, PhoneNumber, Picture, UserName 
	DrivingLicencePicBack, DrivingLicencePicFront, KnowledgeOfLanguages, Rating, 
	Car.Id as CarId, Car.Brand as Brand, CarPicture1, CarPicture2, CarPicture3, FuelType, Car.HasAirConditioner, Car.HasKitchen, Car.HasMicrophone, Car.HasToilet
	FROM Users JOIN Drivers ON Users.Id = Drivers.UserId
	Join Car On Drivers.CarId = Car.Id
	Where UserId = @driverId
RETURN 0
