CREATE PROCEDURE [dbo].[GetByUserName]
	 @userName nvarchar(Max)
AS
	SELECT Users.Id, UserName, [Password], Roles.RoleName, IsActive,UserGuid
	FROM Users join UserRole on Users.Id=UserId
		 join Roles on RoleId=Roles.Id
	WHERE Users.UserName=@userName
RETURN 0
