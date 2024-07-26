using APIPhoneBook.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPhoneBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _stageService;
        public CountryController(ICountryService stageService)
        {
            _stageService = stageService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var response = _stageService.GetCountry();
            if (!response.Success)
            {
                return NotFound(Response);
            }

            return Ok(response);
        }
        [HttpGet("GetCountryById/{id}")]
        public IActionResult GetCountryById(int id)
        {
            var response = _stageService.GetCountryById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
