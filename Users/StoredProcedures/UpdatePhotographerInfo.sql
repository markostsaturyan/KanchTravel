CREATE PROCEDURE [dbo].[UpdatePhotographerInfo]
	@userId INT,
    @profession NVARCHAR(MAX), 
    @knowledgeOfLanguages NVARCHAR(MAX), 
    @workExperience NVARCHAR(MAX), 
    @hasDron BIT, 
    @hasCameraStabilizator BIT, 
    @hasGopro BIT, 
    @rating FLOAT
AS
	UPDATE Photographer
	SET Profession=@profession, KnowledgeOfLanguages=@knowledgeOfLanguages, WorkExperience=@workExperience, HasDron=@hasDron, HasCameraStabilizator=@hasCameraStabilizator, HasGopro=@hasGopro, Rating=@rating
	WHERE UserId=@userId
RETURN 0
