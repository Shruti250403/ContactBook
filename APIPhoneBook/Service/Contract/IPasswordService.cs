using APIPhoneBook.Models;

namespace APIPhoneBook.Service.Contract
{
    public interface IPasswordService
    {
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
