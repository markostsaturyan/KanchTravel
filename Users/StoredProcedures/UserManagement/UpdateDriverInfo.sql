CREATE PROCEDURE [dbo].[UpdateDriverInfo]
	@userId int,
	@drivingLicencePicFront varbinary(Max),
	@drivingLicencePicBack varbinary(Max),
	@knowledgeOfLanguages nvarchar(Max)
AS

UPDATE Drivers
SET DrivingLicencePicFront=@drivingLicencePicFront,DrivingLicencePicBack=@drivingLicencePicBack,KnowledgeOfLanguages=@knowledgeOfLanguages
WHERE UserId=@userId;

RETURN 0
