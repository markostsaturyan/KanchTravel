CREATE PROCEDURE [dbo].[UpdateCameraInfo]
	@id int,
	@isProfessional bit,
	@model nvarchar(Max)
AS
	UPDATE Camera
	SET IsProfessional=@isProfessional, Model=@model
	WHERE Id=@id
RETURN 0
