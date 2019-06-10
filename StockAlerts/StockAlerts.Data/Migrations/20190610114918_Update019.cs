using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Stocks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Stocks");
        }
    }
}
