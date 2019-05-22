using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertDefinitions_AppUser_AppUserId",
                table: "AlertDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_AlertDefinitions_Stock_StockId",
                table: "AlertDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_AppUser_AppUserId",
                table: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stock",
                table: "Stock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.RenameTable(
                name: "Stock",
                newName: "Stocks");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "AppUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                column: "StockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertDefinitions_AppUsers_AppUserId",
                table: "AlertDefinitions",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertDefinitions_Stocks_StockId",
                table: "AlertDefinitions",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_AlertDefinitions_AppUsers_AppUserId",
                table: "AlertDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_AlertDefinitions_Stocks_StockId",
                table: "AlertDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_AppUsers_AppUserId",
                table: "UserPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.RenameTable(
                name: "Stocks",
                newName: "Stock");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "AppUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stock",
                table: "Stock",
                column: "StockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "AppUserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_AppUser_AppUserId",
                table: "UserPreferences",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
