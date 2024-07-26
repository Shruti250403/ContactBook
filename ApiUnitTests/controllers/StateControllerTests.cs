using APIPhoneBook.Controllers;
using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTests.controllers
{
    public class StateControllerTests
    {
        [Fact]
        public void GetAllStates_ReturnsOkWithStates_WhenStateExists()
        {
            //Arrange
            var states = new List<State>
             {
            new State{StateId=1,StateName="State 1", CountryId= 1},
            new State{StateId=2,StateName="State 2", CountryId= 2},
            };
            var response = new ServiceResponse<IEnumerable<StateDto>>
            {
                Success = true,
                Data = states.Select(c => new StateDto { StateId = c.StateId, StateName = c.StateName, CountryId = c.CountryId }) // Convert to StateDto
            };
            var mockStateService = new Mock<IStateService>();
            var target = new StateController(mockStateService.Object);
            mockStateService.Setup(c => c.GetStates()).Returns(response);
            //Act
            var actual = target.GetStates() as OkObjectResult;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockStateService.Verify(c => c.GetStates(), Times.Once);
        }
        [Fact]
        public void GetAllStates_ReturnsNotFound_WhenNoStateExists()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<StateDto>>
            {
                Success = false,
                Data = new List<StateDto>()
            };
            var mockStateService = new Mock<IStateService>();
            var target = new StateController(mockStateService.Object);
            mockStateService.Setup(c => c.GetStates()).Returns(response);
            //Act
            var actual = target.GetStates() as BadRequestObjectResult;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            mockStateService.Verify(c => c.GetStates(), Times.Once);
        }
        [Fact]
        public void GetStateById_ReturnsOkWithState_WhenStateExists()
        {
            //Arrange
            int categoryId = 1;
            var state = new State { StateId = 1, CountryId = 1, StateName = "Category 1" };
            var response = new ServiceResponse<StateDto>
            {
                Success = true,
                Data = new StateDto { StateId = state.StateId, CountryId = state.CountryId, StateName = state.StateName }
            };

            var mockCategoryService = new Mock<IStateService>();
            var target = new StateController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetStateById(categoryId)).Returns(response);

            //Act
            var actual = target.GetStateById(categoryId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCategoryService.Verify(c => c.GetStateById(categoryId), Times.Once);
        }

        [Fact]
        public void GetStateById_ReturnsNotFound_WhenStateNotExists()
        {
            //Arrange
            int categoryId = 1;
            var response = new ServiceResponse<StateDto>
            {
                Success = false,
                Data = new StateDto()
            };

            var mockCategoryService = new Mock<IStateService>();
            var target = new StateController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetStateById(categoryId)).Returns(response);

            //Act
            var actual = target.GetStateById(categoryId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCategoryService.Verify(c => c.GetStateById(categoryId), Times.Once);
        }
        [Fact]
        public void GetAllStandardByCountryId_ReturnsOkWithState_WhenStateExists()
        {
            // Arrange
            int countryId = 1;
            var state = new State { StateId = 1, CountryId = 1, StateName = "State 1" };
            var response = new ServiceResponse<List<StateDto>>
            {
                Success = true,
                Data = new List<StateDto> { new StateDto { StateId = state.StateId, CountryId = state.CountryId, StateName = state.StateName } }
            };

            var mockStateService = new Mock<IStateService>();
            var target = new StateController(mockStateService.Object);
            mockStateService.Setup(s => s.GetAllStateByCountryId(countryId)).Returns(response);

            // Act
            var actual = target.GetAllStandardByCountryId(countryId) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockStateService.Verify(s => s.GetAllStateByCountryId(countryId), Times.Once);
        }
        [Fact]
        public void GetAllStandardByCountryId_ReturnsNotFound_WhenStateNotExists()
        {
            // Arrange
            int countryId = 1;
            var response = new ServiceResponse<List<StateDto>>
            {
                Success = false,
                Data = null
            };

            var mockStateService = new Mock<IStateService>();
            var target = new StateController(mockStateService.Object);
            mockStateService.Setup(s => s.GetAllStateByCountryId(countryId)).Returns(response);

            // Act
            var actual = target.GetAllStandardByCountryId(countryId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockStateService.Verify(s => s.GetAllStateByCountryId(countryId), Times.Once);
        }
    }
}
