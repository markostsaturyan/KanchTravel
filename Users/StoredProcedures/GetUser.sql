CREATE PROCEDURE [dbo].[GetUser]
	@userId INT
AS
	SELECT Id,FirstName,LastName,DATEDIFF(hour,DateOfBirth,GETDATE())/8766 AS Age, 
	Gender, PhoneNumber, UserName, Email, Picture
	From Users
	Where Id=@userId
RETURN 0
