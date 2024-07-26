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
    public class StateServiceTests
    {
        [Fact]
        public void GetStates_ReturnsOk_WhenStatesExist()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var states = new List<State>
            {
                new State { StateId = 1, StateName = "State1", CountryId = 1, Country = new Country{ CountryId=1, CountryName="India" } },
                new State { StateId = 2, StateName = "State2", CountryId = 2, Country = new Country{ CountryId=1, CountryName="India" } }
            };
            mockStateRepository.Setup(c => c.GetAll()).Returns(states);
            var target = new StateService(mockStateRepository.Object);
            // Act
            var actual = target.GetStates();
            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal(states.Count, actual.Data.Count());
            mockStateRepository.Verify(c => c.GetAll(), Times.Once);
        }
        [Fact]
        public void GetStates_ReturnsNotFound_WhenNoStatesExist()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            mockStateRepository.Setup(c => c.GetAll()).Returns((List<State>)(null));
            var target = new StateService(mockStateRepository.Object);
            // Act
            var actual = target.GetStates();
            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("No record found!", actual.Message);
            mockStateRepository.Verify(c => c.GetAll(), Times.Once);
        }
        [Fact]
        public void GetStateById_ReturnsOk_WhenStateExist()
        {
            // Arrange
            var categories = new State()
            {
                StateId = 1,
                CountryId = 1,
                StateName = "Name 1",
                Country = new Country()
                {
                    CountryId = 1,
                    CountryName = "Name1"
                }
            };

            var mockCategoryRepository = new Mock<IStateRepository>();
            mockCategoryRepository.Setup(c => c.GetStateById(1)).Returns(categories);

            var target = new StateService(mockCategoryRepository.Object);

            // Act
            var actual = target.GetStateById(1);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            mockCategoryRepository.Verify(c => c.GetStateById(1), Times.Once);

        }
        [Fact]
        public void GetAllStateByCountryId_ReturnsOk_WhenStateExist()
        {
            var states = new List<State>
    {
        new State
        {
            StateId = 1,
            StateName = "Name 1",
            CountryId = 1
        }
    };

            var mockStateRepository = new Mock<IStateRepository>();
            mockStateRepository.Setup(repo => repo.GetAllStateByCountryId(1)).Returns(states);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetAllStateByCountryId(1);

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Single(actual.Data);
            Assert.Equal(1, actual.Data.First().StateId);
            Assert.Equal("Name 1", actual.Data.First().StateName);
            Assert.Equal(1, actual.Data.First().CountryId);

            mockStateRepository.Verify(repo => repo.GetAllStateByCountryId(1), Times.Once);
        }
        [Fact]
        public void GetAllStateByCountryId_ReturnsOk_WhenStateNotExist()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            mockStateRepository.Setup(repo => repo.GetAllStateByCountryId(It.IsAny<int>())).Returns((List<State>)null);

            var target = new StateService(mockStateRepository.Object);

            // Act
            var actual = target.GetAllStateByCountryId(1);

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);

            mockStateRepository.Verify(repo => repo.GetAllStateByCountryId(1), Times.Once);
        }
    }
}
