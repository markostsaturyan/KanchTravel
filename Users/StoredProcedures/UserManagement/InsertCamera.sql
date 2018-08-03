CREATE PROCEDURE [dbo].[InsertCamera]
	@isProfessional bit,
	@model nvarchar(Max)
AS
	INSERT INTO Camera(IsProfessional, Model) 
	Output inserted.Id
	VALUES(@isProfessional, @model)
	

RETURN 0
