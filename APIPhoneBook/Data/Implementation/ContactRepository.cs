using APIPhoneBook.Data.Contract;
using APIPhoneBook.Data;
using Microsoft.EntityFrameworkCore;
using APIPhoneBook.Models;
using System.Diagnostics.CodeAnalysis;
using APIPhoneBook.Dto;

namespace ApiApplicationCore.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly IAppDbContext _AppDBContext;

        public ContactRepository(IAppDbContext AppDBContext)
        {
            _AppDBContext = AppDBContext;
        }
        public IEnumerable<ContactModel> GetAll()
        {
            List<ContactModel> contacts = _AppDBContext.Contacts.Include(c => c.State).Include(c => c.Country).ToList();
            return contacts;
        }
        public IEnumerable<ContactSPDto> GetPaginatedContactsSP(int page, int pageSize, char? letter, string? search, string sortOrder)
        {
            var result = _AppDBContext.ContactListSP(letter, search, page, pageSize, sortOrder);
            return result;
        }
        public IEnumerable<ContactModel> GetByLetter(char letter)
        {
            var contacts = _AppDBContext.Contacts.Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
            if (contacts != null && !contacts.Any())
            {
                return contacts;
            }
            return new List<ContactModel>();
        }
        [ExcludeFromCodeCoverage]
        public IEnumerable<ContactModel> GetPaginatedFavouriteContacts(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            return _AppDBContext.Contacts
                .Include(c => c.State)
                .Include(c => c.Country)
                .OrderBy(c => c.ContactId)
                .Where(c => c.Favourites == true)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        [ExcludeFromCodeCoverage]
        public IEnumerable<ContactModel> GetPaginatedFavouriteContacts(int page, int pageSize, char? letter)
        {
            int skip = (page - 1) * pageSize;
            return _AppDBContext.Contacts
                 .Include(c => c.State)
                 .Include(c => c.Country)
                .Where(c => c.FirstName.StartsWith(letter.ToString()))
                .Where(c => c.Favourites == true)
                .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        [ExcludeFromCodeCoverage]
        public int TotalContactFavourite(char? letter)
        {
            IQueryable<ContactModel> query = _AppDBContext.Contacts.Where(c => c.Favourites == true);

            if (letter.HasValue)
            {
                query = query
                    .Where(c => c.FirstName.StartsWith(letter.ToString()));
            }
            return query.Count();
        }
        //public IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, string sortOrder)
        //{
        //    int skip = (page - 1) * pageSize;
        //    IQueryable<ContactModel> query = _AppDBContext.Contacts
        //        .Include(c => c.State)
        //        .Include(c => c.Country);
        //    switch (sortOrder.ToLower())
        //    {
        //        case "asc":
        //            query = query.OrderBy(c => c.FirstName);
        //            break;
        //        case "desc":
        //            query = query.OrderByDescending(c => c.FirstName);
        //            break;
        //        default:
        //            throw new ArgumentException("Invalid sorting order");
        //    }
        //    return query

        //        .Skip(skip)
        //        .Take(pageSize)
        //        .ToList();
        //}
        [ExcludeFromCodeCoverage]
        public int TotalContacts()
        {
            return _AppDBContext.Contacts.Count();
        }
        public int TotalContacts(char? letter,string? searchQuery)
        {
            IQueryable<ContactModel> query = _AppDBContext.Contacts;

            if (letter!=null)
            {
                string letterString = letter.ToString();
                query = query.Where(c => c.FirstName.StartsWith(letterString));
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.FirstName.Contains(searchQuery) || c.LastName.Contains(searchQuery));
            }
            return query.Count();
        }
        public int TotalFavContacts(char? letter)
        {
            IQueryable<ContactModel> query = _AppDBContext.Contacts.Where(c => c.Favourites);

            if (letter.HasValue)
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.ToString()));
            }
            return query.Count();
        }

        //public int TotalContacts(char? letter)
        //{
        //    IQueryable<ContactModel> query = _AppDBContext.Contacts;

        //    if (letter.HasValue)
        //    {
        //        query = query.Where(c => c.FirstName.StartsWith(letter.ToString()));
        //    }
        //    return query.Count();
        //}
        [ExcludeFromCodeCoverage]
        public int TotalContacts(char? letter, string? searchQuery, string sortOrder)
        {
            IQueryable<ContactModel> query = _AppDBContext.Contacts.Include(c => c.State)
                .Include(c => c.Country);
            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName);
                    break;
                default:
                    throw new ArgumentException("Invalid sorting order");
            }
            // Trim the search query to remove etra spaces
            searchQuery = searchQuery?.Trim();

            if (letter.HasValue)
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.Value.ToString()));
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.FirstName.Contains(searchQuery) || c.Phone.Contains(searchQuery));
            }

            return query.Count();
        }

        //public IEnumerable<ContactModel> GetPaginatedContact(int page, int pageSize)
        //{
        //    int skip = (page - 1) * pageSize;
        //    return _AppDBContext.Contacts
        //        .OrderBy(c => c.ContactId)
        //        .Skip(skip)
        //        .Take(pageSize)
        //        .ToList();
        //}
        
        public IEnumerable<ContactModel> GetPaginatedContacts(int page, int pageSize, char? letter, string sortOrder,string? search)
        {
            int skip = (page - 1) * pageSize;
            IQueryable<ContactModel> query = _AppDBContext.Contacts
               .Include(c => c.State)
               .Include(c => c.Country);
            if (letter != null)
            {
                string letterString = letter.ToString();
                query = query.Where(c => c.FirstName.StartsWith(letterString));
            }
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
            }


            switch (sortOrder.ToLower())
            {
                case "asc":
                    query = query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
                    break;
                case "desc":
                    query = query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName);
                    break;
                default:
                    throw new ArgumentException("Invalid sorting order");
            }

            return query
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public ContactModel? GetContact(int id)
        {
            var contact = _AppDBContext.Contacts.Include(c => c.State).Include(c => c.Country).FirstOrDefault(c => c.ContactId == id);
            return contact;
        }
        public bool InsertContact(ContactModel contact)
        {
            var result = false;
            if (contact != null)
            {
                _AppDBContext.Contacts.Add(contact);
                _AppDBContext.SaveChanges();
                result = true;
            }

            return result;
        }

        public bool UpdateContact(ContactModel contact)
        {
            var result = false;
            if (contact != null)
            {
                _AppDBContext.Contacts.Update(contact);
                _AppDBContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool DeleteContact(int id)
        {
            var result = false;
            var contact = _AppDBContext.Contacts.Find(id);
            if (contact != null)
            {
                _AppDBContext.Contacts.Remove(contact);
                _AppDBContext.SaveChanges();
                result = true;
            }

            return result;
        }
        public IEnumerable<ContactModel> GetFavouriteContacts(int page, int pageSize, char? letter)
        {
            int skip = (page - 1) * pageSize;
            return _AppDBContext.Contacts
                .Include(c => c.Country)
                .Include(c => c.State)
                .Where(c => c.Favourites)
               .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        public bool ContactExists(string contactNumber)
        {
            var contact = _AppDBContext.Contacts.FirstOrDefault(c => c.Phone == contactNumber);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContactExists(int ContactId, string contactNumber)
        {
            var contact = _AppDBContext.Contacts.FirstOrDefault(c => c.ContactId != ContactId && c.Phone == contactNumber);
            if (contact != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<ContactModel> GetAllFavouriteContacts(char? letter)
        {

            List<ContactModel> contacts = _AppDBContext.Contacts
                .Include(c => c.Country)
                .Include(c => c.State)
                .Where(c => c.Favourites)
                .Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
            return contacts;
        }
        public IEnumerable<ContactSPDto> GetContactsByBirthMonth(int month)
        {
            var totals = _AppDBContext.GetContactsByBirthMonth(month);

            return totals;
        }
        public int GetContactsCountByCountry(int countryId)
        {
            var totals = _AppDBContext.GetContactsCountByCountry(countryId);

            return totals;
        }
        public IEnumerable<ContactSPDto> GetContactsByState(int stateId)
        {
            var totals = _AppDBContext.GetContactsByState(stateId);

            return totals;
        }
        public int GetContactCountByGender(string gender)
        {
            var totals = _AppDBContext.GetContactCountByGender(gender);

            return totals;
        }
    }
}
