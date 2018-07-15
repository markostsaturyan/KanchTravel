CREATE PROCEDURE [dbo].[InsertUser]
	@FirstName nvarchar(Max),
	@LastName nvarchar(Max),
	@Gender bit,
	@DateOfBirth Date,
	@PhonNumber  nvarchar(Max),
	@Email nvarchar(Max),
	@UserName nvarchar(Max),
	@Password nvarchar(Max)
AS
	Insert into Users (FirstName, LastName, Gender, DateOfBirth, PhoneNumber, Email, UserName, [Password], IsActive)
	Values(@FirstName, @LastName, @Gender, @DateOfBirth, @PhonNumber, @Email,@UserName,@Password, 1)

	Insert into UserRole Values((Select SCOPE_IDENTITY()),(Select Id from Roles Where RoleName='User'));
RETURN 0
