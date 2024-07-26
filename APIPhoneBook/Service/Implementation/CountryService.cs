using APIPhoneBook.Data.Contract;
using APIPhoneBook.Dto;
using APIPhoneBook.Service.Contract;

namespace APIPhoneBook.Service.Implementation
{
    public class CountryService:ICountryService
    {
        private readonly ICountryRepository _stageRepository;

        public CountryService(ICountryRepository stageRepository)
        {
            _stageRepository = stageRepository;
        }
        public ServiceResponse<CountryDto> GetCountryById(int id)
        {
            var response = new ServiceResponse<CountryDto>();

            var stage = _stageRepository.GetCountryById(id);

            var stageDto = new CountryDto()
            {
                CountryId = stage.CountryId,
                CountryName = stage.CountryName,

            };

            response.Data = stageDto;
            return response;
        }
        public ServiceResponse<IEnumerable<CountryDto>> GetCountry()
        {
            var response = new ServiceResponse<IEnumerable<CountryDto>>();
            var stages = _stageRepository.GetAll();
            if (stages != null && stages.Any())
            {

                List<CountryDto> stageDtos = new List<CountryDto>();
                foreach (var stage in stages)
                {
                    stageDtos.Add(new CountryDto()
                    {
                        CountryId = stage.CountryId,
                        CountryName = stage.CountryName,
                    });
                }
                response.Data = stageDtos;
                return response;
            }
            else
            {
                //response.Data = new List<CountryDto>();
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }
    }
}
