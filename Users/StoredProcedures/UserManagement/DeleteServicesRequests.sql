CREATE PROCEDURE [dbo].[DeleteServicesRequests]
	@id int
AS

Delete from RequestsForServices
Where Id = @id

RETURN 0
