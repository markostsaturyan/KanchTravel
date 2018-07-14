CREATE PROCEDURE [dbo].[InsertUserVerificationCode]
	@id int,
	@code int
AS

Insert into UserVerification(UserId,Code)
Values(@id,@code)
	
RETURN 0
