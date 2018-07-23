CREATE PROCEDURE [dbo].[DeleteServiceRequestResponse]
	@id int
AS

Delete ServicesRequestsResponse
Where Id=@id

RETURN 0
