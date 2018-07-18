CREATE PROCEDURE [dbo].[GetGuidePlaces]
	@id int
AS

	SELECT Places
	from GuidePlaces
	where UserId=@id

RETURN 0
