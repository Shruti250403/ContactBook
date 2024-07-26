using APIPhoneBook.Data;
using APIPhoneBook.Models;
using ClientPhoneBook.Data.Contract;

namespace ClientPhoneBook.Data.Implementation
{
    public class ContactRepository:IContactRepository
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext context)
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

        public ContactModel? GetContact(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.ContactId == id);
            return contact;
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
        public bool InsertContact(ContactModel contact)
        {
            var result = false;
            if (contact != null)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }
        public bool ContactExists(string fname, string lname)
        {
            var existcontact = _context.Contacts.ToList().Find(c => c.FirstName == fname && c.LastName == lname);
            if (existcontact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool ContactExists(int categoryId, string fname, string lname)
        {
            var existcontact = _context.Contacts.ToList().FirstOrDefault(c => c.ContactId != categoryId && c.FirstName == fname && c.LastName == lname);
            if (existcontact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateContact(ContactModel contact)
        {
            var result = false;
            if (contact != null)
            {
                _context.Contacts.Update(contact);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool DeleteContact(int id)
        {
            var result = false;
            var contact = _context.Contacts.Find(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }
    }
}
