CREATE PROCEDURE [dbo].[GetUserEmailById]
	@id int
AS
	SELECT Email
	FROM Users
	WHERE @id=Id
RETURN 0
