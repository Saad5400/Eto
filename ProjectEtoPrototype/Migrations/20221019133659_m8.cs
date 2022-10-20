using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectEtoPrototype.Migrations
{
    public partial class m8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxCalories",
                table: "Preferences");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Preferences",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Preferences");

            migrationBuilder.AddColumn<int>(
                name: "MaxCalories",
                table: "Preferences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
