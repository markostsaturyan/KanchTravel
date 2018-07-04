CREATE TABLE [dbo].[Car]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Brand] NVARCHAR(MAX) NOT NULL, 
    [NumberOfSeats] INT NOT NULL, 
    [FuelType] NVARCHAR(MAX) NOT NULL, 
    [CarPicture1] IMAGE NOT NULL, 
    [CarPicture2] IMAGE NULL, 
    [CarPicture3] IMAGE NULL, 
    [LicensePlate] NVARCHAR(MAX) NOT NULL, 
    [HasWiFi] BIT NOT NULL, 
    [HasMicrophone] BIT NOT NULL, 
    [HasAirConditioner] BIT NOT NULL, 
    [HasKitchen] BIT NOT NULL, 
    [HasToilet] BIT NOT NULL
)
