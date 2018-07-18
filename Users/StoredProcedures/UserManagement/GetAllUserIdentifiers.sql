CREATE PROCEDURE [dbo].[GetAllUserIdentifiers]
AS
	SELECT Users.Id, FirstName, LastName, UserName, [Password], Email, Roles.RoleName, IsActive
	FROM Users join UserRole on Users.Id=UserId
		 join Roles on RoleId=Roles.Id
RETURN 0
