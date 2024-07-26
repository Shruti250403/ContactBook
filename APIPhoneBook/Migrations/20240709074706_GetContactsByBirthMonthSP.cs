using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhoneBook.Migrations
{
    public partial class GetContactsByBirthMonthSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE or alter PROCEDURE GetContactsByBirthMonth
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Query to fetch contacts based on birth month with country and state names
    SELECT C.[ContactId]
          ,C.[FirstName]
          ,C.[LastName]
          ,C.[Email]
          ,C.[Phone]
          ,C.[Image]
          ,C.[Gender]
          ,CO.[CountryName] AS CountryName
          ,S.[StateName] AS StateName
          ,C.[Favourites]
          ,C.[BirthDate]
          ,C.[ImageByte]
    FROM [Contact2DB].[dbo].[Contacts] C
    LEFT JOIN [Contact2DB].[dbo].[Countries] CO ON C.[CountryId] = CO.[CountryId]
    LEFT JOIN [Contact2DB].[dbo].[States] S ON C.[StateId] = S.[StateId]
    WHERE MONTH(C.[BirthDate]) = @Month;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetContactsByBirthMonth;");
        }
    }
}
