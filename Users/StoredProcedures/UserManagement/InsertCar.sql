CREATE PROCEDURE [dbo].[InsertCar]
	@brand nvarchar(Max),
	@numberOfSeats int,
	@fuelType nvarchar(Max),
	@carPicture1 varbinary(Max) = NULL,
	@carPicture2 varbinary(Max) = NULL,
	@carPicture3 varbinary(Max) = NULL,
	@licensePlate nvarchar(7),
	@hasWiFi bit,
	@hasMicrophone bit,
	@hasAirConditioner bit,
	@hasKitchen bit,
	@hasToilet bit
AS
	INSERT INTO Car(Brand,NumberOfSeats,FuelType,CarPicture1,CarPicture2,CarPicture3,LicensePlate,HasWiFi,HasMicrophone,HasAirConditioner,HasKitchen,HasToilet)
	Values(@brand, @numberOfSeats, @fuelType, @carPicture1, @carPicture2, @carPicture3, @licensePlate, @hasWiFi, @hasMicrophone, @hasAirConditioner, @hasKitchen, @hasToilet)

	Select Id FROM Car Where LicensePlate=@licensePlate
RETURN 0
