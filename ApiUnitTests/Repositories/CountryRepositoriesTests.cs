using APIPhoneBook.Data.Implementation;
using APIPhoneBook.Data;
using APIPhoneBook.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTests.Repositories
{
    public class CountryRepositoriesTests
    {
        [Fact]
        public void GetAll_ReturnsCountries_WhenCountriesExist()
        {
            var countriesList = new List<Country>
            {
              new Country{ CountryId=1, CountryName="Country 1"},
              new Country{ CountryId=2, CountryName="Country 2"},
             }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Country>>();
            // This line is setting up our fake database to act like a real one. When our program asks for all the categories,
            // our fake database will give it the list of categories we already set up. This helps us test our program's behavior
            // without needing a real database
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.GetEnumerator()).Returns(countriesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);
            var target = new CountryRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(countriesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.Countries, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.GetEnumerator(), Times.Once);
        }
        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoCountriesExist()
        {
            var countriesList = new List<Country>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Country>>();
            mockDbSet.As<IQueryable<Country>>().Setup(c => c.GetEnumerator()).Returns(countriesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);
            var target = new CountryRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(countriesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.Countries, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(c => c.GetEnumerator(), Times.Once);
        }

        [Fact]
        public void GetCountryById_ReturnsCountry_WhenCountryExists()
        {
            // Arrange
            var id = 1;
            var expectedCountry = new Country { CountryId = id, CountryName = "Country 1" };

            var countries = new List<Country>
    {
        expectedCountry,
        new Country { CountryId = 2, CountryName = "Country 2" },
    }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Country>>();
            var mockDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<Country>>().Setup(m => m.Provider).Returns(countries.Provider);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.Expression).Returns(countries.Expression);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.ElementType).Returns(countries.ElementType);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.GetEnumerator()).Returns(countries.GetEnumerator());

            mockDbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);

            var target = new CountryRepository(mockDbContext.Object);

            // Act
            var actual = target.GetCountryById(id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCountry.CountryId, actual.CountryId);
            Assert.Equal(expectedCountry.CountryName, actual.CountryName);
            mockDbSet.As<IQueryable<Country>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(m => m.Expression, Times.Once);
            mockDbContext.VerifyGet(c => c.Countries, Times.Once);
        }

        [Fact]
        public void GetCountryById_ReturnsNull_WhenCountryDoesNotExist()
        {
            // Arrange
            var id = 1;
            var countries = new List<Country>().AsQueryable(); // No countries in the list
            var mockDbSet = new Mock<DbSet<Country>>();
            var mockDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<Country>>().Setup(m => m.Provider).Returns(countries.Provider);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.Expression).Returns(countries.Expression);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.ElementType).Returns(countries.ElementType);
            mockDbSet.As<IQueryable<Country>>().Setup(m => m.GetEnumerator()).Returns(countries.GetEnumerator());

            mockDbContext.Setup(c => c.Countries).Returns(mockDbSet.Object);

            var target = new CountryRepository(mockDbContext.Object);

            // Act
            var actual = target.GetCountryById(id);

            // Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Country>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<Country>>().Verify(m => m.Expression, Times.Once);
            mockDbContext.VerifyGet(c => c.Countries, Times.Once);
        }
    }
}
