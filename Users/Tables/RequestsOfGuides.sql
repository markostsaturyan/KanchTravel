CREATE TABLE [dbo].[RequestsOfGuides]
(
	[GuideId] INT NOT NULL, 
    [CampingTripId] NVARCHAR(MAX) NOT NULL
	FOREIGN KEY (GuideId) REFERENCES Users(Id),
	FOREIGN KEY (GuideId) REFERENCES Guide(UserId)
)
