using APIPhoneBook.Controllers;
using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Contract;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTests.controllers
{
    public class ContactControllerTests
    {
        [Fact]
        public void GetAllContacts_ReturnsOkWithContacts()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    };

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetContact()).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllContacts() as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(), Times.Once);
        }


        [Fact]
        public void GetAllContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    };

            var letter = 'a';

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetContact()).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllContacts() as OkObjectResult; // No need to pass the letter parameter

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(), Times.Once);
        }


        [Fact]
        public void GetAllContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<ContactModel>();

            var letter = 'a';

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetContact()).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllContacts() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            mockContactService.Verify(c => c.GetContact(), Times.Once);
        }


        [Fact]
        public void GetAllContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<ContactModel>(); // Empty list

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = null, // Setting data to null when no contacts are found
                Message = "No contacts found" // Add a message indicating no contacts found
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact()).Returns(response);

            // Act
            var actual = target.GetAllContacts() as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            mockContactService.Verify(c => c.GetContact(), Times.Once);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    };

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, It.IsAny<char?>())).Returns(response); // Setup with any value for letter

            // Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }


        [Fact]
        public void GetAllFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
             {
            new ContactModel{ContactId=1,FirstName="Contact 1", Phone= "1234567890"},
            new ContactModel{ContactId=2,FirstName="Contact 2", Phone= "1234567899"},
            };

            var letter = 'a';
            int page = 1;
            int pageSize = 2;

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter)).Returns(response);

            //Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }
        [Fact]
        public void GetAllFavouriteContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
             {
            new ContactModel{ContactId=1,FirstName="Contact 1", Phone= "1234567890"},
            new ContactModel{ContactId=2,FirstName="Contact 2", Phone= "1234567899"},
            };

            var letter = 'a';
            int page = 1;
            int pageSize = 2;


            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter)).Returns(response);

            //Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
             {
            new ContactModel{ContactId=1,FirstName="Contact 1", Phone= "1234567890"},
            new ContactModel{ContactId=2,FirstName="Contact 2", Phone= "1234567899"},
            };
            var letter = 'a';
            int page = 1;
            int pageSize = 2;

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter)).Returns(response);

            //Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

    //    [Fact]
    //    public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull()
    //    {
    //        // Arrange
    //        var contacts = new List<ContactModel>
    //{
    //    new ContactModel{ContactId=1, FirstName="Contact 1", Phone= "1234567890"},
    //    new ContactModel{ContactId=2, FirstName="Contact 2", Phone= "1234567899"},
    //};

    //        int page = 1;
    //        int pageSize = 2;
    //        string searchQuery = "search";
    //        string sortOrder = "asc";

    //        var response = new ServiceResponse<IEnumerable<ContactDto>>
    //        {
    //            Success = true,
    //            Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        var target = new ContactController(mockContactService.Object);
    //        mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null,sortOrder, searchQuery)).Returns(response);

    //        // Act
    //        var actual = target.GetPaginatedContacts(null, page, pageSize, searchQuery) as OkObjectResult;

    //        // Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(200, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null,sortOrder, searchQuery), Times.Once);
    //    }


    //    [Fact]
    //    public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
    //    {
    //        // Arrange
    //        var contacts = new List<ContactModel>
    //{
    //    new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
    //    new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    //};

    //        var letter = 'd';
    //        int page = 1;
    //        int pageSize = 2;
    //        string searchQuery = "search";
    //        string sortOrder = "asc";

    //        var response = new ServiceResponse<IEnumerable<ContactDto>>
    //        {
    //            Success = true,
    //            Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter,sortOrder, searchQuery)).Returns(response);

    //        var target = new ContactController(mockContactService.Object);

    //        // Act
    //        var actual = target.GetPaginatedContacts(letter, page, pageSize, sortOrder, searchQuery) as OkObjectResult;

    //        // Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(200, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter,sortOrder, searchQuery), Times.Once);
    //    }


    //    [Fact]
    //    public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNull()
    //    {
    //        // Arrange
    //        var contacts = new List<ContactModel>
    //{
    //    new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
    //    new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    //};

    //        int page = 1;
    //        int pageSize = 2;
    //        char? letter = null;  // Ensure letter is null
    //        string searchQuery = "search";
    //        string sortOrder = "asc";

    //        var response = new ServiceResponse<IEnumerable<ContactDto>>
    //        {
    //            Success = false,
    //            Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter,sortOrder, searchQuery)).Returns(response);

    //        var target = new ContactController(mockContactService.Object);

    //        // Act
    //        var actual = target.GetPaginatedContacts(letter, page, pageSize,sortOrder, searchQuery) as NotFoundObjectResult;

    //        // Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(404, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, sortOrder, searchQuery), Times.Once);
    //    }


    //    [Fact]
    //    public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull()
    //    {
    //        // Arrange
    //        var contacts = new List<ContactModel>
    //{
    //    new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
    //    new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    //};

    //        var letter = 'd';
    //        int page = 1;
    //        int pageSize = 2;
    //        string searchQuery = "search";
    //        string sortOrder = "asc";

    //        var response = new ServiceResponse<IEnumerable<ContactDto>>
    //        {
    //            Success = false,
    //            Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter,sortOrder, searchQuery)).Returns(response);

    //        var target = new ContactController(mockContactService.Object);

    //        // Act
    //        var actual = target.GetPaginatedContacts(letter, page, pageSize,sortOrder, searchQuery) as NotFoundObjectResult;

    //        // Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(404, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter,sortOrder, searchQuery), Times.Once);
        //}


        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null; // Ensure letter is null

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null;  // Ensure letter is null

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }


        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone= "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone= "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetFavouriteContacts(page, pageSize, letter)).Returns(response);

            //Act
            var actual = target.GetFavouriteContacts(letter, page, pageSize) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

    //    [Fact]
    //    public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNull()
    //    {
    //        //Arrange
    //        var contacts = new List<ContactModel>
    //{
    //    new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
    //    new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    //};

    //        var response = new ServiceResponse<int>
    //        {
    //            Success = true,
    //            Data = contacts.Count
    //        };

    //        var mockContactService = new Mock<IContactService>();
    //        var target = new ContactController(mockContactService.Object);
    //        mockContactService.Setup(c => c.TotalContacts(null,search)).Returns(response);

    //        //Act
    //        var actual = target.GetTotalCountOfContacts(null) as OkObjectResult;

    //        //Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(200, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        Assert.Equal(2, response.Data);
    //        mockContactService.Verify(c => c.TotalContacts(null), Times.Once);
    //    }

    //    [Fact]
    //    public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
    //    {
    //        //Arrange
    //        var contacts = new List<ContactModel>
    //{
    //    new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
    //    new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    //};

    //        var response = new ServiceResponse<int>
    //        {
    //            Success = true,
    //            Data = contacts.Count
    //        };

    //        var letter = 'd';
    //        var mockContactService = new Mock<IContactService>();
    //        var target = new ContactController(mockContactService.Object);
    //        mockContactService.Setup(c => c.TotalContacts(letter)).Returns(response);

    //        //Act
    //        var actual = target.GetTotalCountOfContacts(letter) as OkObjectResult;

    //        //Assert
    //        Assert.NotNull(actual);
    //        Assert.Equal(200, actual.StatusCode);
    //        Assert.NotNull(actual.Value);
    //        Assert.Equal(response, actual.Value);
    //        Assert.Equal(2, response.Data);
    //        mockContactService.Verify(c => c.TotalContacts(letter), Times.Once);
    //    }


        //[Fact]
        //public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNotNull()
        //{
        //    var response = new ServiceResponse<int>
        //    {
        //        Success = false,
        //        Data = 0
        //    };

        //    var letter = 'd';
        //    var mockContactService = new Mock<IContactService>();
        //    var target = new ContactController(mockContactService.Object);
        //    mockContactService.Setup(c => c.TotalContacts(letter)).Returns(response);

        //    //Act
        //    var actual = target.GetTotalCountOfContacts(letter) as NotFoundObjectResult;

        //    //Assert
        //    Assert.NotNull(actual);
        //    Assert.Equal(404, actual.StatusCode);
        //    Assert.NotNull(actual.Value);
        //    Assert.Equal(response, actual.Value);
        //    Assert.Equal(0, response.Data);
        //    mockContactService.Verify(c => c.TotalContacts(letter), Times.Once);
        //}


        //[Fact]
        //public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNull()
        //{
        //    var response = new ServiceResponse<int>
        //    {
        //        Success = false,
        //        Data = 0
        //    };

        //    var mockContactService = new Mock<IContactService>();
        //    var target = new ContactController(mockContactService.Object);
        //    mockContactService.Setup(c => c.TotalContacts(null)).Returns(response);

        //    //Act
        //    var actual = target.GetTotalCountOfContacts(null) as NotFoundObjectResult;

        //    //Assert
        //    Assert.NotNull(actual);
        //    Assert.Equal(404, actual.StatusCode);
        //    Assert.NotNull(actual.Value);
        //    Assert.Equal(response, actual.Value);
        //    Assert.Equal(0, response.Data);
        //    mockContactService.Verify(c => c.TotalContacts(null), Times.Once);
        //}


        [Fact]
        public void GetTotalCountOfFavouriteContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalFavContacts(null)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfFavContacts(null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalFavContacts(null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfFavContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            // Arrange
            var letter = 'd';
            var totalCount = 10; // For example, total count of favorite contacts

            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = totalCount
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalFavContacts(letter)).Returns(response);

            // Act
            var actual = target.GetTotalCountOfFavContacts(letter) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(totalCount, response.Data); // Ensure data in the response is as expected
            mockContactService.Verify(c => c.TotalFavContacts(letter), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfFavouriteContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalFavContacts(letter)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfFavContacts(letter) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalFavContacts(letter), Times.Once);
        }


        [Fact]

        public void GetContactById_ReturnsOk()
        {

            var ContactId = 1;
            var contact = new ContactModel
            {

                ContactId = ContactId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<ContactDto>
            {
                Success = true,
                Data = new ContactDto
                {
                    ContactId = ContactId,
                    FirstName = contact.FirstName
                }
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact(ContactId)).Returns(response);

            //Act
            var actual = target.GetContactById(ContactId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(ContactId), Times.Once);
        }

        [Fact]

        public void GetContactById_ReturnsNotFound()
        {

            var ContactId = 1;
            var contact = new ContactModel
            {

                ContactId = ContactId,
                FirstName = "Contact 1"
            };

            var response = new ServiceResponse<ContactDto>
            {
                Success = false,
                Data = new ContactDto
                {
                    ContactId = ContactId,
                    FirstName = contact.FirstName
                }
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact(ContactId)).Returns(response);

            //Act
            var actual = target.GetContactById(ContactId) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(ContactId), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsOk_WhenContactIsAddedSuccessfully()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<ContactModel>())).Returns(response);

            //Act

            var actual = target.AddContact(addContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<ContactModel>()), Times.Once);

        }

        [Fact]
        public void AddContact_ReturnsBadRequest_WhenContactIsNotAdded()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<ContactModel>())).Returns(response);

            //Act

            var actual = target.AddContact(addContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<ContactModel>()), Times.Once);

        }
        [Fact]
        public void AddContact_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidContactDto = new AddContactDto(); // This will be invalid as it has no data

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            target.ModelState.AddModelError("FirstName", "FirstName is required"); // Adding ModelState error manually

            // Act
            var actual = target.AddContact(invalidContactDto) as BadRequestObjectResult;

            // Assert
            mockContactService.Verify(c => c.AddContact(It.IsAny<ContactModel>()), Times.Never); // Verify that the service method was not called
        }

        [Fact]
        public void UpdateContact_ReturnsOk_WhenContactIsUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.ModifyContact(It.IsAny<ContactModel>())).Returns(response);

            //Act

            var actual = target.UpdateContact(updateContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(It.IsAny<ContactModel>()), Times.Once);

        }

        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenContactIsNotUpdated()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.ModifyContact(It.IsAny<ContactModel>())).Returns(response);

            //Act

            var actual = target.UpdateContact(updateContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(It.IsAny<ContactModel>()), Times.Once);

        }

        [Fact]
        public void RemoveContact_ReturnsOkResponse_WhenContactDeletedSuccessfully()
        {

            var ContactId = 1;
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveContact(ContactId)).Returns(response);

            //Act

            var actual = target.RemoveContact(ContactId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(ContactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_ReturnsBadRequest_WhenContactNotDeleted()
        {

            var ContactId = 1;
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveContact(ContactId)).Returns(response);

            //Act

            var actual = target.RemoveContact(ContactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(ContactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_ReturnsBadRequest_WhenContactIsLessThanZero()
        {

            var ContactId = 0;

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);

            //Act

            var actual = target.RemoveContact(ContactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal("Please enter proper data.", actual.Value);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null, sortOrder,null)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(null, null, page, pageSize,sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, sortOrder,null), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "tac";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null,sortOrder,search)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(null, search, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, sortOrder, search), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter,sortOrder,null)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(letter, null, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, sortOrder, null), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsOkWithContacts_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter,sortOrder,search)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(letter, search, page, pageSize, sortOrder) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, sortOrder, search), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null, sortOrder, null)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(null, null, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, sortOrder, null), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
             };

            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, null, sortOrder, search)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(null, search, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, null, sortOrder, search), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, sortOrder, null)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(letter, null, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter, sortOrder,null), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            string search = "dev";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, letter, sortOrder, search)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(letter, search, page, pageSize, sortOrder) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, letter,sortOrder, search), Times.Once);
        }
        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
             {
            new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
            new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, null)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(null, null) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, null), Times.Once);
        }
        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
             {
            new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
            new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };
            string search = "dev";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, search)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(null, search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNotNull_SearchIsNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
             {
            new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
            new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, null)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(letter, null) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, null), Times.Once);
        }
        [Fact]
        public void GetTotalCountOfContacts_ReturnsOkWithContacts_WhenLetterIsNotNull_SearchIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
             {
            new ContactModel{ContactId=1,FirstName="Contact 1", Phone = "1234567890"},
            new ContactModel{ContactId=2,FirstName="Contact 2", Phone = "1234567899"},
            };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };
            string search = "dev";
            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, search)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(letter, search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, null)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(letter, null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNotNull_SearchIsNotNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'd';
            string search = "dev";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, search)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(letter, search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, search), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNull_SearchIsNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, null)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(null, null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, null), Times.Once);
        }

        [Fact]
        public void GetTotalCountOfContacts_ReturnsNotFound_WhenLetterIsNull_SearchIsNotNull()
        {



            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };
            string search = "dev";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, search)).Returns(response);

            //Act
            var actual = target.GetTotalCountOfContacts(null, search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, search), Times.Once);
        }
        [Fact]
        public void GetFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null; // Ensure letter is null
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllFavouriteContacts(letter) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(letter), Times.Once);
        }

        [Fact]
        public void GetFavouriteContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone = "1234567899" }
    };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllFavouriteContacts(letter) as OkObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(letter), Times.Once);
        }

        [Fact]
        public void GetFavouriteContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel { ContactId = 1, FirstName = "Contact 1", Phone = "1234567890" },
        new ContactModel { ContactId = 2, FirstName = "Contact 2", Phone= "1234567899" }
    };

            int page = 1;
            int pageSize = 2;
            char? letter = null;  // Ensure letter is null
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone })
            };

            var mockContactService = new Mock<IContactService>();
            mockContactService.Setup(c => c.GetFavouriteContacts(letter)).Returns(response);

            var target = new ContactController(mockContactService.Object);

            // Act
            var actual = target.GetAllFavouriteContacts(letter) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(letter), Times.Once);
        }


        [Fact]
        public void GetFavouriteContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<ContactModel>
            {
               new ContactModel{ContactId=1,FirstName="Contact 1", Phone= "1234567890"},
                 new ContactModel{ContactId=2,FirstName="Contact 2", Phone= "1234567899"},
             };

            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, Phone = c.Phone }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetFavouriteContacts(letter)).Returns(response);

            //Act
            var actual = target.GetAllFavouriteContacts(letter) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetFavouriteContacts(letter), Times.Once);
        }

    }
}
