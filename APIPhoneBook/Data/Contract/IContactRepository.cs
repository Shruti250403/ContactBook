using APIPhoneBook.Dto;
using APIPhoneBook.Models;

namespace APIPhoneBook.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<ContactModel> GetAll();
        int TotalContacts();
        int TotalContacts(char? letter, string? searchQuery, string sortOrder);
        IEnumerable<ContactModel> GetPaginatedFavouriteContacts(int page, int pageSize);
        //IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, string sortOrder);
        IEnumerable<ContactModel> GetPaginatedFavouriteContacts(int page, int pageSize, char? letter);

        ContactModel? GetContact(int id);
        IEnumerable<ContactModel> GetByLetter(char letter);
        bool ContactExists(string contactNumber);
        int TotalContactFavourite(char? letter);

        bool ContactExists(int ContactId, string contactNumber);

        bool InsertContact(ContactModel contact);

        bool UpdateContact(ContactModel contact);

        bool DeleteContact(int id);
        IEnumerable<ContactModel> GetFavouriteContacts(int page, int pageSize, char? letter);
        int TotalFavContacts(char? letter);
        int TotalContacts(char? letter, string? search);
        IEnumerable<ContactModel> GetAllFavouriteContacts(char? letter);
        IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, char? letter, string sortOrder, string? search);
        IEnumerable<ContactSPDto> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder);
        IEnumerable<ContactSPDto> GetContactsByBirthMonth(int month);
        int GetContactsCountByCountry(int countryId);
        IEnumerable<ContactSPDto> GetContactsByState(int stateId);
        int GetContactCountByGender(string gender);
    }
}

