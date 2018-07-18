CREATE PROCEDURE [dbo].[UpdateGuidePlaces]
	@id int = 0,
	@place int
AS

if @place not in (select Places
			  from GuidePlaces
			  where UserId=@id)
insert into GuidePlaces
values (@id,@place)

RETURN 0
