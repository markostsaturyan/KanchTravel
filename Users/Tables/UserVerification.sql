CREATE TABLE [dbo].[UserVerification]
(
	[UserId] INT NOT NULL PRIMARY KEY, 
    [Code] INT NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id)
)
