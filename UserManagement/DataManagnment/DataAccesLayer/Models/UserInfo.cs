using System;
using Veldrid.ImageSharp;

namespace UserManagement.DataManagnment.DataAccesLayer.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DataOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ImageSharpTexture Image { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
