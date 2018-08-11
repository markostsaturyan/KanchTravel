CREATE PROCEDURE [dbo].[GetUserPasswordAndGuideById]
	@id int
AS

	SELECT Password, UserGuid
	from Users
	where @id = Id

RETURN 0
