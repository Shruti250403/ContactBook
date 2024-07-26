using APIPhoneBook.Models;

namespace ClientPhoneBook.Data.Contract
{
    public interface IHomeRepository
    {
        IEnumerable<ContactModel> GetAll(char letter);

        int TotalContacts();
        ContactModel? GetContact(int id);
        IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, char letter);
    }
}
