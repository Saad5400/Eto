using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectEtoPrototype.Migrations
{
    public partial class m7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentCalories",
                table: "Preferences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentCalories",
                table: "Preferences");
        }
    }
}
