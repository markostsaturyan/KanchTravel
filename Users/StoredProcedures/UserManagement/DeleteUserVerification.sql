CREATE PROCEDURE [dbo].[DeleteUserVerification]
	@userName nvarchar(Max)
AS

Delete from UserVerification
Where UserName = @userName

Delete from UserRole
Where UserId = (Select Id
				From Users
				Where UserName = @userName)

Delete from Users
Where UserName=@userName

RETURN 0
