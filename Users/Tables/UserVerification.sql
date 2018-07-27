CREATE TABLE [dbo].[UserVerification]
(
	[UserName] NVARCHAR(400) NOT NULL PRIMARY KEY, 
    [Code] INT NOT NULL,
	FOREIGN KEY ([UserName]) REFERENCES Users(UserName)
)
