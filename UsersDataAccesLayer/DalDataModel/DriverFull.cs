using System;
using Veldrid.ImageSharp;

namespace UsersDataAccesLayer.DalDataModel
{
    public class DriverFull
    {
        public int Id;
        public string FirstName;
        public string LastName;
        public DateTime DataOfBirth;
        public string PhoneNumber;
        public string Email;
        public ImageSharpTexture Image;
        public string UserName;
        public string Password;
        public Car Car;
        public ImageSharpTexture DrivingLicencePicFront;
        public ImageSharpTexture DrivingLicencePicBack;
        public string KnowledgeOfLanguages;
        public double Raiting;
    }
}
