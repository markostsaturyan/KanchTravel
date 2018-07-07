CREATE PROCEDURE [dbo].[GetByUserName]
	 @userName nvarchar(Max)
AS
	SELECT Users.Id, FirstName, LastName, UserName, Password, Email, Roles.RoleName, IsActive
	FROM Users join UserRole on Users.Id=UserId
		 join Roles on RoleId=Roles.Id
	WHERE Users.UserName=@userName
RETURN 0
