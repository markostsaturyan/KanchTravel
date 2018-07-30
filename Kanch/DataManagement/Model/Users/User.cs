using System.Drawing;

namespace Kanch.DataManagement.Model.Users
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Image Img { get; set; }

        public string UserName { get; set; }
    }
}
