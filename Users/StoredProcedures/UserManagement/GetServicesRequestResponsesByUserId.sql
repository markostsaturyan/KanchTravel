CREATE PROCEDURE [dbo].[GetServicesRequestResponsesByUserId]
	@userId int
AS

Select Id, CampingTripId, Price
From ServicesRequestsResponse
Where UserId=@userId

RETURN 0
