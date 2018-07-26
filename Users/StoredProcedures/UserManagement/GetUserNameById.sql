CREATE PROCEDURE [dbo].[GetUserNameById]
	@id int
AS
	SELECT UserName
	FROM Users
	WHERE Id=@id
RETURN 0
