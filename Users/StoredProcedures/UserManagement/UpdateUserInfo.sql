CREATE PROCEDURE [dbo].[UpdateUserInfo]
	@id INT, 
    @firstName NVARCHAR(MAX), 
    @lastName NVARCHAR(MAX), 
	@gender NVARCHAR(6),
    @dateOfBirth DATE, 
    @phoneNumber NVARCHAR(MAX), 
	@picture VARBINARY(MAX)
AS

UPDATE Users
SET FirstName = @firstName, LastName=@lastName, Gender=@gender, DateOfBirth=@dateOfBirth, PhoneNumber=@phoneNumber, Picture=@picture
WHERE Id=@id;

RETURN 0
