CREATE PROCEDURE [dbo].[DeleteUserFromCampingTrips]
	@id int
AS

delete from [CampingTrip'sMembers]
where UserID=@id

RETURN 0
