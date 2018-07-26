CREATE PROCEDURE [dbo].[InsertServiceRequestResponse]
	@userId int,
	@campingTripId nvarchar(Max),
	@price float
AS

Insert into ServicesRequestsResponse(UserId,CampingTripId,Price)
Values(@userId,@campingTripId,@price)

RETURN 0
