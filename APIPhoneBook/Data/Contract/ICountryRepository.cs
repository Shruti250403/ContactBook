using APIPhoneBook.Models;

namespace APIPhoneBook.Data.Contract
{
    public interface ICountryRepository
    {
        Country GetCountryById(int id);
        IEnumerable<Country> GetAll();
        int TotalCountries();
    }
}
