using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectEtoPrototype.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTasks_Users_UserId",
                table: "DailyTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Preferences_PreferenceId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyTasks",
                table: "DailyTasks");

            migrationBuilder.RenameTable(
                name: "Preferences",
                newName: "Preference");

            migrationBuilder.RenameTable(
                name: "DailyTasks",
                newName: "DailyTask");

            migrationBuilder.RenameIndex(
                name: "IX_DailyTasks_UserId",
                table: "DailyTask",
                newName: "IX_DailyTask_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preference",
                table: "Preference",
                column: "PreferenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyTask",
                table: "DailyTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTask_Users_UserId",
                table: "DailyTask",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Preference_PreferenceId",
                table: "Users",
                column: "PreferenceId",
                principalTable: "Preference",
                principalColumn: "PreferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTask_Users_UserId",
                table: "DailyTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Preference_PreferenceId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Preference",
                table: "Preference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyTask",
                table: "DailyTask");

            migrationBuilder.RenameTable(
                name: "Preference",
                newName: "Preferences");

            migrationBuilder.RenameTable(
                name: "DailyTask",
                newName: "DailyTasks");

            migrationBuilder.RenameIndex(
                name: "IX_DailyTask_UserId",
                table: "DailyTasks",
                newName: "IX_DailyTasks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences",
                column: "PreferenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyTasks",
                table: "DailyTasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTasks_Users_UserId",
                table: "DailyTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Preferences_PreferenceId",
                table: "Users",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "PreferenceId");
        }
    }
}
