CREATE PROCEDURE [dbo].[SignUpForTheCamping]
	@CampingTripID NVARCHAR(MAX),
	@UserID int
AS
	Insert Into [CampingTripsMembers] Values(@CampingTripID,@UserID)
RETURN 0
