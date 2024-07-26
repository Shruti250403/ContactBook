using APIPhoneBook.Models;

namespace ClientPhoneBook.Services.Contract
{
    public interface IContactService
    {
        IEnumerable<ContactModel> GetContacts(char letter);

        ContactModel? GetContact(int id);

        int TotalContacts();
        IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, char letter);
        string AddContact(ContactModel contact);
        string ModifyContact(ContactModel contact);

        string RemoveContact(int id);
    }
}
