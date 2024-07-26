using APIPhoneBook.Data.Contract;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Implementation;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTests.Services
{
    public class ContactServiceTests
    {
        [Fact]

        public void GetContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactModel
            {
               ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetContacts_Returns_WhenNoContactsExistAndLetterIsNull()
        {
            // Arrange
            var contacts = new List<ContactModel>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]

        public void GetContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            // Arrange
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetContacts_Returns_WhenNoContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';

            // Arrange
            var contacts = new List<ContactModel>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]

        public void GetFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            char? letter = null;
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
               ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactModel
            {
               ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        [Fact]
        public void GetFavouriteContacts_Returns_WhenNoContactsExistAndLetterIsNull()
        {
            // Arrange
            char? letter = null;
            int page = 1;
            string sortOrder = "asc";
            int pageSize = 2;
            var contacts = new List<ContactModel>();
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        [Fact]

        public void GetFavouriteContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            int page = 1;
            int pageSize = 2;

            string sortOrder = "asc";
            // Arrange
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }
        [Fact]
        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenNoFavoriteContactsFound()
        {
            // Arrange
            var contacts = new List<ContactModel>
    {
        new ContactModel
        {
            ContactId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            Phone = "1234567890",
            Company = "company",
            Image = "file1.png",
            Gender = "Male",
            Favourites = false, // Ensure none of the contacts are favorites
            CountryId = 1,
            StateId = 1,
            Country = new Country
            {
                CountryId = 1,
                CountryName = "USA"
            },
            State = new State
            {
                StateId = 1,
                StateName = "California"
            }
        },
        new ContactModel
        {
            ContactId = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            Phone = "9876543210",
            Company = "company",
            Image = "file2.png",
            Gender = "Female",
            Favourites = false, // Ensure none of the contacts are favorites
            CountryId = 1,
            StateId = 1,
            Country = new Country
            {
                CountryId = 2,
                CountryName = "Canada"
            },
            State = new State
            {
                StateId = 2,
                StateName = "Ontario"
            }
        }
    };
            var letter = 'd';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns(contacts);
            var contactService = new ContactService(mockRepository.Object);
            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);
            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        [Fact]
        public void GetFavouriteContacts_Returns_WhenNoContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            // Arrange
            var contacts = new List<ContactModel>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        //[Fact]
        //[ExcludeFromCodeCoverage]
        //public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        //{

        //    // Arrange
        //    var contacts = new List<ContactModel>
        //{
        //    new ContactModel
        //    {
        //       ContactId = 1,
        //        FirstName = "John",
        //        LastName = "Doe",
        //        Email = "john@example.com",
        //        Phone = "1234567890",
        //        Company = "company",
        //        Image = "file1.png",
        //        Gender = "Male",
        //        Favourites = true,
        //        CountryId = 1,
        //        StateId = 1,
        //        Country = new Country
        //        {
        //            CountryId = 1,
        //            CountryName = "USA"
        //        },
        //        State = new State
        //        {
        //            StateId = 1,
        //            StateName = "California"
        //        }
        //    },
        //    new ContactModel
        //    {
        //       ContactId = 1,
        //        FirstName = "John",
        //        LastName = "Doe",
        //        Email = "john@example.com",
        //        Phone = "1234567890",
        //        Company = "company",
        //        Image = "file1.png",
        //        Gender = "Male",
        //        Favourites = true,
        //        CountryId = 1,
        //        StateId = 1,
        //        Country = new Country
        //        {
        //            CountryId = 2,
        //            CountryName = "Canada"
        //        },
        //        State = new State
        //        {
        //            StateId = 2,
        //            StateName = "Ontario"
        //        }
        //    }
        //};
        //    int page = 1;
        //    int pageSize = 2;
        //    string searchQuery = "search";
        //    string sortOrder = "asc";
        //    var letter = 'd';

        //    var mockRepository = new Mock<IContactRepository>();
        //    mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery)).Returns(contacts);

        //    var contactService = new ContactService(mockRepository.Object);

        //    // Act
        //    var actual = contactService.GetPaginatedContacts(page, pageSize, letter, sortOrder);

        //    // Assert
        //    Assert.False(actual.Success);
        //    Assert.Null(actual.Data);
        //    Assert.Equal(contacts.Count(), actual.Data.Count());
        //    mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery), Times.Once);
        //}
        [Fact]

        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";
            var letter = 'd';

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns<IEnumerable<ContactModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }

        [Fact]

        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
               ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactModel
            {
               ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";
            var letter = 'd';

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }
        [Fact]

        public void GetPaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string searchQuery = "search";
            string sortOrder = "asc";
            var letter = 'd';
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder)).Returns<IEnumerable<ContactModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, letter, searchQuery, sortOrder), Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNull()
        {
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactModel
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts()).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts();

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(), Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsContacts_WhenLetterIsNotNull()
        {
            var letter = 'c';
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactModel
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts()).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts();

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(), Times.Once);
        }
        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            string sortOrder = "asc";

            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            int page = 1;
            int pageSize = 2;
            var letter = 'd';
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }
        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var letter = 'd';
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns<IEnumerable<ContactModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };
            var letter = 'x';
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }
        [Fact]

        public void GetFavouritePaginatedContacts_ReturnsNoRecord_WhenContactsExistAndLetterIsNotNull()
        {

            // Arrange
            int page = 1;
            int pageSize = 2;
            string sortOrder = "asc";
            var letter = 'x';
            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetFavouriteContacts(page, pageSize, letter)).Returns<IEnumerable<ContactModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No favourite contacts found!", actual.Message);
            mockRepository.Verify(r => r.GetFavouriteContacts(page, pageSize, letter), Times.Once);
        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsContacts_WhenLetterIsNull()
        {
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactModel
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalFavContacts(null)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalFavContacts(null);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalFavContacts(null), Times.Once);
        }

        [Fact]
        public void TotalFavouriteContacts_ReturnsContacts_WhenLetterIsNotNull()
        {
            var letter = 'c';
            var contacts = new List<ContactModel>
        {
            new ContactModel
            {
                ContactId = 1,
                FirstName = "John"

            },
            new ContactModel
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalFavContacts(letter)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalFavContacts(letter);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalFavContacts(letter), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsContact_WhenContactExist()
        {
            // Arrange
            var ContactId = 1;
            var contact = new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }

            };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(ContactId)).Returns(contact);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(ContactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            mockRepository.Verify(r => r.GetContact(ContactId), Times.Once);
        }

        [Fact]
        public void GetContact_ReturnsNoRecord_WhenNoContactsExist()
        {
            // Arrange
            var ContactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(ContactId)).Returns<IEnumerable<ContactModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(ContactId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal("No records found.", actual.Message);
            mockRepository.Verify(r => r.GetContact(ContactId), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsContactSavedSuccessfully_WhenContactisSaved()
        {
            var contact = new ContactModel()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,

            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.Phone)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(true);


            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Contact saved successfully.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.Phone), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);


        }

        [Fact]
        public void AddContact_ReturnsSomethingWentWrong_WhenContactisNotSaved()
        {
            var contact = new ContactModel()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.Phone)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(false);


            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.Phone), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);


        }

        [Fact]
        public void AddContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var contact = new ContactModel()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.Phone)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact Already exists", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.Phone), Times.Once);

        }
        [Fact]
        public void AddContact_SetsDefaultImage_WhenImageIsNullOrEmpty()
        {
            // Arrange
            var contact = new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "Company",
                Image = null,
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            };

            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.ContactExists(It.IsAny<string>())).Returns(false);
            mockContactRepository.Setup(repo => repo.InsertContact(It.IsAny<ContactModel>())).Returns(true);

            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.AddContact(contact);

            // Assert
            Assert.True(response.Success);
            Assert.Equal("DefaultImage.jpg", contact.Image);
        }
        [Fact]
        public void AddContact_ReturnsError_WhenDateIsInvalid()
        {
            var contact = new ContactModel()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "joh@nexample.com",
                Phone = "1234899990",
                Image = "file1.txt",
                Gender = "Male",
                Company="CIVICA",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                BirthDate = DateTime.Now.AddDays(1)
            };


            var mockRepository = new Mock<IContactRepository>();

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Birthdate can't be greater than today's date", actual.Message);

        }
        [Fact]
        public void ModifyContact_ReturnsInvalid_WhenPhineInvalid()
        {
            var contactId = 1;
            var contact = new ContactModel()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "12367890",
                Image= "file1.txt",
                Gender = "Male",
                Company="Civica",
                Favourites = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contactId, contact.Phone)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contactId, contact.Phone), Times.Once);
        }
        [Fact]
        public void AddContact_ReturnsError_WhenEmailIsInvalid()
        {
            var contact = new ContactModel()
            {

                FirstName = "John",
                LastName = "Doe",
                Email = "johnexample.com",
                Phone = "1234899990",
                Image = "file1.txt",
                Gender = "Male",
                Company = "CIVICA",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Email should be in xyz@abc.com format only!", actual.Message);

        }

        [Fact]
        public void ModifyContact_ReturnsInvalid_WhenDateInvalid()
        {
            var contactId = 1;
            var contact = new ContactModel()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1236788890",
                Image = "file1.txt",
                Company="Civica",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                BirthDate = DateTime.Now.AddDays(1)
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contactId, contact.Phone)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contactId, contact.Phone), Times.Once);
        }
        [Fact]
        public void AddContact_ReturnsError_WhenPhoneIsInvalid()
        {
            var contact = new ContactModel()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "joh@nexample.com",
                Phone = "123480",
                Image = "file1.txt",
                Gender = "Male",
                Company = "CIVICA",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Number should include be 10 digits", actual.Message);

        }


        [Fact]
        public void ModifyContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var ContactId = 1;
            var contact = new ContactModel()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(ContactId, contact.Phone)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact already exists.", actual.Message);
            mockRepository.Verify(r => r.ContactExists(ContactId, contact.Phone), Times.Once);
        }
        [Fact]
        public void ModifyContact_ReturnsSomethingWentWrong_WhenContactNotFound()
        {
            var ContactId = 1;
            var existingContact = new ContactModel()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,

            };

            var updatedContact = new ContactModel()
            {
                ContactId = ContactId,
                FirstName = "C1"
            };


            var mockRepository = new Mock<IContactRepository>();
            //mockRepository.Setup(r => r.ContactExist(ContactId, updatedContact.Phone)).Returns(false);
            mockRepository.Setup(r => r.GetContact(updatedContact.ContactId)).Returns<IEnumerable<ContactModel>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(existingContact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            //mockRepository.Verify(r => r.ContactExist(ContactId, updatedContact.Phone), Times.Once);
            mockRepository.Verify(r => r.GetContact(ContactId), Times.Once);
        }

        [Fact]
        public void ModifyContact_ReturnsUpdatedSuccessfully_WhenContactModifiedSuccessfully()
        {

            //Arrange
            var existingContact = new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };

            var updatedContact = new ContactModel
            {
                ContactId = 1,
                FirstName = "Contact 1"
            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExists(updatedContact.ContactId, updatedContact.Phone)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.ContactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(true);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Contact updated successfully.", actual.Message);

            mockContactRepository.Verify(c => c.GetContact(updatedContact.ContactId), Times.Once);


            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }
        [Fact]
        public void ModifyContact_ReturnsError_WhenContactModifiedFails()
        {

            //Arrange
            var existingContact = new ContactModel
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
            };

            var updatedContact = new ContactModel
            {
                ContactId = 1,
                FirstName = "Contact 1"
            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExists(updatedContact.ContactId, updatedContact.Phone)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.ContactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(false);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            mockContactRepository.Verify(c => c.GetContact(updatedContact.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }

        [Fact]
        public void RemoveContact_ReturnsDeletedSuccessfully_WhenDeletedSuccessfully()
        {
            var ContactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(ContactId)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(ContactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Contact deleted successfully.", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(ContactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_SomethingWentWrong_WhenDeletionFailed()
        {
            var ContactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(ContactId)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(ContactId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong, please try after sometime.", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(ContactId), Times.Once);
        }
        [Fact]
        public void GetContactByLetter_WhenContactsFound_ReturnsSuccessResponseWithData()
        {
            // Arrange
            char letter = 'A';
            var contacts = new List<ContactModel>
        {
            new ContactModel { ContactId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "1234567890", Company = "ABC Inc.", Gender = "Male", Favourites = true, StateId = 1, BirthDate = new DateTime(1990, 1, 1), ImageByte = new byte[] { 0x00, 0x01 }, CountryId = 1, State = new State { StateId = 1, StateName = "State 1" }, Country = new Country { CountryId = 1, CountryName = "Country 1" } }
        };

            var contactRepositoryMock = new Mock<IContactRepository>();
            contactRepositoryMock.Setup(repo => repo.GetByLetter(letter)).Returns(contacts);
            var contactService = new ContactService(contactRepositoryMock.Object);

            // Act
            var result = contactService.GetContactByLetter(letter);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Count());
            Assert.Equal("John", result.Data.First().FirstName);
            // Add more assertions for other properties
        }
        [Fact]
        public void GetContactByLetter_WhenNoContactsFound_ReturnsErrorResponseWithMessage()
        {
            // Arrange
            char letter = 'Z';
            var contactRepositoryMock = new Mock<IContactRepository>();
            contactRepositoryMock.Setup(repo => repo.GetByLetter(letter)).Returns((List<ContactModel>)null);
            var contactService = new ContactService(contactRepositoryMock.Object);

            // Act
            var result = contactService.GetContactByLetter(letter);

            // Assert
            Assert.False(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(0, result.Data.Count());
            Assert.Equal("No record found.", result.Message);
        }
        [Fact]
        public void TotalContacts_ReturnsTotalNumberOfContacts()
        {
            // Arrange
            char? letter = 'A';
            string? search = "example";
            int expectedTotal = 10;

            var contactRepositoryMock = new Mock<IContactRepository>();
            contactRepositoryMock.Setup(repo => repo.TotalContacts(letter, search)).Returns(expectedTotal);
            var contactService = new ContactService(contactRepositoryMock.Object);

            // Act
            var result = contactService.TotalContacts(letter, search);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTotal, result.Data);
        }
        [Fact]
        public void GetPaginatedFavouriteContacts_WhenContactsFound_ReturnsSuccessResponseWithData()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            char? letter = 'A';
            var contacts = new List<ContactModel>
        {
            new ContactModel { ContactId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "1234567890", Image = "image.jpg", Gender = "Male", BirthDate = new DateTime(1990, 1, 1), ImageByte = new byte[] { 0x00, 0x01 }, Favourites = true, StateId = 1, CountryId = 1, State = new State { StateId = 1, StateName = "State 1" }, Country = new Country { CountryId = 1, CountryName = "Country 1" } }
        };

            var contactRepositoryMock = new Mock<IContactRepository>();
            contactRepositoryMock.Setup(repo => repo.GetPaginatedFavouriteContacts(page, pageSize, letter)).Returns(contacts);
            var contactService = new ContactService(contactRepositoryMock.Object);

            // Act
            var result = contactService.GetPaginatedFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Count());
            Assert.Equal("John", result.Data.First().FirstName);
            // Add more assertions for other properties
        }
        [Fact]
        public void TotalContactFavourite_ReturnsTotalNumberOfFavouriteContacts()
        {
            // Arrange
            char? letter = 'A';
            int expectedTotal = 10;

            var contactRepositoryMock = new Mock<IContactRepository>();
            contactRepositoryMock.Setup(repo => repo.TotalContactFavourite(letter)).Returns(expectedTotal);
            var contactService = new ContactService(contactRepositoryMock.Object);

            // Act
            var result = contactService.TotalContactFavourite(letter);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedTotal, result.Data);
            Assert.Equal("Paginated", result.Message);
        }

        [Fact]
        public void GetPaginatedFavouriteContacts_WhenNoContactsFound_ReturnsErrorResponseWithMessage()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            char? letter = 'A';
            List<ContactModel> contacts = new List<ContactModel>();

            var contactRepositoryMock = new Mock<IContactRepository>();
            contactRepositoryMock.Setup(repo => repo.GetPaginatedFavouriteContacts(page, pageSize, letter)).Returns(contacts);
            var contactService = new ContactService(contactRepositoryMock.Object);

            // Act
            var result = contactService.GetPaginatedFavouriteContacts(page, pageSize, letter);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("No record found", result.Message);
        }
        [Fact]
        public void GetFavouriteContacts_ReturnsContacts_WhenContactsExistForLetter()
        {
            // Arrange
            char letter = 'j';
            var contacts = new List<ContactModel>
            {new ContactModel
            {
               ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new ContactModel
            {
               ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Company = "company",
                Image = "file1.png",
                Gender = "Male",
                Favourites = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
            }.AsQueryable();
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.GetAllFavouriteContacts(It.IsAny<char>())).Returns(contacts);
            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.GetFavouriteContacts(letter);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);

        }
        [Fact]
        public void GetFavouriteContacts_ReturnsNoRecordsFound_WhenNoContactsExistForLetter()
        {
            // Arrange
            char letter = 'Z';
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(repo => repo.GetAllFavouriteContacts(It.IsAny<char>())).Returns((IQueryable<ContactModel>)null);
            var service = new ContactService(mockContactRepository.Object);

            // Act
            var response = service.GetFavouriteContacts(letter);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("No record found", response.Message);
        }
    }

}
