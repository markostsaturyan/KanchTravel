CREATE PROCEDURE [dbo].[UpdateUserFullInfo]
	@id INT, 
    @firstName NVARCHAR(MAX), 
    @lastName NVARCHAR(MAX), 
	@gender NVARCHAR(6),
    @dateOfBirth DATE, 
    @phoneNumber NVARCHAR(MAX), 
    @email NVARCHAR(MAX), 
	@picture VARBINARY(MAX),
    @userName NVARCHAR(MAX), 
    @password NVARCHAR(MAX)
AS

UPDATE Users
SET FirstName = @firstName, LastName=@lastName, Gender=@gender, DateOfBirth=@dateOfBirth, PhoneNumber=@phoneNumber,Email=@email, Picture=@picture, UserName=@userName, [Password]=@password
WHERE Id=@id;

RETURN 0
