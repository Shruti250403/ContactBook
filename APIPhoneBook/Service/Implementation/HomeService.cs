using APIPhoneBook.Data.Contract;
using APIPhoneBook.Dto;
using APIPhoneBook.Service.Contract;
using System.Diagnostics.CodeAnalysis;

namespace APIPhoneBook.Service.Implementation
{
    [ExcludeFromCodeCoverage]
    public class HomeService:IHomeService
    {
        private readonly IHomeRepository _homeRepository;


        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetContacts(char letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _homeRepository.GetAll(letter);
            if (contacts == null)
            {
                response.Success = false;
                response.Data = new List<ContactDto>();
                response.Message = "No record found.";
                return response;
            }
            List<ContactDto> ContactDtos = new List<ContactDto>();
            foreach (var contact in contacts.ToList())
            {
                ContactDtos.Add(
                    new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Company = contact.Company,

                    });

            }

            response.Data = ContactDtos;
            return response;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetAllContacts()
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _homeRepository.GetAllContacts();
            if (contacts == null)
            {
                response.Success = false;
                response.Data = new List<ContactDto>();
                response.Message = "No record found.";
                return response;
            }
            List<ContactDto> ContactDtos = new List<ContactDto>();
            foreach (var contact in contacts.ToList())
            {
                ContactDtos.Add(
                    new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        Company = contact.Company,

                    });

            }

            response.Data = ContactDtos;
            return response;
        }

        public ServiceResponse<ContactDto> GetContact(int contactId)
        {
            var response = new ServiceResponse<ContactDto>();
            var contact = _homeRepository.GetContact(contactId);
            if (contact == null)
            {
                response.Success = false;
                response.Data = new ContactDto();
                response.Message = "No records found.";
                return response;
            }
            var ContactDtos = new ContactDto()
            {

                ContactId = contact.ContactId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Phone = contact.Phone,
                Company = contact.Company,

            };


            response.Data = ContactDtos;
            return response;
        }
    }
}
