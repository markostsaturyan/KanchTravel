CREATE TABLE [dbo].[Drivers]
(
	[User_id] INT NOT NULL,
	[Car_id] INT NOT NULL,
	[Driving_licence_pic_front] IMAGE NOT NULL,
	[Driving_licence_pic_back] IMAGE NOT NULL, 
    [KnowledgeOfLanguages] NVARCHAR(MAX) NOT NULL, 
    [Rating] FLOAT NOT NULL,
	FOREIGN KEY (User_id) REFERENCES Users(Id)
)
