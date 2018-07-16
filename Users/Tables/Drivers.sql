CREATE TABLE [dbo].[Drivers]
(
	[UserId] INT NOT NULL PRIMARY KEY,
	[CarId] INT NOT NULL,
	[DrivingLicencePicFront] VARBINARY(MAX) NOT NULL,
	[DrivingLicencePicBack] VARBINARY(MAX) NOT NULL, 
    [KnowledgeOfLanguages] NVARCHAR(MAX) NOT NULL, 
    [Rating] FLOAT NULL,
	[NumberOfAppraisers] INT NULL, 
    FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (CarId) REFERENCES Car(Id)
)
