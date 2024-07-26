using APIPhoneBook.Data.Contract;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTests.Services
{
    public class CountryServiceTests
    {
        [Fact]
        [Trait("Countries", "GetAll")]
        public void GetCountry_ReturnsCountries_WhenCountriesExist()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { CountryId = 1, CountryName = "Country1"},
                new Country { CountryId = 2, CountryName = "Country2"}
            };
            var mockRepository = new Mock<ICountryRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(countries);
            var countryService = new CountryService(mockRepository.Object);
            // Act
            var actual = countryService.GetCountry();
            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(countries.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }
        [Fact]
        [Trait("Countries", "GetAll")]
        public void GetCountry_Returns_WhenNoCountriesExist()
        {
            // Arrange
            var countries = new List<Country>();
            var mockRepository = new Mock<ICountryRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(countries);
            var countryService = new CountryService(mockRepository.Object);
            // Act
            var actual = countryService.GetCountry();
            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetCountryById_ReturnsOk_WhenCountryExist()
        {
            // Arrange
            var categories = new Country()
            {
                CountryId = 1,
                CountryName = "Name 1",
            };

            var mockCategoryRepository = new Mock<ICountryRepository>();
            mockCategoryRepository.Setup(c => c.GetCountryById(1)).Returns(categories);

            var target = new CountryService(mockCategoryRepository.Object);

            // Act
            var actual = target.GetCountryById(1);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockCategoryRepository.Verify(c => c.GetCountryById(1), Times.Once);

        }
    }
}
