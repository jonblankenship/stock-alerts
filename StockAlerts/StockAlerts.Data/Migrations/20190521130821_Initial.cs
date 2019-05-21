using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertDefinitions",
                columns: table => new
                {
                    AlertDefinitionId = table.Column<Guid>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    PriceLevel = table.Column<decimal>(nullable: false),
                    ComparisonOperator = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertDefinitions", x => x.AlertDefinitionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertDefinitions");
        }
    }
}
