using APIPhoneBook.Data.Contract;
using APIPhoneBook.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace APIPhoneBook.Data.Implementation
{
    public class CountryRepository:ICountryRepository
    {
        private readonly IAppDbContext _appDbContext;

        public CountryRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Country> GetAll()
        {
            List<Country> stages = _appDbContext.Countries.ToList();
            return stages;
        }
        public Country GetCountryById(int id)
        {
            var student = _appDbContext.Countries.FirstOrDefault(c => c.CountryId == id);
            return student;
        }
        [ExcludeFromCodeCoverage]
        public int TotalCountries()
        {
            return _appDbContext.Countries.Count();
        }
    }
}
