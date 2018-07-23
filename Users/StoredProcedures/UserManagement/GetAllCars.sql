CREATE PROCEDURE [dbo].[GetAllCars]
	@numberOfSeats int

AS

Select Id,UserId,Brand,NumberOfSeats,FuelType,CarPicture1,CarPicture2,CarPicture3,HasWiFi,HasMicrophone,HasAirConditioner,HasKitchen,HasToilet,LicensePlate
From Car join Drivers on Car.Id=Drivers.CarId

RETURN 0
