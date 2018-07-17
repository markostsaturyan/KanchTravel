CREATE PROCEDURE [dbo].[InsertUser]
	@firstName nvarchar(Max),
	@lastName nvarchar(Max),
	@gender bit,
	@dateOfBirth Date,
	@phoneNumber  nvarchar(Max),
	@email nvarchar(Max),
	@picture varbinary(MAX),
	@userName nvarchar(Max),
	@password nvarchar(Max),
	@userGude nvarchar(Max)
AS
	Insert into Users (FirstName, LastName, Gender, DateOfBirth, PhoneNumber, Email, UserName, [Password], IsActive, UserGuid, IsApproved)
	Values(@firstName, @lastName, @gender, @dateOfBirth, @phoneNumber, @email, @userName, @password, 1, @userGude, 0)

	INSERT INTO UserRole Values((Select SCOPE_IDENTITY() from Users),(Select Id from Roles Where RoleName='User'));

	SELECT SCOPE_IDENTITY()
	FROM Users

RETURN 0
