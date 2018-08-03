CREATE PROCEDURE [dbo].[UpdateApproveValue]
	@userName nvarchar(400),
	@approveNewValue bit
AS

UPDATE Users
SET IsApproved=@approveNewValue
WHERE UserName=@userName;

RETURN 0
