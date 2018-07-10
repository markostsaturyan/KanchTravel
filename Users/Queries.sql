Select * from Roles;

Insert into UsersDB.dbo.Users()
Values('Admin'),
('User'),
('Driver'),
('Guide'),
('Photographer');

Delete FRom Users where Users.Id=6


Select * from Users

Insert into Users (FirstName, LastName, Gender, DateOfBirth, PhoneNumber, Email, UserName, [Password], IsActive)
Values('Anjela', 'Badalyan', 1, '19960830', '+37498889928', 'anjelabadalyan96@gmail.com','anjela','anjela', 1 )

	SELECT UserId, FirstName, LastName, DateOfBirth, Email, PhoneNumber, Picture, UserName, [Password], IsActive, 
	DrivingLicencePicBack, DrivingLicencePicFront, KnowledgeOfLanguages, Rating, 
	Car.Id as CarId, Car.Brand as Brand, CarPicture1, CarPicture2, CarPicture3, FuelType, Car.HasAirConditioner, Car.HasKitchen, Car.HasMicrophone, Car.HasToilet
	FROM Users JOIN Drivers ON Users.Id = Drivers.UserId
	Join Car On Drivers.CarId=Car.Id

	SELECT Users.Id, FirstName, LastName, UserName, [Password], Email, Roles.RoleName, IsActive
	FROM Users join UserRole on Users.Id=UserId
		 join Roles on RoleId=Roles.Id

		