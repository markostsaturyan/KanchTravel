CREATE PROCEDURE [dbo].[InsertCamera]
	@isProfessional bit,
	@model nvarchar(Max)
AS
	INSERT INTO Camera(IsProfessional, Model) 
	OUTPUT SCOPE_IDENTITY() 
	VALUES(@isProfessional, @model)
RETURN 0
