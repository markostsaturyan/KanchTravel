CREATE PROCEDURE [dbo].[UpdateCarInfo]
	@carId int,
	@brand nvarchar(Max),
	@fuelType nvarchar(Max),
	@numberOdSeats int,
	@licensePlate varbinary(Max),
	@carPicture1 varbinary(Max),
	@carPicture2 varbinary(Max),
	@carPicture3 varbinary(Max),
	@hasAirConditioner bit,
	@hasWiFi bit,
	@hasToilet bit,
	@hasMicrophone bit,
	@hasKitchen bit
AS

UPDATE Car
SET Brand=@brand,FuelType=@fuelType,NumberOfSeats=@numberOdSeats,LicensePlate=@licensePlate,CarPicture1=@carPicture1,CarPicture2=@carPicture2,CarPicture3=@carPicture3,HasAirConditioner=@hasAirConditioner,HasWiFi=@hasWiFi,HasKitchen=@hasKitchen,HasMicrophone=@hasMicrophone,HasToilet=@hasToilet
WHERE Id=@carId;

RETURN 0
