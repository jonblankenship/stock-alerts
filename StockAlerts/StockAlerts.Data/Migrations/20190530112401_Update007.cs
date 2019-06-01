using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComparisonOperator",
                table: "AlertDefinitions");

            migrationBuilder.DropColumn(
                name: "PriceLevel",
                table: "AlertDefinitions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AlertDefinitions");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AlertDefinitions",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "AlertCriteria",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Modified",
                table: "AlertCriteria",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AlertDefinitions");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "AlertCriteria");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "AlertCriteria");

            migrationBuilder.AddColumn<int>(
                name: "ComparisonOperator",
                table: "AlertDefinitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceLevel",
                table: "AlertDefinitions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AlertDefinitions",
                nullable: false,
                defaultValue: 0);
        }
    }
}
