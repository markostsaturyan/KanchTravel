CREATE PROCEDURE [dbo].[DeleteUser]
	@id int
AS

Delete from Users
Where Id = @id

Delete from UserRole
Where UserId = @id

RETURN 0
