﻿CREATE TABLE [dbo].[Guide]
(
	[UserId] INT NOT NULL,
	[EducationGrade] NVARCHAR(MAX) NOT NULL, 
    [Profession] NVARCHAR(MAX) NOT NULL, 
    [KnowledgeOfLanguages] NVARCHAR(MAX) NOT NULL, 
    [WorkExperience] NVARCHAR(MAX) NOT NULL, 
    FOREIGN KEY (UserId) REFERENCES Users(Id)
)
