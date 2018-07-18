CREATE PROCEDURE [dbo].[InsertGuidePlaces]
	@id int,
	@place int
AS

Insert into GuidePlaces(UserId,Places)
Values(@id,@place)

RETURN 0
