CREATE PROCEDURE [dbo].[GetFullUserInfoById]
	@userId int
AS
	SELECT Id, FirstName, LastName, Picture, Gender, DateOfBirth, PhoneNumber, Email, UserName
	FROM Users
	WHERE Users.Id=@userID

RETURN 0
