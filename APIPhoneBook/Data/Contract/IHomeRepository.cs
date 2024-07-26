using APIPhoneBook.Models;

namespace APIPhoneBook.Data.Contract
{
    public interface IHomeRepository
    {
        IEnumerable<ContactModel> GetAll(char letter);

        IEnumerable<ContactModel> GetAllContacts();
        int TotalContacts();
        ContactModel? GetContact(int id);
        IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, char letter);
    }
}
