using APIPhoneBook.Models;

namespace APIPhoneBook.Data.Contract
{
    public interface IStateRepository
    {
        IEnumerable<State> GetAll();
        State GetStateById(int id);

        int TotalStates();
        IEnumerable<State> GetAllStates();

        List<State> GetAllStateByCountryId(int countryId);
    }
}
