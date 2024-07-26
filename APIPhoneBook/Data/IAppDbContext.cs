using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace APIPhoneBook.Data
{
    public interface IAppDbContext:IDbContext
    {
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public IQueryable<ContactSPDto> ContactListSP(char? letter, string? search, int page, int pageSize, string sortOrder);
        public IQueryable<ContactSPDto> GetContactsByBirthMonth(int month);
        public int GetContactsCountByCountry(int countryid);
        public DbSet<ReportCountDto> ReportCounts { get; set; }
        public IQueryable<ContactSPDto> GetContactsByState(int stateId);
        public int GetContactCountByGender(string gender);

    }
}
