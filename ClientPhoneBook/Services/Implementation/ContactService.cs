using ClientPhoneBook.Data.Contract;

namespace ClientPhoneBook.Services.Implementation
{
    public class ContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IEnumerable<ContactModel> GetContacts(char letter)
        {
            var contacts = _contactRepository.GetAll(letter);
            if (contacts != null && contacts.Any())
            {
                return contacts;
            }
            return new List<ContactModel>();
        }

        public int TotalContacts()
        {
            return _contactRepository.TotalContacts();
        }
        public IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, char letter)
        {
            return _contactRepository.GetPaginatedContacts(page, pageSize, letter);
        }

        public string AddContact(ContactModel contact)
        {
            if (_contactRepository.ContactExists(contact.FirstName, contact.LastName))
            {
                return "Contact already exists.";
            }
            /*var filename = string.Empty;
            if (file != null && file.Length > 0)
            {/*, IFormFile file*//*
            //Process the uploaded file(eg. Save it to disk)
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);

                //Save the file to storage and set path

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    filename = file.FileName;
                }
                contact.FileName = filename;*/



            var result = _contactRepository.InsertContact(contact);
            return result ? "Contact saved successfully." : "Something went wrong.Try again later";

        }

        public ContactModel? GetContact(int id)
        {
            var contact = _contactRepository.GetContact(id);
            return contact;
        }

        public string RemoveContact(int id)
        {
            var result = _contactRepository.DeleteContact(id);
            if (result)
            {
                return "Contact deleted successfully.";

            }
            else
            {
                return "Something went wrong.Try again later";
            }
        }
        public string ModifyContact(ContactModel contact)
        {
            var message = string.Empty;
            if (_contactRepository.ContactExists(contact.ContactId, contact.FirstName, contact.LastName))
            {
                message = "Contact already exists.";
            }
            var existingcontact = _contactRepository.GetContact(contact.ContactId);
            var result = false;
            if (existingcontact != null)
            {
                existingcontact.FirstName = contact.FirstName;
                existingcontact.LastName = contact.LastName;
                existingcontact.Email = contact.Email;
                existingcontact.Phone = contact.Phone;
                existingcontact.Company = contact.Company;

                result = _contactRepository.UpdateContact(existingcontact);
            }
            message = result ? "Contact updated successfully." : "Something went wrong.Try again later.";
            return message;
        }
    }
}
