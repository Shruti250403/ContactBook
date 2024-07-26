using APIPhoneBook.Dto;
using APIPhoneBook.Models;

namespace APIPhoneBook.Service.Contract
{
    public interface IContactService
    {
        ServiceResponse<IEnumerable<ContactDto>> GetContact();

        ServiceResponse<string> AddContact(ContactModel contact);

        ServiceResponse<string> RemoveContact(int id);
        ServiceResponse<int> TotalContacts(char? letter, string? search);
        ServiceResponse<int> TotalContactFavourite(char? letter);
        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedFavouriteContacts(int page, int pageSize);
        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedFavouriteContacts(int page, int pageSize, char? letter);
        //ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, string sortOrder);

        ServiceResponse<string> ModifyContact(ContactModel contact);
        ServiceResponse<ContactDto> GetContact(int contactId);
        ServiceResponse<IEnumerable<ContactDto>> GetContactByLetter(char letter);
        //ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize);
        ServiceResponse<int> TotalContacts();
        ServiceResponse<int> TotalContacts(char? letter, string? searchQuery, string sortOrder);
        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, char? letter, string sortOrder, string? search);
        ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(int page, int pageSize, char? letter);
        ServiceResponse<int> TotalFavContacts(char? letter);
        ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(char? letter);
        ServiceResponse<IEnumerable<ContactSPDto>> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder);
        ServiceResponse<IEnumerable<ContactSPDto>> GetContactsByBirthMonth(int month);
        ServiceResponse<int> GetContactsCountByCountry(int countryId);
        ServiceResponse<IEnumerable<ContactSPDto>> GetContactsByState(int stateId);
        ServiceResponse<int> GetContactCountByGender(string gender);
    }
}
