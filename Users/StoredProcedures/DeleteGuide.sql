CREATE PROCEDURE [dbo].[DeleteGuide]
	@id int
AS
Delete from Users
Where @id = Id

Delete from UserRole
Where @id = UserId

Delete from Guide
where @id=UserId

Delete from GuidePlaces
Where @id=UserId

RETURN 0
