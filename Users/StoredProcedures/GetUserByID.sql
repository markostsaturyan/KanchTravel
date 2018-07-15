CREATE PROCEDURE [dbo].[GetUserById]
	@userId int
AS
	SELECT Users.Id, UserName, [Password],Roles.RoleName, IsActive, UserGuid
	FROM Users join UserRole on Users.Id=UserId
		 join Roles on RoleId=Roles.Id
	WHERE Users.Id=@userID

RETURN 0
