using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertCriteria_AlertDefinitions_AlertDefinitionId",
                table: "AlertCriteria");

            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria");

            migrationBuilder.AlterColumn<Guid>(
                name: "AlertDefinitionId",
                table: "AlertCriteria",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_AlertDefinitions_RootCriteriaId",
                table: "AlertDefinitions",
                column: "RootCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertCriteria_AlertDefinitions_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId",
                principalTable: "AlertDefinitions",
                principalColumn: "AlertDefinitionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertDefinitions_AlertCriteria_RootCriteriaId",
                table: "AlertDefinitions",
                column: "RootCriteriaId",
                principalTable: "AlertCriteria",
                principalColumn: "AlertCriteriaId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertCriteria_AlertDefinitions_AlertDefinitionId",
                table: "AlertCriteria");

            migrationBuilder.DropForeignKey(
                name: "FK_AlertDefinitions_AlertCriteria_RootCriteriaId",
                table: "AlertDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_AlertDefinitions_RootCriteriaId",
                table: "AlertDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria");

            migrationBuilder.AlterColumn<Guid>(
                name: "AlertDefinitionId",
                table: "AlertCriteria",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertCriteria_AlertDefinitions_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId",
                principalTable: "AlertDefinitions",
                principalColumn: "AlertDefinitionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
