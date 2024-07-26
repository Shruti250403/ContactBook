using APIPhoneBook.Data.Contract;
using APIPhoneBook.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace APIPhoneBook.Data.Implementation
{
    public class AuthRepository:IAuthRepository
    {
        private readonly IAppDbContext _appDbContext;

        public AuthRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool RegisterUser(User user)
        {
            var result = false;
            if (user != null)
            {
                _appDbContext.Users.Add(user);
                _appDbContext.SaveChanges();

                result = true;
            }

            return result;
        }
        public User? GetUser(string id)
        {
            var user = _appDbContext.Users
                .FirstOrDefault(c => c.LoginId == id || c.Email==id);
            return user;
        }
        public bool UserExist(int userId, string loginId, string email)
        {
            if (_appDbContext.Users.Any(c => c.userId != userId && (c.LoginId.ToLower() == loginId.ToLower() || c.Email.ToLower() == email.ToLower())))
            {
                return true;
            }
            return false;
        }
        [ExcludeFromCodeCoverage]
        public void SaveChanges()
        {
            _appDbContext.SaveChanges();
        }

        public User? ValidateUser(string Username)
        {
            User? user = _appDbContext.Users.FirstOrDefault(c => c.LoginId.ToLower() == Username.ToLower() || c.Email == Username.ToLower());
            return user;
        }

        public bool UserExists(string loginId, string email)
        {
            if (_appDbContext.Users.Any(c => c.LoginId.ToLower() == loginId.ToLower() || c.Email.ToLower() == email.ToLower()))
            {
                return true;
            }

            return false;
        }
        public bool UpdateUser(User user)
        {
            var result = false;
            if (user != null)
            {
                _appDbContext.Users.Update(user);
                _appDbContext.SaveChanges();

                result = true;
            }
            return result;

        }
    }
}
