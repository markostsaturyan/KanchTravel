Select * from UsersDB.dbo.Roles;

Insert into UsersDB.dbo.Roles(RolesName)
Values('Admin'),
('User'),
('Driver'),
('Guide'),
('Photographer');



Insert into UsersDB.dbo.Users (FirstName, LastName, Sex, DateOfBirth, PhoneNumber, Email, UserName, [Password], IsActive)
Values('Anjela', 'Badalyan', 1, '19960830', '+37498889928', 'anjelabadalyan96@gmail.com','anjela','anjela', 1 )

	SELECT UserId, FirstName, LastName, DateOfBirth, Email, PhoneNumber, Picture, UserName, [Password], IsActive, 
	DrivingLicencePicBack, DrivingLicencePicFront, KnowledgeOfLanguages, Rating, 
	Car.Id as CarId, Car.Brand as Brand, CarPicture1, CarPicture2, CarPicture3, FuelType, Car.HasAirConditioner, Car.HasKitchen, Car.HasMicrophone, Car.HasToilet
	FROM Users JOIN Drivers ON Users.Id = Drivers.UserId
	Join Car On Drivers.CarId=Car.Id