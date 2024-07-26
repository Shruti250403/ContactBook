using APIPhoneBook.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPhoneBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StateController : ControllerBase
    {
        private readonly IStateService _standardService;


        public StateController(IStateService standardService)
        {

            _standardService = standardService;
        }
        [HttpGet("GetAllStateByCountryId/{id}")]

        public IActionResult GetAllStandardByCountryId(int id)
        {
            var response = _standardService.GetAllStateByCountryId(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetStates")]

        public IActionResult GetStates()
        {
            var response = _standardService.GetStates();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("GetStateById/{id}")]
        public IActionResult GetStateById(int id)
        {
            var response = _standardService.GetStateById(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
