using APIPhoneBook.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPhoneBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;


        public HomeController(IHomeService homeService)
        {
            this._homeService = homeService;
        }

        [HttpGet("GetAllContacts/{letter}")]

        public IActionResult GetAllContacts(char letter)
        {
            var response = _homeService.GetContacts(letter);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetContacts")]

        public IActionResult GetContacts()
        {
            var response = _homeService.GetAllContacts();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //[HttpPut("{id}/favorite")]
        //public IActionResult SetFavorite(int id, bool isFavorite)
        //{
        //    var response = _homeService.GetContact(id);
        //    if (response == null)
        //    {
        //        return NotFound();
        //    }

        //    response.Data.Favorites = isFavorite;
        //    return Ok(response);
        //}
        [HttpGet("GetContactById/{id}")]

        public IActionResult GetContactById(int id)
        {
            var response = _homeService.GetContact(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
