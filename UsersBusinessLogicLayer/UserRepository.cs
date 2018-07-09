using System;
using UsersDataModel;
using UsersDataAccesLayer;
namespace UsersBusinessLogicLayer
{
    public class UserRepository:IUserRepository
    {
        public UserRepository() { }

        public User FindUserAsync(string userName)
        {
            var user = UsersDAL.GetByUserName(userName);
            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }

        public User FindUserAsync(long v)
        {
            var user = UsersDAL.GetByUserID(v);
            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }
    }
}
