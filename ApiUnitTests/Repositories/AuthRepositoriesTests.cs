using APIPhoneBook.Data.Implementation;
using APIPhoneBook.Data;
using APIPhoneBook.Models;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTests.Repositories
{
    public class AuthRepositoriesTests
    {
        [Fact]
        public void RegisterUser_ReturnTrue()
        {
            //Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            mockAbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.RegisterUser(user);
            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(user), Times.Once);
            mockAbContext.Verify(c => c.SaveChanges(), Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void RegisterUser_ReturnFalse()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.RegisterUser(null);
            //Assert
            Assert.False(actual);
        }
        [Fact]
        public void ValidateUser_ReturnTrue()
        {
            //Arrange
            var users = new List<User>
            {
                new User
            {
                userId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                },
                new User
            {
                userId = 2,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid1",
                },
            }.AsQueryable();
            var Username = "loginid";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.ValidateUser(Username);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void ValidateUser_WhenUsersIsNull()
        {
            //Arrange
            var users = new List<User>().AsQueryable();
            var Username = "loginid";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.ValidateUser(Username);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void UserExist_WhenUsersIsNull()
        {
            //Arrange
            var users = new List<User>().AsQueryable();
            var loginId = "loginid";
            var email = "abc@gmail.com";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.UserExists(loginId, email);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void UserExist_WhenUsersIsThere()
        {
            //Arrange
            var users = new List<User>
            {
                new User
            {
                userId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                },
                new User
            {
                userId = 2,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid1",
                },
            }.AsQueryable();
            var loginId = "loginid";
            var email = "abc@gmail.com";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.UserExists(loginId, email);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new AuthRepository(mockAppDbContext.Object);
            var user = new User
            {
                userId = 1
            };
            //Act
            var actual = target.UpdateUser(user);
            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(user), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateUser_ReturnFalse()
        {
            //Arrange
            var mockAppDbContext = new Mock<IAppDbContext>();
            var target = new AuthRepository(mockAppDbContext.Object);
            //Act
            var actual = target.UpdateUser(null);
            //Assert
            Assert.False(actual);

        }
        //GetUserDetailById
        [Fact]
        public void GetUserDetailById_WhenContactIsNull()
        {
            //Arrange
            var id = "loginid";
            var user = new List<User>().AsQueryable();
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(user.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(user.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.GetUser(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);

        }

        [Fact]
        public void GetUserDetailById_WhenContactIsNotNull()
        {
            //Arrange
            var id = "loginid";
            var users = new List<User>()
            {
              new User { userId = 1, FirstName = "Firstname 1" },
                new User { userId = 2, FirstName = "Firstname 2" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);
            //Act
            var actual = target.GetUser(id);
            //Assert
            //Assert.NotNull(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);

        }
        [Fact]
        public void UserExists_WhenUsersIsNull()
        {
            //Arrange
            var users = new List<User>().AsQueryable();
            var userId = 1;
            var loginId = "loginid";
            var email = "abc@gmail.com";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.UserExist(userId, loginId, email);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void UserExists_WhenUsersIsThere()
        {
            //Arrange
            var users = new List<User>
            {
                new User
            {
                userId = 1,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",

                },
                new User
            {
                userId = 2,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid1",

                },
            }.AsQueryable();
            var userId = 1;
            var loginId = "loginid";
            var email = "email@example.com";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.UserExist(userId, loginId, email);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
    }
}
