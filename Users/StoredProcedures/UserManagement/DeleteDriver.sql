CREATE PROCEDURE [dbo].[DeleteDriver]
	@id int
AS

Delete from Car
Where Id = (select CarId from Drivers where Id = @id)

Delete from Users
Where Id = @id

Delete from UserRole
Where UserId = @id

Delete from Drivers
Where UserId=@id

RETURN 0
