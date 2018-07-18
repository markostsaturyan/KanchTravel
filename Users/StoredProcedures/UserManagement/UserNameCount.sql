CREATE PROCEDURE [dbo].[UserNameCount]
	@userName varchar(Max)
AS

	SELECT COUNT(UserName)
	from Users
	where UserName=@userName

RETURN 0
