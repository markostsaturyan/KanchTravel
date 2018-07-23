CREATE TABLE [dbo].[UserVerification]
(
	[UserName] NVARCHAR(MAX) NOT NULL PRIMARY KEY, 
    [Code] INT NOT NULL,
	FOREIGN KEY ([UserName]) REFERENCES Users(UserName)
)
