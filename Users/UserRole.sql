CREATE TABLE [dbo].[UserRole]
(
	[UserId] INT NOT NULL, 
    [RoleId] INT NOT NULL,
	FOREIGN KEY(UserId) REFERENCES Users(Id),
	FOREIGN KEY(RoleId) REFERENCES Roles(Id)
)
