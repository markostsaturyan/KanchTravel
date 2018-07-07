using System;
using UsersDataModel;

namespace UsersBusinessLogicLayer
{
    public class UserRepository:IUserRepository
    {
        public User FindUserAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public User FindUserAsync(long v)
        {
            throw new NotImplementedException();
        }
    }
}
