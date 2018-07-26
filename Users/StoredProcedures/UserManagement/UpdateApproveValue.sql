CREATE PROCEDURE [dbo].[UpdateApproveValue]
	@userName int,
	@approveNewValue int
AS

UPDATE Users
SET IsApproved=@approveNewValue
WHERE UserName=@userName;

RETURN 0
