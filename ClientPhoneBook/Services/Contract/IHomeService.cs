
using APIPhoneBook.Models;

namespace ClientPhoneBook.Services.Contract
{
    public interface IHomeService
    {
        IEnumerable<ContactModel> GetContacts(char letter);

        ContactModel? GetContact(int id);
        int TotalContacts();
    }
}
