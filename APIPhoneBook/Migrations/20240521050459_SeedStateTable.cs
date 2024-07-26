using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhoneBook.Migrations
{
    public partial class SeedStateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "States",
            columns: new[] { "StateId", "StateName", "CountryId" },
            values: new object[,]
            {
                    {1,"Gujarat",1 },
                    {2,"Abu",1 },
                    {3,"Tamil Nadu",1 },
                    {4,"Hariyana",1 },
                    {5,"Rajasthan",1 },
                    {6,"Sweden",2 },
                    {7,"Maldives",2 },
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "Orders",
            keyColumn: "OrderId",
            keyValue: new object[] { 1, 2, 3, 4, 5, 6, 7 }
            );
        }
    }
}
