using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria");

            migrationBuilder.AddColumn<Guid>(
                name: "RootCriteriaId",
                table: "AlertDefinitions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria");

            migrationBuilder.DropColumn(
                name: "RootCriteriaId",
                table: "AlertDefinitions");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId");
        }
    }
}
