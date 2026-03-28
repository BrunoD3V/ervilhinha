using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ervilhinha.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartPlanningSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BabyCostSimulators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Lifestyle = table.Column<int>(type: "int", nullable: false),
                    ExpectedBirthType = table.Column<int>(type: "int", nullable: false),
                    PregnancyWeeks = table.Column<int>(type: "int", nullable: false),
                    ExpectedDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasGovernmentSupport = table.Column<bool>(type: "bit", nullable: false),
                    WillBreastfeed = table.Column<bool>(type: "bit", nullable: false),
                    HasDonatedItems = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedPregnancyCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedBirthCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estimated0to6MonthsCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estimated6to12MonthsCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedTotalFirstYear = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedMonthlyImpact = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyCostSimulators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BabyShoppingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecommendedTiming = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsGift = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StoreLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyShoppingItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BabyTimelines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Phase = table.Column<int>(type: "int", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PregnancyWeek = table.Column<int>(type: "int", nullable: true),
                    BabyMonthAge = table.Column<int>(type: "int", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Recommendation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HasReminder = table.Column<bool>(type: "bit", nullable: false),
                    ReminderDaysBefore = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyTimelines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyBudgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TotalIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetBaby = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetHouse = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetFood = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetTransport = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetHealth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetLeisure = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetSavings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetOther = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualBaby = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualHouse = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualFood = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualTransport = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualHealth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualLeisure = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualOther = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ForecastBaby = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ForecastTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HealthStatus = table.Column<int>(type: "int", nullable: false),
                    AlertMessages = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyBudgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmartAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AssociatedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActionLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActionButtonText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDismissed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    RecurrencePattern = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartAlerts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BabyCostSimulators");

            migrationBuilder.DropTable(
                name: "BabyShoppingItems");

            migrationBuilder.DropTable(
                name: "BabyTimelines");

            migrationBuilder.DropTable(
                name: "FamilyBudgets");

            migrationBuilder.DropTable(
                name: "SmartAlerts");
        }
    }
}
