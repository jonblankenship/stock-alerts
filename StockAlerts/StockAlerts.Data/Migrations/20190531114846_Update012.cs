using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertDefinitions_AlertCriteria_RootCriteriaId",
                table: "AlertDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_AlertDefinitions_RootCriteriaId",
                table: "AlertDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria");

            migrationBuilder.AddColumn<Guid>(
                name: "AlertDefinitionId1",
                table: "AlertCriteria",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId",
                unique: true,
                filter: "[AlertDefinitionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_AlertDefinitionId1",
                table: "AlertCriteria",
                column: "AlertDefinitionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertCriteria_AlertDefinitions_AlertDefinitionId1",
                table: "AlertCriteria",
                column: "AlertDefinitionId1",
                principalTable: "AlertDefinitions",
                principalColumn: "AlertDefinitionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertCriteria_AlertDefinitions_AlertDefinitionId1",
                table: "AlertCriteria");

            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria");

            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_AlertDefinitionId1",
                table: "AlertCriteria");

            migrationBuilder.DropColumn(
                name: "AlertDefinitionId1",
                table: "AlertCriteria");

            migrationBuilder.CreateIndex(
                name: "IX_AlertDefinitions_RootCriteriaId",
                table: "AlertDefinitions",
                column: "RootCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertDefinitions_AlertCriteria_RootCriteriaId",
                table: "AlertDefinitions",
                column: "RootCriteriaId",
                principalTable: "AlertCriteria",
                principalColumn: "AlertCriteriaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
