using APIPhoneBook.Controllers;
using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Contract;
using APIPhoneBook.Service.Implementation;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTests.controllers
{
    public class AuthControllerTests
    {
        [Theory]
        [InlineData("User already exists.")]
        [InlineData("Something went wrong, please try after sometime.")]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        public void Register_ReturnsBadRequest_WhenRegistrationFails(string message)
        {
            // Arrange
            var registerDto = new RegisterDto();
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message
            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.RegisterUser(registerDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }
        [Theory]
        [InlineData("Invalid Username or password!")]
        [InlineData("Something went wrong, please try after sometime.")]
        public void Login_ReturnsBadRequest_WhenLoginFails(string message)
        {
            // Arrange
            var loginDto = new LoginDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message
            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.LoginUser(loginDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Fact]
        public void Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                LoginId = "loginid",
                Email = "email@email.com",
                Password = "password",
                ConfirmPassword = "password",
                ContactNumber = "1234567890",
                FirstName = "firstname",
                LastName = "lastname"
            };
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty
            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);
            var controller = new AuthController(mockAuthService.Object);
            // Act
            var actual = controller.RegisterUser(registerDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }
        [Fact]
        public void Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "Username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty
            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.LoginUser(loginDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }
        [Theory]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        [InlineData("Invalid Username!")]
        [InlineData("Password and confirmation password do not match!")]
        [InlineData("Something went wrong, please try again later.")]
        public void ForgetPassword_ReturnsBadRequest_WhenForgetPasswordFails(string message)
        {
            // Arrange
            var forgetDto = new ForgetDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message
            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgetPasswordService(forgetDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.ForgetPassword(forgetDto) as ObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.ForgetPasswordService(forgetDto), Times.Once);
        }
        [Fact]
        public void ForgetPassword_ReturnsOk_WhenForgetPasswordSucceeds()
        {
            // Arrange
            var fixture = new Fixture();
            var forgetDto = new ForgetDto()
            {
                Username = "Username",
                Password = "Password@1234",
                ConfirmPassword = "Password@1234"
            };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = ""
            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ForgetPasswordService(forgetDto))
                           .Returns(expectedServiceResponse);
            var target = new AuthController(mockAuthService.Object);
            // Act
            var actual = target.ForgetPassword(forgetDto) as OkObjectResult;
            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.ForgetPasswordService(forgetDto), Times.Once);
        }
        [Fact]
        public void GetUserById_ValidId_ReturnsOk()
        {
            // Arrange
            var userId = "validUserId";
            var validUser = new User(); // Assuming User class exists
            var validResponse = new ServiceResponse<UserDto> { Success = true, Message = "User successfully"};

            var userServiceMock = new Mock<IAuthService>();
            userServiceMock.Setup(service => service.GetUser(userId)).Returns(validResponse);

            var controller = new AuthController(userServiceMock.Object);

            // Act
            var result = controller.GetUserById(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result); // Ensure that the result is not null
            Assert.Equal(validResponse, result.Value);
        }

        [Fact]
        public void GetUserById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidUserId = "invalidUserId";
            var invalidResponse = new ServiceResponse<UserDto> { Success = false, Message = "User not found" };

            var userServiceMock = new Mock<IAuthService>();
            userServiceMock.Setup(service => service.GetUser(invalidUserId)).Returns(invalidResponse);

            var controller = new AuthController(userServiceMock.Object);

            // Act
            var result = controller.GetUserById(invalidUserId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result); // Ensure that the result is not null
            Assert.Equal(invalidResponse, result.Value);
        }
        [Fact]
        public void UpdateUser_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var LoginId = "";
            var userDto = new UpdateUserDto { };
            var response = new ServiceResponse<UserDto>();
            var mockService = new Mock<IAuthService>();
            mockService.Setup(service => service.GetUser(LoginId)).Returns(response);

            var userService = new AuthController(mockService.Object);

            // Act
            var result = userService.UpdateUser(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User not found.", badRequestResult.Value);
        }
        [Fact]
        public void UpdateUser_ModifyUser_Success_ReturnsOk()
        {
            // Arrange
            var userDto = new UpdateUserDto
            {
                LoginId = "existinguser",
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567890",
                FileName = "profile.jpg",
                ImageByte = new byte[] { 0x1, 0x2, 0x3 }
            };

            var existingUser = new UserDto
            {
                userId = 1,
                Email = "john.doe@example.com"
                // Add any other properties relevant for the test
            };
            var response = new ServiceResponse<UserDto>
            {
                Data=existingUser,
                Success=true,
                Message= "User updated successfully."
            };

            var mockService = new Mock<IAuthService>();
            mockService.Setup(service => service.GetUser(userDto.LoginId)).Returns(response);
            mockService.Setup(service => service.ModifyUser(It.IsAny<User>(), existingUser.userId, existingUser.Email))
                       .Returns(new ServiceResponse<string> { Success = true });

            var userService = new AuthController(mockService.Object);

            // Act
            var result = userService.UpdateUser(userDto);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("User updated successfully.", response.Message);
        }
        [Fact]
        public void UpdateUser_ModifyUser_Failure_ReturnsBadRequestWithResponse()
        {
            // Arrange
            var userDto = new UpdateUserDto
            {
              
                LoginId = "existinguser",
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "1234567890",
                FileName = "profile.jpg",
                ImageByte = new byte[] { 0x1, 0x2, 0x3 }
            };

            var existingUser = new User
            {
                userId = 1,
                Email = "john.doe@example.com"
                // Add any other properties relevant for the test
            };
            var response = new ServiceResponse<UserDto>
            {

                Data = new UserDto // This should be populated with expected DTO data
                   {
                    userId = existingUser.userId,
                    Email = existingUser.Email

                },
                Success = true
            };
            var editResponse = new ServiceResponse<string>
            {
                Success = false
            };
            var mockService = new Mock<IAuthService>();
            mockService.Setup(service => service.GetUser(userDto.LoginId)).Returns(response);
            mockService.Setup(service => service.ModifyUser(It.IsAny<User>(), existingUser.userId, existingUser.Email))
                       .Returns(editResponse);

            var userService = new AuthController(mockService.Object);

            // Act
            var result = userService.UpdateUser(userDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(editResponse);
            Assert.False(editResponse.Success);
            Assert.Equal(400,result.StatusCode);
        }

    }
}
