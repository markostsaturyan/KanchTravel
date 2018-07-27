CREATE PROCEDURE [dbo].[GetByUserId]
	@id int
AS

	SELECT Users.Id, UserName, [Password], Roles.RoleName, IsActive,UserGuid
	FROM Users join UserRole on Users.Id=UserId
		 join Roles on RoleId=Roles.Id
	WHERE Users.Id=@id

RETURN 0
