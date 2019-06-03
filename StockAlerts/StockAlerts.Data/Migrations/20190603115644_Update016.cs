using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update016 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_AppUsers_UserId",
                table: "UserPreferences");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserPreferences",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                newName: "IX_UserPreferences_AppUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AppUsers",
                newName: "AppUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AppUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_AppUsers_AppUserId",
                table: "UserPreferences",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_AppUsers_AppUserId",
                table: "UserPreferences");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AppUsers");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "UserPreferences",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferences_AppUserId",
                table: "UserPreferences",
                newName: "IX_UserPreferences_UserId");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "AppUsers",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "AppUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "AppUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "AppUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "AppUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AppUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_AppUsers_UserId",
                table: "UserPreferences",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
