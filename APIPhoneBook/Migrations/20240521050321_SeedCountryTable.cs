using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhoneBook.Migrations
{
    public partial class SeedCountryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "Countries",
            columns: new[] { "CountryId", "CountryName" },
            values: new object[,]
            {
                    {1,"India" },
                    {2,"Australia" },
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
           table: "Countries",
           keyColumn: "CountryId",
           keyValue: new object[] { 1, 2 }
           );
        }
    }
}
