using APIPhoneBook.Data.Contract;
using APIPhoneBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace APIPhoneBook.Data.Implementation
{
    public class StateRepository:IStateRepository
    {
        private readonly IAppDbContext _appDbContext;

        public StateRepository(IAppDbContext context)
        {
            _appDbContext = context;
        }
        public IEnumerable<State> GetAll()
        {
            List<State> standards = _appDbContext.States.Include(c => c.Country).ToList();
            return standards;
        }
        public State GetStateById(int id)
        {
            var student = _appDbContext.States.Include(c => c.Country).FirstOrDefault(c => c.StateId == id);
            return student;
        }
        [ExcludeFromCodeCoverage]
        public int TotalStates()
        {
            return _appDbContext.States.Include(c => c.Country).Count();
        }
        [ExcludeFromCodeCoverage]
        public IEnumerable<State> GetAllStates()
        {
            var students = new List<State>();
            students = _appDbContext.States.Include(c => c.Country).ToList();
            return students;
        }

        public List<State> GetAllStateByCountryId(int countryId)
        {
            List<State> countries = _appDbContext.States.Include(p => p.Country).Where(c => c.CountryId == countryId).ToList();
            return countries;
        }
    }
}
