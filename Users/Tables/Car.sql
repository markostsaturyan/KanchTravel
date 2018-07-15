CREATE TABLE [dbo].[Car]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Brand] NVARCHAR(MAX) NOT NULL, 
    [NumberOfSeats] INT NOT NULL, 
    [FuelType] NVARCHAR(MAX) NOT NULL, 
    [CarPicture1] VARBINARY(MAX) NOT NULL, 
    [CarPicture2] VARBINARY(MAX) NULL, 
    [CarPicture3] VARBINARY(MAX) NULL, 
    [LicensePlate] NVARCHAR(MAX) NOT NULL, 
    [HasWiFi] BIT NOT NULL, 
    [HasMicrophone] BIT NOT NULL, 
    [HasAirConditioner] BIT NOT NULL, 
    [HasKitchen] BIT NOT NULL, 
    [HasToilet] BIT NOT NULL
)
