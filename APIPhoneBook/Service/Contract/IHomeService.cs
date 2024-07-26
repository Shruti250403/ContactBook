using APIPhoneBook.Dto;

namespace APIPhoneBook.Service.Contract
{
    public interface IHomeService
    {
        ServiceResponse<IEnumerable<ContactDto>> GetContacts(char letter);

        ServiceResponse<ContactDto> GetContact(int contactId);

        ServiceResponse<IEnumerable<ContactDto>> GetAllContacts();
    }
}
