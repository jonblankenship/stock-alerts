using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "AlertDefinitions");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "AlertDefinitions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AlertDefinitions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "AlertDefinitions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "StockId",
                table: "AlertDefinitions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.AppUserId);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    StockId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    LastPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.StockId);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    UserPreferencesId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    AppUserId = table.Column<Guid>(nullable: false),
                    ShouldSendEmail = table.Column<bool>(nullable: false),
                    ShouldSendPush = table.Column<bool>(nullable: false),
                    ShouldSendSms = table.Column<bool>(nullable: false),
                    SmsNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.UserPreferencesId);
                    table.ForeignKey(
                        name: "FK_UserPreferences_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertDefinitions_AppUserId",
                table: "AlertDefinitions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertDefinitions_StockId",
                table: "AlertDefinitions",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_AppUserId",
                table: "UserPreferences",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertDefinitions_AppUser_AppUserId",
                table: "AlertDefinitions",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertDefinitions_Stock_StockId",
                table: "AlertDefinitions",
                column: "StockId",
                principalTable: "Stock",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertDefinitions_AppUser_AppUserId",
                table: "AlertDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_AlertDefinitions_Stock_StockId",
                table: "AlertDefinitions");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AlertDefinitions_AppUserId",
                table: "AlertDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_AlertDefinitions_StockId",
                table: "AlertDefinitions");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AlertDefinitions");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "AlertDefinitions");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "AlertDefinitions");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "AlertDefinitions");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "AlertDefinitions",
                nullable: true);
        }
    }
}
