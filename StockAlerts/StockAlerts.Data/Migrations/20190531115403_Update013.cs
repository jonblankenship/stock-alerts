using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertCriteria_AlertDefinitions_AlertDefinitionId1",
                table: "AlertCriteria");

            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_AlertDefinitionId1",
                table: "AlertCriteria");

            migrationBuilder.DropColumn(
                name: "AlertDefinitionId1",
                table: "AlertCriteria");

            migrationBuilder.AlterColumn<Guid>(
                name: "RootCriteriaId",
                table: "AlertDefinitions",
                nullable: true,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RootCriteriaId",
                table: "AlertDefinitions",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AlertDefinitionId1",
                table: "AlertCriteria",
                nullable: true);

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
    }
}
