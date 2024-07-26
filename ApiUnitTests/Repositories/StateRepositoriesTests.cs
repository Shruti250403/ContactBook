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
    public class StateRepositoriesTests
    {
        [Fact]
        public void GetAll_ReturnsStates_WhenStatesExist()
        {
            var statesList = new List<State>
            {
              new State{ StateId=1, StateName="State 1", CountryId = 1 },
              new State{ StateId=2, StateName="State 2", CountryId = 1},
             }.AsQueryable();
            var mockDbSet = new Mock<DbSet<State>>();

            mockDbSet.As<IQueryable<State>>().Setup(c => c.GetEnumerator()).Returns(statesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(statesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.States, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.GetEnumerator(), Times.Once);
        }
        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoStatesExist()
        {
            var statesList = new List<State>().AsQueryable();
            var mockDbSet = new Mock<DbSet<State>>();
            mockDbSet.As<IQueryable<State>>().Setup(c => c.GetEnumerator()).Returns(statesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(statesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.States, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.GetEnumerator(), Times.Once);
        }
        [Fact]
        public void GetStateById_ReturnsState_WhenStateExists()
        {
            // Arrange
            var id = 1;
            var expectedState = new State { StateId = id, CountryId = 1, StateName = "State 1", Country = new Country { CountryId = 1, CountryName = "Country 1" } };

            var states = new List<State> { expectedState }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();
            var mockDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(states.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(states.Expression);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(states.ElementType);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(states.GetEnumerator());

            mockDbContext.Setup(c => c.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockDbContext.Object);

            // Act
            var actual = target.GetStateById(id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedState.Country.CountryId, actual.Country.CountryId);
            Assert.Equal(expectedState.Country.CountryName, actual.Country.CountryName);
            //mockDbSet.As<IQueryable<State>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(m => m.Expression, Times.Once);
            mockDbContext.VerifyGet(c => c.States, Times.Once);
        }

        [Fact]
        public void GetStateById_ReturnsNull_WhenStateDoesNotExist()
        {
            // Arrange
            var id = 1;
            var states = new List<State>().AsQueryable(); // No states in the list

            var mockDbSet = new Mock<DbSet<State>>();
            var mockDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(states.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(states.Expression);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(states.ElementType);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(states.GetEnumerator());

            mockDbContext.Setup(c => c.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockDbContext.Object);

            // Act
            var actual = target.GetStateById(id);

            // Assert
            Assert.Null(actual);
            //mockDbSet.As<IQueryable<State>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(m => m.Expression, Times.Once);
            mockDbContext.VerifyGet(c => c.States, Times.Once);
        }
        [Fact]
        public void GetAllStateByCountryId_ReturnsStates_WhenStatesExist()
        {
            // Arrange
            var countryId = 1;
            var expectedStates = new List<State>
    {
        new State { StateId = 1, CountryId = countryId, StateName = "State 1", Country = new Country { CountryId = countryId, CountryName = "Country 1" } },
        new State { StateId = 2, CountryId = countryId, StateName = "State 2", Country = new Country { CountryId = countryId, CountryName = "Country 1" } }
    };

            var states = expectedStates.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();
            var mockDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(states.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(states.Expression);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(states.ElementType);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(states.GetEnumerator());

            mockDbContext.Setup(c => c.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockDbContext.Object);

            // Act
            var actual = target.GetAllStateByCountryId(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedStates.Count, actual.Count);
            for (int i = 0; i < expectedStates.Count; i++)
            {
                Assert.Equal(expectedStates[i].StateId, actual[i].StateId);
                Assert.Equal(expectedStates[i].StateName, actual[i].StateName);
                Assert.Equal(expectedStates[i].CountryId, actual[i].CountryId);
                Assert.NotNull(actual[i].Country);
                Assert.Equal(expectedStates[i].Country.CountryId, actual[i].Country.CountryId);
                Assert.Equal(expectedStates[i].Country.CountryName, actual[i].Country.CountryName);
            }
            //mockDbSet.As<IQueryable<State>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(m => m.Expression, Times.Once);
            mockDbContext.VerifyGet(c => c.States, Times.Once);
        }

        [Fact]
        public void GetAllStateByCountryId_ReturnsEmptyList_WhenStatesDoNotExist()
        {
            // Arrange
            var countryId = 1;
            var states = new List<State>().AsQueryable(); // No states in the list

            var mockDbSet = new Mock<DbSet<State>>();
            var mockDbContext = new Mock<IAppDbContext>();

            mockDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(states.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(states.Expression);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.ElementType).Returns(states.ElementType);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.GetEnumerator()).Returns(states.GetEnumerator());

            mockDbContext.Setup(c => c.States).Returns(mockDbSet.Object);

            var target = new StateRepository(mockDbContext.Object);

            // Act
            var actual = target.GetAllStateByCountryId(countryId);

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            //mockDbSet.As<IQueryable<State>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(m => m.Expression, Times.Once);
            mockDbContext.VerifyGet(c => c.States, Times.Once);
        }
    }
}
