CREATE PROCEDURE [dbo].[InsertGuideInfo]
	@userId int,
	@educationGrade nvarchar(MAX),
	@profession nvarchar(MAX),
	@knowledgeOfLanguages nvarchar(MAX),
	@workExperience nvarchar(MAX)
AS
	INSERT INTO Guide(UserId,EducationGrade,Profession,KnowledgeOfLanguages,WorkExperience)
	Values(@userId, @educationGrade, @profession, @knowledgeOfLanguages, @workExperience)
RETURN 0
