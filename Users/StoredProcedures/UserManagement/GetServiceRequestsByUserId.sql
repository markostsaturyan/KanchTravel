CREATE PROCEDURE [dbo].[GetServiceRequestsByUserId]
	@userId int
AS

Select CamipingTripId
From RequestsForServices
Where UserId=@userId

RETURN 0
