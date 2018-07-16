CREATE PROCEDURE [dbo].[GetPhotographerById]
	@photographerId int
AS
	SELECT UserId as Id, FirstName, LastName, DATEDIFF(hour,DateOfBirth,GETDATE())/8766 AS Age, 
	Email, PhoneNumber, Picture, UserName, KnowledgeOfLanguages, Rating, WorkExperience, Profession, 
	HasDron, HasCameraStabilizator, HasGopro, Camera.Id as CameraId, IsProfessional, Camera.Model
	FROM Users JOIN Photographer ON Users.Id = Photographer.UserId
	Join Camera On Photographer.CameraId = Camera.Id
	Where UserId = @photographerId
RETURN 0
