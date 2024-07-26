using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIPhoneBook.Migrations
{
    public partial class ImageUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageByte",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageByte",
                table: "Users");
        }
    }
}
