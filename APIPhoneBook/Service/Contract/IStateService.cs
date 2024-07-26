using APIPhoneBook.Dto;

namespace APIPhoneBook.Service.Contract
{
    public interface IStateService
    {
        ServiceResponse<StateDto> GetStateById(int id);
        int TotalStates();
        ServiceResponse<List<StateDto>> GetAllStateByCountryId(int divisionId);
        ServiceResponse<IEnumerable<StateDto>> GetStates();
    }
}
