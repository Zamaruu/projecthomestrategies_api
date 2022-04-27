using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace projecthomestrategies_api.Migrations
{
    public partial class MealPlannerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlannedMeals",
                columns: table => new
                {
                    PlannedMealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StartDay = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDay = table.Column<DateTime>(type: "datetime", nullable: false),
                    Color = table.Column<long>(type: "bigint", nullable: false),
                    RecipeId = table.Column<string>(type: "text", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    HouseholdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedMeals", x => x.PlannedMealId);
                    table.ForeignKey(
                        name: "FK_PlannedMeals_Households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "Households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlannedMeals_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMeals_CreatorUserId",
                table: "PlannedMeals",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMeals_HouseholdId",
                table: "PlannedMeals",
                column: "HouseholdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannedMeals");
        }
    }
}
