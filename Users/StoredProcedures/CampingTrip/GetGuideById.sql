CREATE PROCEDURE [dbo].[GetGuideById]
	@guideId int
AS
	SELECT UserId as Id, FirstName, LastName, DATEDIFF(hour,DateOfBirth,GETDATE())/8766 AS Age, Email, Gender, PhoneNumber, Picture, UserName 
	KnowledgeOfLanguages, Rating, EducationGrade, WorkExperience, Profession
	FROM Users JOIN Guide ON Users.Id = Guide.UserId
	Where UserId = @guideId

	Select Places
	From GuidePlaces
	Where UserId=@guideId
RETURN 0
