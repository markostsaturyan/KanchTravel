Select * from Roles;

Insert into UsersDB.dbo.Users()
Values('Admin'),
('User'),
('Driver'),
('Guide'),
('Photographer');



Insert into Users (FirstName, LastName, Sex, DateOfBirth, PhoneNumber, Email, UserName, [Password], IsActive)
Values('Anjela', 'Badalyan', 1, '19960830', '+37498889928', 'anjelabadalyan96@gmail.com','anjela','anjela', 1 )