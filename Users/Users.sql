CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(MAX) NOT NULL, 
    [LastName] NVARCHAR(MAX) NOT NULL, 
	[Sex] BIT NOT NULL,
    [DateOfBirth] DATE NOT NULL, 
    [PhoneNumber] NVARCHAR(MAX) NOT NULL, 
    [Email] NVARCHAR(MAX) NOT NULL, 
	[Picture]  IMAGE NOT NULL,
    [UserName] NVARCHAR(MAX) NOT NULL, 
    [Password] NVARCHAR(MAX) NOT NULL, 
    [IsActive] BIT NOT NULL
)

Insert into 