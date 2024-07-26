using APIPhoneBook.Data.Contract;
using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using APIPhoneBook.Service.Contract;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace APIPhoneBook.Service.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetContact()
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contact = _contactRepository.GetAll();
            if (contact != null && contact.Any())
            {
                contact.Where(c => c.Image == string.Empty).ToList();
                List<ContactDto> contactDto = new List<ContactDto>();

                foreach (var contacts in contact)
                {
                    contactDto.Add(new ContactDto()
                    {
                        ContactId = contacts.ContactId,
                        FirstName = contacts.FirstName,
                        LastName = contacts.LastName,
                        Email = contacts.Email,
                        Phone = contacts.Phone,
                        Company = contacts.Company,
                        Image = contacts.Image,
                        Gender = contacts.Gender,
                        Favourites = contacts.Favourites,
                        StateId = contacts.StateId,
                        BirthDate = contacts.BirthDate,
                        ImageByte = contacts.ImageByte,

                        CountryId = contacts.CountryId,
                        State = new State()
                        {
                            StateId = contacts.State.StateId,
                            StateName = contacts.State.StateName,
                            CountryId = contacts.Country.CountryId,
                        },
                        Country = new Country()
                        {
                            CountryId = contacts.CountryId,
                            CountryName = contacts.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDto;
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }
            return response;
        }
        public ServiceResponse<IEnumerable<ContactSPDto>> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder)
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
            var contacts = _contactRepository.GetPaginatedContactsSP(page, pageSize, letter, search, sortOrder);

            if (contacts != null && contacts.Any())
            {
                response.Data = contacts;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }

        public ServiceResponse<string> AddContact(ContactModel contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contact.Phone))
            {
                response.Success = false;
                response.Message = "Contact Already exists";
                return response;
            }
            if (!ValidateEmail(contact.Email))
            {
                response.Success = false;
                response.Message = "Email should be in xyz@abc.com format only!";
                return response;
            }

            if (contact.Phone.Length != 10)
            {
                response.Success = false;
                response.Message = "Number should include be 10 digits";
                return response;
            }

            if (contact.BirthDate > DateTime.Now)
            {
                response.Success = false;
                response.Message = "Birthdate can't be greater than today's date";
                return response;
            }

            var fileName = string.Empty;

            if (string.IsNullOrEmpty(contact.Image))
            {
                contact.Image = "DefaultImage.jpg";
            }
            var result = _contactRepository.InsertContact(contact);
            if (result)
            {
                response.Message = "Contact saved successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, please try after sometime.";
            }
            return response;
        }

        public ServiceResponse<string> ModifyContact(ContactModel contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contact.ContactId, contact.Phone))
            {
                response.Success = false;
                response.Message = "Contact already exists.";
                return response;
            }
            var fileName = string.Empty;
            if (string.IsNullOrEmpty(contact.Image))
            {
                contact.Image = "DefaultImage.jpg";
            }
            var existingContact = _contactRepository.GetContact(contact.ContactId);
            var result = false;
            if (existingContact != null)
            {
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Phone = contact.Phone;
                existingContact.Email = contact.Email;
                existingContact.Company = contact.Company;
                existingContact.Image = contact.Image;
                existingContact.Gender = contact.Gender;
                existingContact.Favourites = contact.Favourites;
                existingContact.CountryId = contact.CountryId;
                existingContact.StateId = contact.StateId;
                existingContact.BirthDate = contact.BirthDate;
                existingContact.ImageByte = contact.ImageByte;
                result = _contactRepository.UpdateContact(existingContact);
            }

            if (result)
            {
                response.Message = "Contact updated successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, please try after sometime.";
            }
            return response;
        }

        public ServiceResponse<string> RemoveContact(int id)
        {
            var response = new ServiceResponse<string>();
            var result = _contactRepository.DeleteContact(id);
            if (result)
            {
                response.Message = "Contact deleted successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, please try after sometime.";
            }
            return response;
        }
        public ServiceResponse<ContactDto> GetContact(int contactId)
        {
            var response = new ServiceResponse<ContactDto>();
            var contact = _contactRepository.GetContact(contactId);
            if (contact == null)
            {
                response.Success = false;
                response.Data = new ContactDto();
                response.Message = "No records found.";
                return response;
            }
            var contactsDtos = new ContactDto()
            {

                ContactId = contact.ContactId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Phone = contact.Phone,
                Company = contact.Company,
                Image = contact.Image,
                Gender = contact.Gender,
                BirthDate = contact.BirthDate,
                ImageByte = contact.ImageByte,
                Favourites = contact.Favourites,
                StateId = contact.StateId,

                CountryId = contact.CountryId,
                State = new State()
                {
                    StateId = contact.State.StateId,
                    StateName = contact.State.StateName,
                    CountryId = contact.Country.CountryId,
                },
                Country = new Country()
                {
                    CountryId = contact.CountryId,
                    CountryName = contact.Country.CountryName,
                }
            };


            response.Data = contactsDtos;
            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetContactByLetter(char letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetByLetter(letter);
            if (contacts == null)
            {
                response.Success = false;
                response.Data = new List<ContactDto>();
                response.Message = "No record found.";
                return response;
            }
            List<ContactDto> contactsDtos = new List<ContactDto>();
            foreach (var contact in contacts.ToList())
            {
                contactsDtos.Add(
                    new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Company = contact.Company,
                        Image = contact.Image,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        StateId = contact.StateId,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        CountryId = contact.CountryId,
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.Country.CountryId,
                        },
                        Country = new Country()
                        {
                            CountryId = contact.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });

            }

            response.Data = contactsDtos;
            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, char? letter, string sortOrder, string? search)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedContacts(page, pageSize, letter, sortOrder,search);

            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Phone = contact.Phone,
                        Company = contact.Company,
                        Image = contact.Image,
                        Email = contact.Email,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.Country.CountryId,
                        },
                        Country = new Country()
                        {
                            CountryId = contact.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }


                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<int> TotalContacts(char? letter, string? search)
        {
            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalContacts(letter,search);

            response.Data = totalPositions;
            return response;
        }
        //public ServiceResponse<int> TotalContacts(char? letter, string? searchQuery)
        //{
        //    var response = new ServiceResponse<int>();
        //    int totalPositions = _contactRepository.TotalContacts(letter, searchQuery);

        //    response.Data = totalPositions;
        //    response.Success = true;
        //    response.Message = "Pagination successful";

        //    return response;
        //}
        public ServiceResponse<int> TotalFavContacts(char? letter)
        {
            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalFavContacts(letter);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Pagination successful";

            return response;
        }
        public ServiceResponse<int> TotalContacts()
        {
            var response = new ServiceResponse<int>();

            var result = _contactRepository.TotalContacts();
            response.Data = result;
            response.Success = true;

            return response;
        }
        [ExcludeFromCodeCoverage]
        public ServiceResponse<int> TotalContacts(char? letter, string? searchQuery, string sortOrder)
        {
            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalContacts(letter, searchQuery, sortOrder);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Pagination successful";

            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(int page, int pageSize, char? letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetFavouriteContacts(page, pageSize, letter);
            if (contacts != null && contacts.Any())
            {
                var favouriteContacts = contacts.Where(c => c.Favourites).ToList();
                if (favouriteContacts.Any())
                {
                    var contactDtoList = new List<ContactDto>();

                    foreach (var contact in favouriteContacts)
                    {
                        contactDtoList.Add(new ContactDto()
                        {
                            ContactId = contact.ContactId,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            Email = contact.Email,
                            Phone = contact.Phone,
                            Company = contact.Company,
                            Image = contact.Image,
                            Gender = contact.Gender,
                            BirthDate = contact.BirthDate,
                            ImageByte = contact.ImageByte,
                            Favourites = contact.Favourites,
                            StateId = contact.StateId,
                            CountryId = contact.CountryId,
                            State = new State()
                            {
                                StateId = contact.State.StateId,
                                StateName = contact.State.StateName,
                                CountryId = contact.State.CountryId,
                            },
                            Country = new Country()
                            {
                                CountryId = contact.CountryId,
                                CountryName = contact.Country.CountryName,
                            }
                        });
                    }
                    response.Data = contactDtoList;
                    response.Success = true;
                    return response;
                }
            }
            response.Success = false;
            response.Message = "No favourite contacts found!";
            return response;
        }
        public ServiceResponse<int> TotalContactFavourite(char? letter)
        {

            var response = new ServiceResponse<int>();
            int totalPositions = _contactRepository.TotalContactFavourite(letter);

            response.Data = totalPositions;
            response.Success = true;
            response.Message = "Paginated";

            return response;
        }
        [ExcludeFromCodeCoverage]
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedFavouriteContacts(int page, int pageSize)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedFavouriteContacts(page, pageSize);
            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts.ToList())
                {

                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Phone = contact.Phone,
                        Image = contact.Image,
                        Email = contact.Email,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        State = new State
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                        },
                        Country = new Country
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }

                    });

                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedFavouriteContacts(int page, int pageSize, char? letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedFavouriteContacts(page, pageSize, letter);
            if (contacts != null && contacts.Any())
            {
                List<ContactDto> contactDtos = new List<ContactDto>();
                foreach (var contact in contacts.ToList())
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Image = contact.Image,
                        Phone = contact.Phone,
                        Email = contact.Email,
                        Gender = contact.Gender,
                        BirthDate = contact.BirthDate,
                        ImageByte = contact.ImageByte,
                        Favourites = contact.Favourites,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        State = new State
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                        },
                        Country = new Country
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }

                    });

                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetFavouriteContacts(char? letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAllFavouriteContacts(letter);
            if (contacts != null && contacts.Any())
            {
                contacts.Where(c => c.Image == string.Empty).ToList();
                List<ContactDto> contactDtos = new List<ContactDto>();

                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Company = contact.Company,
                        Image = contact.Image,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                        State = new State()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new Country()
                        {
                            CountryId = contact.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }
        [ExcludeFromCodeCoverage]
        private bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);

        }
        public ServiceResponse<IEnumerable<ContactSPDto>> GetContactsByBirthMonth(int month)
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
            var contacts = _contactRepository.GetContactsByBirthMonth(month);
            if (contacts != null && contacts.Any())
            {
                contacts.Where(c => c.Image == string.Empty).ToList();
                List<ContactSPDto> contactDtos = new List<ContactSPDto>();

                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactSPDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Image = contact.Image,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        ImageByte = contact.ImageByte,
                        birthDate = contact.birthDate
                    });
                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }
        public ServiceResponse<IEnumerable<ContactSPDto>> GetContactsByState(int stateId)
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
            var contacts = _contactRepository.GetContactsByState(stateId);
            if (contacts != null && contacts.Any())
            {
                contacts.Where(c => c.Image == string.Empty).ToList();
                List<ContactSPDto> contactDtos = new List<ContactSPDto>();

                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactSPDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Image = contact.Image,
                        Gender = contact.Gender,
                        Favourites = contact.Favourites,
                        ImageByte = contact.ImageByte,
                        birthDate = contact.birthDate
                    });
                }
                response.Data = contactDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }
        public ServiceResponse<int> GetContactsCountByCountry(int countryId)
        {
            var response = new ServiceResponse<int>();
            var contacts = _contactRepository.GetContactsCountByCountry(countryId);
            if (contacts > 0)
                {
                    response.Success = true;
                    response.Data = contacts;

                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            return response;
        }
        public ServiceResponse<int> GetContactCountByGender(string gender)
        {
            var response = new ServiceResponse<int>();
            var contacts = _contactRepository.GetContactCountByGender(gender);
            if (contacts > 0)
            {
                response.Success = true;
                response.Data = contacts;

            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }



    }
}
