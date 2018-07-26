CREATE PROCEDURE [dbo].[GetVerificationCode]
	@userName nvarchar(400)
AS

Select Code
From UserVerification
Where UserName=@userName

RETURN 0
