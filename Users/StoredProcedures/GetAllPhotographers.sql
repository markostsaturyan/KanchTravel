CREATE PROCEDURE [dbo].[GetAllPhotographers]
AS
	SELECT UserId as Id, FirstName, LastName, Gender, DateOfBirth, 
	Email, PhoneNumber, Picture, UserName, KnowledgeOfLanguages, Rating, NumberOfAppraisers, WorkExperience, Profession, 
	HasDron, HasCameraStabilizator, HasGopro, Camera.Id as CameraId, IsProfessional, Camera.Model as Model
	FROM Users JOIN Photographer ON Users.Id = Photographer.UserId
	Join Camera On Photographer.CameraId = Camera.Id
RETURN 0
