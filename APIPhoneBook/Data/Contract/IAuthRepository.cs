using APIPhoneBook.Models;

namespace APIPhoneBook.Data.Contract
{
    public interface IAuthRepository
    {
        bool RegisterUser(User user);

        User? ValidateUser(string Username);

        bool UserExists(string loginId, string email);
        bool UpdateUser(User user);
        void SaveChanges();
        User? GetUser(string id);
        bool UserExist(int userId, string loginId, string email);
    }
}
