using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.DataManagement.BusinessLogicLayer.BusiessLayerDataModel;
using Authentication.DataManagement.DataAccesLayer;

namespace Authentication.DataManagement.BusinessLogicLayer
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
        }

        public User FindUserAsync(string userName)
        {
            var user = AuthDataAccessLayer.GetByUserName(userName);

            return new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }

        public User FindUserAsync(long id)
        {
            var user = AuthDataAccessLayer.GetByUserId(id);

            return new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }
    }
}
