CREATE PROCEDURE [dbo].[UpdateGuideInfo]
	@id int,
	@firstName nvarchar(Max),
	@lastName nvarchar(Max),
	@gender nvarchar(6),
	@dateOfBirth date,
	@phoneNumber nvarchar(Max),
	@picture varbinary(Max),
	@workExperience nvarchar(Max),
	@profession nvarchar(Max),
	@knowledgeOfLanguages nvarchar(Max),
	@educationGrade nvarchar(Max)
AS
	
UPDATE Users
SET FirstName = @firstName, LastName=@lastName, Gender=@gender, DateOfBirth=@dateOfBirth, PhoneNumber=@phoneNumber, Picture=@picture
WHERE Id=@id;

UPDATE Guide
SET WorkExperience=@workExperience,Profession=@profession,KnowledgeOfLanguages=@knowledgeOfLanguages,EducationGrade=@educationGrade
WHERE UserId=@id;

RETURN 0
