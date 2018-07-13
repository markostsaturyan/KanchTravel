CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(MAX) NOT NULL, 
    [LastName] NVARCHAR(MAX) NOT NULL, 
	[Gender] NVARCHAR(6) NOT NULL  CHECK (Gender IN ('Male', 'Female')),
    [DateOfBirth] DATE NOT NULL, 
    [PhoneNumber] NVARCHAR(MAX) NOT NULL, 
    [Email] NVARCHAR(MAX) NOT NULL, 
	[Picture]  VARBINARY(MAX) NULL,
    [UserName] NVARCHAR(MAX) NOT NULL, 
    [Password] NVARCHAR(MAX) NOT NULL, 
    [IsActive] BIT NOT NULL, 
    [UserGuid] NVARCHAR(MAX) NOT NULL
)
