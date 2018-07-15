CREATE PROCEDURE [dbo].[GetFullUserInfoById]
	@userId int
AS
	SELECT *
	FROM Users join UserRole on Users.Id=UserId
		 join Roles on RoleId=Roles.Id
	WHERE Users.Id=@userID

RETURN 0
