using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhoneBook.Migrations
{
    public partial class GetContactsCountByCountrySP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE or alter PROCEDURE GetContactsCountByCountry
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Return the total count of records for the specified country
    SELECT COUNT(*) AS TotalRecords
    FROM [Contact2DB].[dbo].[Contacts]
    WHERE [CountryId] = @CountryId;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetContactsCountByCountry;");
        }
    }
}
