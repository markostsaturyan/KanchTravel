CREATE TABLE [dbo].[RequestsOfDrivers]
(
	[DriverId] INT NOT NULL, 
    [CampingTripId] NVARCHAR(MAX) NOT NULL
	FOREIGN KEY (DriverId) REFERENCES Users(Id),
	FOREIGN KEY (DriverId) REFERENCES Drivers(UserId)
)
