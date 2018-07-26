CREATE TABLE [dbo].[ServicesRequestsResponse]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] INT NOT NULL, 
    [CampingTripId] NVARCHAR(MAX) NOT NULL, 
    [Price] FLOAT NOT NULL
	FOREIGN KEY (UserId) REFERENCES Users(Id)
)
