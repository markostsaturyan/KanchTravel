CREATE TABLE [dbo].[RequestsOfPhotographers]
(
	[PhotographerId] INT NOT NULL, 
    [CampingTripId] NVARCHAR(MAX) NOT NULL
	FOREIGN KEY (PhotographerId) REFERENCES Users(Id),
	FOREIGN KEY (PhotographerId) REFERENCES Photographer(UserId)
)
