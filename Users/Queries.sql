Select * from UsersDB.dbo.Roles;

Insert into UsersDB.dbo.Roles(RoleName)
Values('Admin'),
('User'),
('Driver'),
('Guide'),
('Photographer');



Insert into Users (FirstName, LastName, Gender, DateOfBirth, PhoneNumber, Email, UserName, [Password], IsActive,UserGuid,IsApproved)
Values('Anjela', 'Badalyan', 'Male', '19960830', '+37498889928', 'anjelabadalyan96@gmail.com','anjela','anjela', 1, 10, 0)

	SELECT UserId, FirstName, LastName, DateOfBirth, Email, PhoneNumber, Picture, UserName, [Password], IsActive, 
	DrivingLicencePicBack, DrivingLicencePicFront, KnowledgeOfLanguages, Rating, 
	Car.Id as CarId, Car.Brand as Brand, CarPicture1, CarPicture2, CarPicture3, FuelType, Car.HasAirConditioner, Car.HasKitchen, Car.HasMicrophone, Car.HasToilet
	FROM Users JOIN Drivers ON Users.Id = Drivers.UserId
	Join Car On Drivers.CarId=Car.Id