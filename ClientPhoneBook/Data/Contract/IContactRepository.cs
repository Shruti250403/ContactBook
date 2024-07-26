using APIPhoneBook.Models;

namespace ClientPhoneBook.Data.Contract
{
    public interface IContactRepository
    {
        bool ContactExists(string fname, string lname);

        bool ContactExists(int categoryId, string fname, string lname);
        IEnumerable<ContactModel> GetAll(char letter);

        int TotalContacts();

        IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, char letter);

        ContactModel? GetContact(int id);

        bool InsertContact(ContactModel contact);
        bool UpdateContact(ContactModel contact);

        bool DeleteContact(int id);
    }
}
