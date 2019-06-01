using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertCriteria_AlertCriteria_RootCriteriaId",
                table: "AlertCriteria");

            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_RootCriteriaId",
                table: "AlertCriteria");

            migrationBuilder.DropColumn(
                name: "RootCriteriaId",
                table: "AlertCriteria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RootCriteriaId",
                table: "AlertCriteria",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_RootCriteriaId",
                table: "AlertCriteria",
                column: "RootCriteriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertCriteria_AlertCriteria_RootCriteriaId",
                table: "AlertCriteria",
                column: "RootCriteriaId",
                principalTable: "AlertCriteria",
                principalColumn: "AlertCriteriaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
