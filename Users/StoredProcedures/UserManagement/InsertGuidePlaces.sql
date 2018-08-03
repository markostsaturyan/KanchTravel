CREATE PROCEDURE [dbo].[InsertGuidePlaces]
	@id int,
	@place nvarchar(Max)
AS

Insert into GuidePlaces(UserId,Places)
Values(@id,@place)

RETURN 0
