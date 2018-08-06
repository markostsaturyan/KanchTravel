CREATE PROCEDURE [dbo].[GetUserRegisteredCampingTrips]
	@userId int
AS
	Select CampingTripID
	From [CampingTripsMembers]
	Where UserID=@userId
RETURN 0