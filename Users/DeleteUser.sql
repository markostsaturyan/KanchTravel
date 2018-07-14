CREATE PROCEDURE [dbo].[DeleteUser]
	@id int
AS

Delete from Users
Where @id = Id

Delete from UserRole
Where @id = UserId

RETURN 0
