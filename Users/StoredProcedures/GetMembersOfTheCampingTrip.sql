CREATE PROCEDURE [dbo].[GetMembersOfTheCampingTrip]
	@campingTripId NVARCHAR(MAX)
AS
	Select Users.Id,FirstName,LastName,DATEDIFF(hour,DateOfBirth,GETDATE())/8766 AS Age, 
	Gender, PhoneNumber, UserName, Email, Picture
	From [CampingTrip'sMembers] JOIN Users on UserID = Users.Id
	WHERE CampingTripID = @campingTripId
RETURN 0
