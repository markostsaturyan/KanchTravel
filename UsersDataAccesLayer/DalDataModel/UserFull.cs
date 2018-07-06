using System;
using Veldrid.ImageSharp;

namespace UsersDataAccesLayer
{
    public class UserFull
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
    }
}
