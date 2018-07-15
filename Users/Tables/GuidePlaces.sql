CREATE TABLE [dbo].[GuidePlaces]
(
	[UserId] INT NOT NULL,
	[Places] NVARCHAR(MAX) NOT NULL, 
    FOREIGN KEY(UserId) REFERENCES Users(Id)
)
