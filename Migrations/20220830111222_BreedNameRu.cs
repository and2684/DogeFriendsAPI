using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogeFriendsAPI.Migrations
{
    public partial class BreedNameRu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BreedNameRu",
                table: "Breeds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreedNameRu",
                table: "Breeds");
        }
    }
}
