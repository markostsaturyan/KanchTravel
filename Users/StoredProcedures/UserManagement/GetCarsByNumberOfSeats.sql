CREATE PROCEDURE [dbo].[GetCarsByNumberOfSeats]
	@numberOfSeats int
AS

Select Id,UserId,Brand,NumberOfSeats,FuelType,CarPicture1,CarPicture2,CarPicture3,HasWiFi,HasMicrophone,HasAirConditioner,HasKitchen,HasToilet,LicensePlate
From Car join Drivers on Car.Id=Drivers.CarId
Where NumberOfSeats >= @numberOfSeats

RETURN 0
