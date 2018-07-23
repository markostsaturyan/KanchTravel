CREATE PROCEDURE [dbo].[GetNonApprovedGuideById]
	@id int
AS

Select Id,FirstName,LastName,DateOfBirth,Email,PhoneNumber,
	   Picture, UserName, [Password], Gender, KnowledgeOfLanguages,
	   EducationGrade,Profession,WorkExperience

from Users join Guide on Users.Id=Guide.UserId
where IsApproved=0 and Id = @id and UserName not in (select UserName
													from UserVerification)

RETURN 0
