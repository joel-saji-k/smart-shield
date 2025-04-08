using InsuranceBackend.Models;

namespace InsuranceBackend.Services.Contracts
{
    public interface IUserService
    {
        User GetUserByName(string userName);
        User? GetUser(int userID);
        User AddUser(User user);
        void DeleteUser(User user);
        User UpdateUser(User user);
    }
}
