CREATE PROCEDURE [dbo].[InsertUserVerificationCode]
	@userName nvarchar(Max),
	@code int
AS

Insert into UserVerification(UserName,Code)
Values(@userName,@code)
	
RETURN 0
