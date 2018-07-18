CREATE PROCEDURE [dbo].[DeletePhotographer]
	@id int
AS

Delete from Users
Where Id = @id

Delete from UserRole
Where UserId = @id

Delete from Camera
Where Id = (SELECT CameraId FROM Photographer WHERE UserId=@id)

DELETE FROM Photographer
WHERE UserId=@id

RETURN 0