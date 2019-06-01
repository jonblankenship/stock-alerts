using Microsoft.EntityFrameworkCore.Migrations;

namespace StockAlerts.Data.Migrations
{
    public partial class Update008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_ParentCriteriaId",
                table: "AlertCriteria");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_ParentCriteriaId",
                table: "AlertCriteria",
                column: "ParentCriteriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AlertCriteria_ParentCriteriaId",
                table: "AlertCriteria");

            migrationBuilder.CreateIndex(
                name: "IX_AlertCriteria_ParentCriteriaId",
                table: "AlertCriteria",
                column: "ParentCriteriaId",
                unique: true,
                filter: "[ParentCriteriaId] IS NOT NULL");
        }
    }
}
