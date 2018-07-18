CREATE PROCEDURE [dbo].[DeleteMemberFromTheTrip]
	@id INT,
	@campingTripId NVARCHAR(MAX)
AS
	DELETE FROM [CampingTrip'sMembers]
	WHERE CampingTripID=@campingTripId AND UserID=@id
RETURN 0
