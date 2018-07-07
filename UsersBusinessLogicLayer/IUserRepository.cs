using UsersDataModel;

namespace UsersBusinessLogicLayer
{
    public interface IUserRepository
    {
        User FindUserAsync(string userName);
        User FindUserAsync(long v);
    }
}