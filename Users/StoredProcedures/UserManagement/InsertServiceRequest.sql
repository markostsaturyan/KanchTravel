CREATE PROCEDURE [dbo].[InsertServiceRequest]
	@userId int,
	@campingTripId nvarchar(Max)
AS

Insert into RequestsForServices(UserId,CamipingTripId)
Values(@userId,@campingTripId)

RETURN 0
