using APIPhoneBook.Dto;

namespace APIPhoneBook.Service.Contract
{
    public interface ICountryService
    {
        ServiceResponse<CountryDto> GetCountryById(int id);
        ServiceResponse<IEnumerable<CountryDto>> GetCountry();
    }
}
