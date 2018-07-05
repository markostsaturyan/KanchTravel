CREATE TABLE [dbo].[Drivers]
(
	[UserId] INT NOT NULL,
	[CarId] INT NOT NULL,
	[DrivingLicencePicFront] IMAGE NOT NULL,
	[DrivingLicencePicBack] IMAGE NOT NULL, 
    [KnowledgeOfLanguages] NVARCHAR(MAX) NOT NULL, 
    [Rating] FLOAT NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (CarID) REFERENCES Car(Id)
)
