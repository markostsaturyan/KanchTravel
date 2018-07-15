CREATE PROCEDURE [dbo].[InsertUser]
	@firstName nvarchar(Max),
	@lastName nvarchar(Max),
	@gender bit,
	@dateOfBirth Date,
	@phonNumber  nvarchar(Max),
	@email nvarchar(Max),
	@picture varbinary(MAX),
	@userName nvarchar(Max),
	@password nvarchar(Max),
	@isActive bit,
	@userGude nvarchar(Max),
	@isApproved bit
AS
	Insert into Users (FirstName, LastName, Gender, DateOfBirth, PhoneNumber, Email, UserName, [Password], IsActive, UserGuid,IsApproved)
	Values(@FirstName, @LastName, @Gender, @DateOfBirth, @PhonNumber, @Email,@UserName,@Password, 1,@UserGude,@isApproved)

	Insert into UserRole Values((Select SCOPE_IDENTITY() from Users),(Select Id from Roles Where RoleName='User'));

	select Id
	from Users
	where @UserName=UserName

RETURN 0
