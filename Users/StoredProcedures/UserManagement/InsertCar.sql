CREATE PROCEDURE [dbo].[InsertCar]
	@brand nvarchar(Max),
	@numberOfSeats int,
	@fuelType nvarchar(Max),
	@carPicture1 varbinary(Max),
	@carPicture2 varbinary(Max),
	@carPicture3 varbinary(Max),
	@licensePlate varbinary(Max),
	@hasWiFi bit,
	@hasMicrophone bit,
	@hasAirConditioner bit,
	@hasKitchen bit,
	@hasToilet bit
AS
	INSERT INTO Car(Brand,NumberOfSeats,FuelType,CarPicture1,CarPicture2,CarPicture3,LicensePlate,HasWiFi,HasMicrophone,HasAirConditioner,HasKitchen,HasToilet)
	output SCOPE_IDENTITY()
	Values(@brand, @numberOfSeats, @fuelType, @carPicture1, @carPicture2, @carPicture3, @licensePlate, @hasWiFi, @hasMicrophone, @hasAirConditioner, @hasKitchen, @hasToilet)
RETURN 0
