CREATE PROCEDURE [dbo].[InsertUser]
	@firstName nvarchar(Max),
	@lastName nvarchar(Max),
	@gender nvarchar(6),
	@dateOfBirth DateTime,
	@phoneNumber  nvarchar(Max),
	@email nvarchar(Max),
	@picture varbinary(MAX) = NULL,
	@userName nvarchar(Max),
	@password nvarchar(Max),
	@userGuid nvarchar(Max),
	@userRole nvarchar(Max)
AS

	Insert into Users (FirstName, LastName, Gender, DateOfBirth, PhoneNumber, Email, Picture ,UserName, [Password], IsActive, UserGuid, IsApproved)
	Values(@firstName, @lastName, @gender, @dateOfBirth, @phoneNumber, @email, @picture, @userName, @password, 1, @userGuid, 0)

	INSERT INTO UserRole Values((Select Id from Users Where UserName=@userName),(Select Id from Roles Where RoleName=@userRole));

	Select Id from Users Where UserName=@userName
	
RETURN 0
