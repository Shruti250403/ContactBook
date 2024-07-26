using APIPhoneBook.Data.Contract;
using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Contract;
using System.Diagnostics.CodeAnalysis;

namespace APIPhoneBook.Service.Implementation
{
    public class StateService:IStateService
    {
        private readonly IStateRepository _standardRepository;
        public StateService(IStateRepository standardRepository)
        {
            _standardRepository = standardRepository;
        }
        public ServiceResponse<StateDto> GetStateById(int id)
        {
            var response = new ServiceResponse<StateDto>();

            var stage = _standardRepository.GetStateById(id);

            var stageDto = new StateDto()
            {

                StateId = stage.StateId,
                StateName = stage.StateName,
                CountryId = stage.CountryId,
                Country = new Models.Country()
                {
                    CountryId = stage.Country.CountryId,
                    CountryName = stage.Country.CountryName,
                }
            };

            response.Data = stageDto;
            return response;
        }
        [ExcludeFromCodeCoverage]
        public int TotalStates()
        {
            return _standardRepository.TotalStates();
        }
        public ServiceResponse<List<StateDto>> GetAllStateByCountryId(int divisionId)
        {
            var response = new ServiceResponse<List<StateDto>>();
            var existingPosition = _standardRepository.GetAllStateByCountryId(divisionId);
            if (existingPosition != null && existingPosition.Any())
            {

                List<StateDto> divisionDtos = new List<StateDto>();
                foreach (var position in existingPosition)
                {
                    divisionDtos.Add(new StateDto()
                    {
                        StateId = position.StateId,
                        StateName = position.StateName,
                        CountryId = position.CountryId,
                    });
                }
                response.Data = divisionDtos;

            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }

        public ServiceResponse<IEnumerable<StateDto>> GetStates()
        {
            var response = new ServiceResponse<IEnumerable<StateDto>>();
            var categories = _standardRepository.GetAll();
            if (categories != null && categories.Any())
            {
                //  categories.Where(c => c.FileName == string.Empty).ToList();
                List<StateDto> categoryDtos = new List<StateDto>();
                foreach (var category in categories.ToList())
                {
                    categoryDtos.Add(new StateDto()
                    {
                        StateId = category.StateId,
                        StateName = category.StateName,
                        CountryId = category.CountryId,
                        Country = new Country()
                        {
                            CountryId = category.CountryId,
                            CountryName = category.Country.CountryName,
                        }

                    });
                }
                response.Data = categoryDtos;
                return response;
            }
            else
            {
                response.Data = new List<StateDto>();
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }
    }
}
