CREATE PROCEDURE [dbo].[DeleteDriver]
	@id int
AS

Delete from Users
Where Id = @id

Delete from Car
Where Id = (SELECT CarId FROM Drivers WHERE UserId=@id)

DELETE FROM Drivers
WHERE UserId=@id

RETURN 0