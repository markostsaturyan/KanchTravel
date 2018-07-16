CREATE PROCEDURE [dbo].[SignUpForTheCamping]
	@CampingTripID NVARCHAR(MAX),
	@UserID int
AS
	Insert Into [CampingTrip'sMembers] Values(@CampingTripID,@UserID)
RETURN 0
