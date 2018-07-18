CREATE PROCEDURE [dbo].[GetAllUsers]
	@userId INT
AS
	SELECT Id, FirstName, LastName, DateOfBirth, Gender, PhoneNumber, UserName, Email, Picture
	From Users
	Where Id=@userId
RETURN 0
