CREATE PROCEDURE [dbo].[GetUserPasswordAndGuideById]
	@id int
AS

	SELECT Id, UserGuid
	from Users
	where @id = Id

RETURN 0
