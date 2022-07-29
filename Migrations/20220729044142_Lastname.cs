using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogeFriendsAPI.Migrations
{
    public partial class Lastname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondaryName",
                table: "Users",
                newName: "LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "SecondaryName");
        }
    }
}
