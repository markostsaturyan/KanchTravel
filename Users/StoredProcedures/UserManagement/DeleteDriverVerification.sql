CREATE PROCEDURE [dbo].[DeleteDriverVerification]
	@userName nvarchar(Max)
AS

Delete from UserVerification
Where UserName = @userName

Delete from Car
Where Id = (select CarId from Drivers where UserId = (Select Id from Users Where UserName = @userName ))

Delete from UserRole
Where UserId = (Select Id
				From Users
				Where UserName = @userName)

Delete from Drivers
Where UserId = (Select Id
				From Users
				Where UserName = @userName)

Delete from Users
Where UserName=@userName

RETURN 0
