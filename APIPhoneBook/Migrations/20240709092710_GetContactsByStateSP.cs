using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhoneBook.Migrations
{
    public partial class GetContactsByStateSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE or alter PROCEDURE GetContactsByState
    @StateId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve contact details along with country and state names
    SELECT C.[ContactId]
          ,C.[FirstName]
          ,C.[LastName]
          ,C.[Email]
          ,C.[Phone]
          ,C.[Image]
          ,C.[Gender]
          ,C.[Favourites]
          ,CO.[CountryName] AS CountryName
          ,S.[StateName] AS StateName
          ,C.[birthDate]
          ,C.[ImageByte]
    FROM [Contact2DB].[dbo].[Contacts] C
    INNER JOIN [Contact2DB].[dbo].[States] S ON C.[StateId] = S.[StateId]
    INNER JOIN [Contact2DB].[dbo].[Countries] CO ON C.[CountryId] = CO.[CountryId]
    WHERE C.[StateId] = @StateId;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
