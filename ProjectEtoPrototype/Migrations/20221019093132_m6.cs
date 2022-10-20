using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectEtoPrototype.Migrations
{
    public partial class m6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTasks_Users_UserId",
                table: "DailyTasks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DailyTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTasks_Users_UserId",
                table: "DailyTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTasks_Users_UserId",
                table: "DailyTasks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DailyTasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTasks_Users_UserId",
                table: "DailyTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
