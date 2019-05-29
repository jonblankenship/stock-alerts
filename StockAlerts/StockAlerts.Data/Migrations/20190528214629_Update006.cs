using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "UserPreferences",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceLevel",
                table: "AlertDefinitions",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastSent",
                table: "AlertDefinitions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AlertCriteria",
                columns: table => new
                {
                    AlertCriteriaId = table.Column<Guid>(nullable: false),
                    AlertDefinitionId = table.Column<Guid>(nullable: false),
                    ParentCriteriaId = table.Column<Guid>(nullable: true),
                    RootCriteriaId = table.Column<Guid>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Operator = table.Column<int>(nullable: false),
                    Level = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertCriteria", x => x.AlertCriteriaId);
                    table.ForeignKey(
                        name: "FK_AlertCriteria_AlertDefinitions_AlertDefinitionId",
                        column: x => x.AlertDefinitionId,
                        principalTable: "AlertDefinitions",
                        principalColumn: "AlertDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertCriteria_AlertCriteria_ParentCriteriaId",
                        column: x => x.ParentCriteriaId,
                        principalTable: "AlertCriteria",
                        principalColumn: "AlertCriteriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlertCriteria_AlertCriteria_RootCriteriaId",
                        column: x => x.RootCriteriaId,
                        principalTable: "AlertCriteria",
                        principalColumn: "AlertCriteriaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlertTriggerHistory",
                columns: table => new
                {
                    AlertTriggerHistoryId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Modified = table.Column<DateTimeOffset>(nullable: false),
                    AlertDefinitionId = table.Column<Guid>(nullable: false),
                    TimeTriggered = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertTriggerHistory", x => x.AlertTriggerHistoryId);
                    table.ForeignKey(
                        name: "FK_AlertTriggerHistory_AlertDefinitions_AlertDefinitionId",
                        column: x => x.AlertDefinitionId,
                        principalTable: "AlertDefinitions",
                        principalColumn: "AlertDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_AlertDefinitionId",
                table: "AlertCriteria",
                column: "AlertDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_ParentCriteriaId",
                table: "AlertCriteria",
                column: "ParentCriteriaId",
                unique: true,
                filter: "[ParentCriteriaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_RootCriteriaId",
                table: "AlertCriteria",
                column: "RootCriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlertTriggerHistory_AlertDefinitionId",
                table: "AlertTriggerHistory",
                column: "AlertDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertCriteria");

            migrationBuilder.DropTable(
                name: "AlertTriggerHistory");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "UserPreferences");

            migrationBuilder.DropColumn(
                name: "LastSent",
                table: "AlertDefinitions");

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceLevel",
                table: "AlertDefinitions",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
