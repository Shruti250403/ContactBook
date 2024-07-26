using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhoneBook.Migrations
{
    public partial class GetContactCountByGenderSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE or alter PROCEDURE GetContactCountByGender
    @Gender CHAR(1)  -- Input parameter for Gender ('M' or 'F')
AS
BEGIN
    SET NOCOUNT ON;

    -- Query to fetch contact count based on specified gender
    SELECT
        COUNT(*) AS TotalRecords
    FROM
        [Contact2DB].[dbo].[Contacts]
    WHERE
        [Gender] = @Gender;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetContactCountByGender;");
        }
    }
}
