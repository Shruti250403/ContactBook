using APIPhoneBook.Data;
using APIPhoneBook.Models;
using ClientPhoneBook.Data.Contract;

namespace ClientPhoneBook.Data.Implementation
{
    public class HomeRepository:IHomeRepository
    {
        private readonly AppDbContext _context;

        public HomeRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ContactModel> GetAll(char letter)
        {
            var contacts = _context.Contacts.Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
            if (contacts != null)
            {
                return contacts;
            }
            return new List<ContactModel>();
        }


        public int TotalContacts()
        {
            return _context.Contacts.Count();
        }
        public IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, char letter)
        {
            int skip = (page - 1) * pageSize;
            return _context.Contacts
                .Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList()
                .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        public ContactModel? GetContact(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.ContactId == id);
            return contact;
        }
    }
}
