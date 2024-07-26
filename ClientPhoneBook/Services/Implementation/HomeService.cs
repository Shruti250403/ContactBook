using APIPhoneBook.Models;
using ClientPhoneBook.Data.Contract;

namespace ClientPhoneBook.Services.Implementation
{
    public class HomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }
        public IEnumerable<ContactModel> GetContacts(char letter)
        {
            var contacts = _homeRepository.GetAll(letter);
            if (contacts != null && contacts.Any())
            {
                return contacts;
            }
            return new List<ContactModel>();
        }

        public ContactModel? GetContact(int id)
        {
            var contact = _homeRepository.GetContact(id);
            return contact;
        }

        public int TotalContacts()
        {
            return _homeRepository.TotalContacts();
        }
        public IEnumerable<ContactModel> GetPaginatedContact(int page, int pageSize, char letter)
        {
            return _homeRepository.GetPaginatedContacts(page, pageSize, letter);
        }
    }
}
