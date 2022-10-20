using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectEtoPrototype.Migrations
{
    public partial class m3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Preference_PreferenceId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DailyTask",
                newName: "TaskID");

            migrationBuilder.AlterColumn<string>(
                name: "PreferenceId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Preference_PreferenceId",
                table: "Users",
                column: "PreferenceId",
                principalTable: "Preference",
                principalColumn: "PreferenceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Preference_PreferenceId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "TaskID",
                table: "DailyTask",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "PreferenceId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Preference_PreferenceId",
                table: "Users",
                column: "PreferenceId",
                principalTable: "Preference",
                principalColumn: "PreferenceId");
        }
    }
}
