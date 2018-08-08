CREATE TABLE [dbo].[Camera]
(
	[Id] INT NOT NULL PRIMARY KEY Identity, 
    [IsProfessional] BIT NOT NULL, 
    [Model] NVARCHAR(MAX) NOT NULL
)
