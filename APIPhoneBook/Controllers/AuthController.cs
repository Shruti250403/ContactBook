using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Contract;
using APIPhoneBook.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPhoneBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(ForgetDto forgetDto)
        {
            var response = _service.ForgetPasswordService(forgetDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(RegisterDto registerDto)
        {
            var response = _service.RegisterUserService(registerDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpPost("LoginUser")]

        public IActionResult LoginUser(LoginDto loginDto)
        {
            var response = _service.LoginUserService(loginDto);
            return !response.Success ? BadRequest(response) : Ok(response);
        }
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(string id)
        {
            var response = _service.GetUser(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPut("ModifyUser")]
        public IActionResult UpdateUser(UpdateUserDto userDto)
        {
            var existingUser = _service.GetUser(userDto.LoginId);
            if (existingUser == null)
            {
                return BadRequest("User not found.");
            }
            var contact = new User()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                LoginId = userDto.LoginId,
                ContactNumber = userDto.ContactNumber,
                FileName = userDto.FileName,
                ImageByte = userDto.ImageByte,
            };
            var response = _service.ModifyUser(contact,existingUser.Data.userId,existingUser.Data.Email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
        
    }
}
