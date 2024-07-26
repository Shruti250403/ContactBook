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
    public class CountryControllerTests
    {
        [Fact]
        public void GetAllCountries_ReturnsOkWithCountries_WhenCountryExists()
        {
            //Arrange
            var countries = new List<Country>
             {
            new Country{CountryId=1,CountryName="Country 1"},
            new Country{CountryId=2,CountryName="Country 2"},
            };
            var response = new ServiceResponse<IEnumerable<CountryDto>>
            {
                Success = true,
                Data = countries.Select(c => new CountryDto { CountryId = c.CountryId, CountryName = c.CountryName }) // Convert to CountryDto
            };
            var mockCountryService = new Mock<ICountryService>();
            var target = new CountryController(mockCountryService.Object);
            mockCountryService.Setup(c => c.GetCountry()).Returns(response);
            //Act
            var actual = target.GetAll() as OkObjectResult;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCountryService.Verify(c => c.GetCountry(), Times.Once);
        }
        [Fact]
        public void GetAllCountries_ReturnsNotFound_WhenNoCountryExists()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<CountryDto>>
            {
                Success = false,
                Data = new List<CountryDto>()
            };
            var mockCountryService = new Mock<ICountryService>();
            var target = new CountryController(mockCountryService.Object);
            mockCountryService.Setup(c => c.GetCountry()).Returns(response);
            //Act
            var actual = target.GetAll() as NotFoundObjectResult;
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            mockCountryService.Verify(c => c.GetCountry(), Times.Once);
        }
        [Fact]
        public void GetCountryById_ReturnsOkWithCategory_WhenCountryExists()
        {
            //Arrange
            int categoryId = 1;
            var category = new Country { CountryId = 1, CountryName = "Category 1" };
            var response = new ServiceResponse<CountryDto>
            {
                Success = true,
                Data = new CountryDto { CountryId = category.CountryId, CountryName = category.CountryName }
            };

            var mockCategoryService = new Mock<ICountryService>();
            var target = new CountryController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetCountryById(categoryId)).Returns(response);

            //Act
            var actual = target.GetCountryById(categoryId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCategoryService.Verify(c => c.GetCountryById(categoryId), Times.Once);
        }

        [Fact]
        public void GetCountryById_ReturnsNotFound_WhenCountryNotExists()
        {
            //Arrange
            int categoryId = 1;
            var response = new ServiceResponse<CountryDto>
            {
                Success = false,
                Data = new CountryDto()
            };

            var mockCategoryService = new Mock<ICountryService>();
            var target = new CountryController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetCountryById(categoryId)).Returns(response);

            //Act
            var actual = target.GetCountryById(categoryId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            mockCategoryService.Verify(c => c.GetCountryById(categoryId), Times.Once);
        }
    }
}
