CREATE PROCEDURE [dbo].[InsertPhotographer]
	@userId INT,
	@cameraId INT,
    @profession NVARCHAR(MAX),
    @knowledgeOfLanguages NVARCHAR(MAX),
    @workExperience NVARCHAR(MAX),
    @hasDron BIT,
    @hasCameraStabilizator BIT,
    @hasGopro BIT,
    @rating FLOAT
AS
	INSERT INTO Photographer
	VALUES(@userId, @cameraId, @profession, @knowledgeOfLanguages, @workExperience, @hasDron, @hasCameraStabilizator, @hasGopro, @rating, @numberOfAppraisers)
RETURN 0
