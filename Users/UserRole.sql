CREATE TABLE [dbo].[UserRole]
(
	[User_id] INT NOT NULL, 
    [Role_id] INT NOT NULL,
	FOREIGN KEY(User_id) REFERENCES Users(Id),
	FOREIGN KEY(Role_id) REFERENCES Roles(Id)
)
