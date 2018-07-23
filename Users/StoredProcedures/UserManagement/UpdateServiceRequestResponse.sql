CREATE PROCEDURE [dbo].[UpdateServiceRequestResponse]
	@id int,
	@price float

AS

Update ServicesRequestsResponse
Set Price=@price
Where Id=@id

RETURN 0
