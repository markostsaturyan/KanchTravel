CREATE PROCEDURE [dbo].[GetUserRegisteredCampingTrips]
	@userId int
AS
	Select CampingTripID
	From [CampingTrip'sMembers]
	Where UserID=@userId
RETURN 0
