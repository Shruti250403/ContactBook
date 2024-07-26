using APIPhoneBook.Dto;
using APIPhoneBook.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace APIPhoneBook.Data
{
    public class AppDbContext:DbContext,IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<ReportCountDto> ReportCounts { get; set; }
        public virtual IQueryable<ContactSPDto> GetContactsByBirthMonth(int month)
        {

            var monthParam = new SqlParameter("@Month", month);

            return Set<ContactSPDto>().FromSqlRaw("dbo.GetContactsByBirthMonth @Month", monthParam);

        }
        public virtual IQueryable<ContactSPDto> GetContactsByState(int stateId)
        {

            var stateid = new SqlParameter("@StateId", stateId);

            return Set<ContactSPDto>().FromSqlRaw("dbo.GetContactsByState @StateId", stateid);

        }
        public virtual int GetContactsCountByCountry(int countryid)
        {

            var countryId = new SqlParameter("@CountryId", countryid);

            var result =  Set<ReportCountDto>().FromSqlRaw("dbo.GetContactsCountByCountry @CountryId", countryId).AsEnumerable().FirstOrDefault();

            return result.TotalRecords;

        }
        public virtual int GetContactCountByGender(string gender)
        {

            var genderr = new SqlParameter("@Gender", gender);

            var result = Set<ReportCountDto>().FromSqlRaw("dbo.GetContactCountByGender @Gender", genderr).AsEnumerable().FirstOrDefault();

            return result.TotalRecords;

        }
        public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }
        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>()
             .HasOne(d => d.Country)
             .WithMany(p => p.States)
             .HasForeignKey(d => d.CountryId)
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("FK_State_Country");

            modelBuilder.Entity<ContactSPDto>().HasNoKey().ToView(null);
            modelBuilder.Entity<ReportCountDto>().HasNoKey().ToView(null);

        }

        public virtual IQueryable<ContactSPDto> ContactListSP(char? letter, string? search, int page, int pageSize, string sortOrder)
        {
            var letterParam = new SqlParameter("@letter", letter ?? (object)DBNull.Value);
            var searchParam = new SqlParameter("@Search", search ?? (object)DBNull.Value);
            var pageParam = new SqlParameter("@page", page);
            var pageSizeParam = new SqlParameter("@pageSize", pageSize);
            var sortOrderParam = new SqlParameter("@sortOrder", sortOrder);

            return Set<ContactSPDto>().FromSqlRaw("dbo.GetAllContactsSPWithCodeFirst @letter,@search, @page, @pageSize, @sortOrder", letterParam, searchParam, pageParam, pageSizeParam, sortOrderParam);
        }


    }

    }
