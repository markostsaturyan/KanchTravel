using Authentication.DataManagement.BusinessLogicLayer.BusiessLayerDataModel;
using Authentication.DataManagement.DataAccesLayer;
using Microsoft.Extensions.Configuration;

namespace Authentication.DataManagement.BusinessLogicLayer
{
    public class UserRepository : IUserRepository
    {
        private AuthDataAccessLayer dataAccessLayer;

        public UserRepository(IConfiguration configuration)
        {
            this.dataAccessLayer = new AuthDataAccessLayer(configuration["SqlConnection:ConnectionString"]);
        }

        public User FindUserAsync(string userName)
        {
            var user = dataAccessLayer.GetByUserName(userName);

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
            var user = dataAccessLayer.GetByUserId(id);

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
