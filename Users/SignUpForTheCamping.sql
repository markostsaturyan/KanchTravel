CREATE PROCEDURE [dbo].[SignUpForTheCamping]
	@CampingTripID NVARCHAR(MAX),
	@UserID int,
	@MinAge int=0,
	@MaxAge int=200,
	@MaxCountOfMembers int=40
AS
	if((Select (DATEDIFF(hour,Users.DateOfBirth,GETDATE())/8766) From Users Where Id=@UserID) >= @MinAge AND 
	(Select (DATEDIFF(hour,Users.DateOfBirth,GETDATE())/8766) From Users Where Id=@UserID) <= @MinAge AND 
	(Select Count(UserID) From [CampingTrip'sMembers] Where CampingTripID=@CampingTripID)<@MaxCountOfMembers)
	Insert Into [CampingTrip'sMembers] Values(@CampingTripID,@UserID)
RETURN 0
