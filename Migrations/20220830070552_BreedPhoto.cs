using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogeFriendsAPI.Migrations
{
    public partial class BreedPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "BreedPhoto",
                table: "Breeds",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BreedPhotoFileName",
                table: "Breeds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreedPhoto",
                table: "Breeds");

            migrationBuilder.DropColumn(
                name: "BreedPhotoFileName",
                table: "Breeds");
        }
    }
}
