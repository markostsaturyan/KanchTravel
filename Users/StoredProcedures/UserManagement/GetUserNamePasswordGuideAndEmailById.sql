CREATE PROCEDURE [dbo].[GetUserNamePasswordGuideAndEmailById]
	@id int
AS
	SELECT UserName, [Password], UserGuid, Email
	FROM Users
RETURN 0
