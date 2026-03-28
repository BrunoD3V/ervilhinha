using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ervilhinha.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSmartPlanningPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SmartAlerts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FamilyBudgets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BabyTimelines",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BabyShoppingItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BabyCostSimulators",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SmartAlerts_UserId_IsRead_CreatedDate",
                table: "SmartAlerts",
                columns: new[] { "UserId", "IsRead", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyBudgets_UserId_Year_Month",
                table: "FamilyBudgets",
                columns: new[] { "UserId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BabyTimelines_UserId_EventDate",
                table: "BabyTimelines",
                columns: new[] { "UserId", "EventDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BabyShoppingItems_UserId",
                table: "BabyShoppingItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BabyCostSimulators_UserId",
                table: "BabyCostSimulators",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SmartAlerts_UserId_IsRead_CreatedDate",
                table: "SmartAlerts");

            migrationBuilder.DropIndex(
                name: "IX_FamilyBudgets_UserId_Year_Month",
                table: "FamilyBudgets");

            migrationBuilder.DropIndex(
                name: "IX_BabyTimelines_UserId_EventDate",
                table: "BabyTimelines");

            migrationBuilder.DropIndex(
                name: "IX_BabyShoppingItems_UserId",
                table: "BabyShoppingItems");

            migrationBuilder.DropIndex(
                name: "IX_BabyCostSimulators_UserId",
                table: "BabyCostSimulators");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SmartAlerts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FamilyBudgets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BabyTimelines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BabyShoppingItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BabyCostSimulators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
