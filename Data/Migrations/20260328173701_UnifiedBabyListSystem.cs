using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ervilhinha.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnifiedBabyListSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BabyLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BabyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    ShareCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BabyListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyListId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "int", nullable: false),
                    AcquiredQuantity = table.Column<int>(type: "int", nullable: false),
                    RecommendedTiming = table.Column<int>(type: "int", nullable: true),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsGift = table.Column<bool>(type: "bit", nullable: false),
                    ProductUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BabyListItems_BabyLists_BabyListId",
                        column: x => x.BabyListId,
                        principalTable: "BabyLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BabyListManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyListId = table.Column<int>(type: "int", nullable: false),
                    ManagerEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CanManageManagers = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyListManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BabyListManagers_BabyLists_BabyListId",
                        column: x => x.BabyListId,
                        principalTable: "BabyLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BabyListShares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyListId = table.Column<int>(type: "int", nullable: false),
                    SharedWithEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SharedWithName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InviteMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SharedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SharedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Permission = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyListShares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BabyListShares_BabyLists_BabyListId",
                        column: x => x.BabyListId,
                        principalTable: "BabyLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BabyListReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyListItemId = table.Column<int>(type: "int", nullable: false),
                    ReservedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReservedByName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ReservedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyListReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BabyListReservations_BabyListItems_BabyListItemId",
                        column: x => x.BabyListItemId,
                        principalTable: "BabyListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BabyListItems_BabyListId",
                table: "BabyListItems",
                column: "BabyListId");

            migrationBuilder.CreateIndex(
                name: "IX_BabyListManagers_BabyListId",
                table: "BabyListManagers",
                column: "BabyListId");

            migrationBuilder.CreateIndex(
                name: "IX_BabyListManagers_ManagerEmail",
                table: "BabyListManagers",
                column: "ManagerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_BabyListReservations_BabyListItemId",
                table: "BabyListReservations",
                column: "BabyListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BabyListReservations_ReservedBy",
                table: "BabyListReservations",
                column: "ReservedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BabyLists_ShareCode",
                table: "BabyLists",
                column: "ShareCode",
                unique: true,
                filter: "[ShareCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BabyLists_UserId",
                table: "BabyLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BabyListShares_BabyListId",
                table: "BabyListShares",
                column: "BabyListId");

            migrationBuilder.CreateIndex(
                name: "IX_BabyListShares_SharedWithEmail",
                table: "BabyListShares",
                column: "SharedWithEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BabyListManagers");

            migrationBuilder.DropTable(
                name: "BabyListReservations");

            migrationBuilder.DropTable(
                name: "BabyListShares");

            migrationBuilder.DropTable(
                name: "BabyListItems");

            migrationBuilder.DropTable(
                name: "BabyLists");
        }
    }
}
