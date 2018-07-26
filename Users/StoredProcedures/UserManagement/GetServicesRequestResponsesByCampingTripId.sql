CREATE PROCEDURE [dbo].[GetServicesRequestResponsesByCampingTripId]
	@campingTripId nvarchar(Max)
AS

Select Id,UserId,Price
From ServicesRequestsResponse
Where CampingTripId = @campingTripId

RETURN 0
