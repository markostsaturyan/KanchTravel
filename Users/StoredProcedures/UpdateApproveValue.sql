CREATE PROCEDURE [dbo].[UpdateApproveValue]
	@id int,
	@approveNewValue int
AS

UPDATE Users
SET IsApproved=@approveNewValue
WHERE Id=@id;

RETURN 0
