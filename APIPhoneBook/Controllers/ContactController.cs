using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace APIPhoneBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService categoryService)
        {
            _contactService = categoryService;
        }
        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts()
        {
            var response = _contactService.GetContact();
            if (!response.Success)
            {
                return NotFound(Response);
            }
            return Ok(response);
        }
        [HttpGet("GetAllContactsByPaginationSP")]
        public IActionResult GetPaginatedContactsSP(char? letter, string? search, int page = 1, int pageSize = 4, string sortOrder = "asc")
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();

            response = _contactService.GetPaginatedContactsSP(page, pageSize, letter, search, sortOrder);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        //[Authorize]
        [HttpPost("Create")]
        public IActionResult AddContact(AddContactDto contactDto)
        {
            if (ModelState.IsValid)
            {
                var contact = new ContactModel()
                {
                    FirstName = contactDto.FirstName,
                    LastName = contactDto.LastName,
                    Email = contactDto.Email,
                    Phone = contactDto.Phone,
                    Company = contactDto.Company,
                    Image = contactDto.Image,
                    Gender = contactDto.Gender,
                    Favourites = contactDto.Favourites,
                    CountryId = contactDto.CountryId,
                    StateId = contactDto.StateId,
                    ImageByte=contactDto.ImageByte,
                    BirthDate=contactDto.BirthDate,
                   

                };
                var result = _contactService.AddContact(contact);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        //[Authorize]
        [HttpPut("ModifyContact")]
        public IActionResult UpdateContact(UpdateContactDto contactDto)
        {
            var contact = new ContactModel()
            {
                ContactId = contactDto.ContactId,
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email,
                Phone = contactDto.Phone,
                Company = contactDto.Company,
                BirthDate = contactDto.BirthDate,
                ImageByte = contactDto.ImageByte,
                Image = contactDto.Image,
                Gender = contactDto.Gender,
                Favourites = contactDto.Favourites,
                CountryId = contactDto.CountryId,
                StateId = contactDto.StateId

            };
            var response = _contactService.ModifyContact(contact);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
        [Authorize]
        [HttpDelete("Remove/{id}")]
        public IActionResult RemoveContact(int id)
        {
            if (id > 0)
            {
                var response = _contactService.RemoveContact(id);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            else
            {
                return BadRequest("Please enter proper data.");
            }
        }
        [HttpGet("GetContactById/{id}")]

        public IActionResult GetContactById(int id)
        {
            var response = _contactService.GetContact(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetAllContactsByPagination")]
        public IActionResult GetPaginatedContacts(char? letter, string? search, int page = 1, int pageSize = 4, string sortOrder = "asc")
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();

            response = _contactService.GetPaginatedContacts(page, pageSize, letter, sortOrder,search);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetContactsCount")]
        public IActionResult GetTotalCountOfContacts(char? letter, string? search)
        {
            var response = _contactService.TotalContacts(letter,search);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetTotalCountOfFavContacts")]
        public IActionResult GetTotalCountOfFavContacts(char? letter)
        {
            var response = _contactService.TotalFavContacts(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("favourites")]
        public IActionResult GetFavouriteContacts(char? letter, int page = 1, int pageSize = 2)
        {
            var response = _contactService.GetFavouriteContacts(page, pageSize, letter);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetPaginatedFavouriteContacts")]
        [ExcludeFromCodeCoverage]
        public IActionResult GetPaginatedFavouriteContacts(char? letter, int page = 1, int pageSize = 2)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            if (letter != null)
            {
                response = _contactService.GetPaginatedFavouriteContacts(page, pageSize, letter);
            }
            else
            {
                response = _contactService.GetPaginatedFavouriteContacts(page, pageSize);

            }
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("TotalContactFavourite")]
        [ExcludeFromCodeCoverage]
        public IActionResult TotalContactFavourite(char? letter)
        {
            var response = _contactService.TotalContactFavourite(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetAllFavouriteContacts")]
        public IActionResult GetAllFavouriteContacts(char? letter)
        {
            var response = _contactService.GetFavouriteContacts(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetContactsByBirthMonthSP")]
        public IActionResult GetContactsByBirthMonth(int month)
        {
            var response = _contactService.GetContactsByBirthMonth(month);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetContactsCountByCountrySP")]
        public IActionResult GetContactsCountByCountry(int countryId)
        {
            var response = _contactService.GetContactsCountByCountry(countryId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetContactsByStateSP")]
        public IActionResult GetContactsByState(int stateId)
        {
            var response = _contactService.GetContactsByState(stateId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet("GetContactCountByGenderSP")]
        public IActionResult GetContactCountByGender(string gender)
        {
            var response = _contactService.GetContactCountByGender(gender);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }



    }
}
