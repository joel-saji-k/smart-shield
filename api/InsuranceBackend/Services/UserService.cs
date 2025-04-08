using InsuranceBackend.Models;
using Microsoft.Data.SqlClient;
using InsuranceBackend.Database;

namespace InsuranceBackend.Services
{
    public class UserService
    {
        private InsuranceDbContext _context;

        public UserService()
        {
            _context = new InsuranceDbContext();
        }

        public UserModel GetUserByName(string userName)
        {
            var validUser = _context.Users.FirstOrDefault(u => u.UserName == userName);
            return validUser;
        }

        public UserModel? GetUser(int userID)
        {
            var validUser = _context.Users.FirstOrDefault(u => u.UserId == userID);
            return validUser;
        }

        public UserModel AddUser(UserModel user)
        {
            var test =_context.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (test == null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return _context.Users.OrderBy(u => u.UserId).Last();
            }
            else return test;
        }

        public void DeleteUser(UserModel user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public UserModel UpdateUser(UserModel user)
        {
            var dbuser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            var testuser = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (dbuser != null)
            {
                if (testuser != null)
                {
                    if (dbuser.UserName == testuser.UserName)
                    {
                        if (dbuser == testuser)
                        {
                            dbuser.UserName = user.UserName;
                            dbuser.Password = user.Password;
                            _context.Users.Update(dbuser);
                            _context.SaveChanges();
                            return _context.Users.First(u => u.UserId == user.UserId);
                        }
                    }
                    else
                    {
                        return new() { UserId = -1 };
                    }
                } 
             else
                {
                    dbuser.UserName = user.UserName;
                    dbuser.Password = user.Password;
                    _context.Users.Update(dbuser);
                    _context.SaveChanges();
                    return _context.Users.First(u => u.UserId == user.UserId);
                }
            }
            return new() { UserId = 0 };
        }
    }
}
