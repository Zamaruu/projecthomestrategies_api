using Microsoft.EntityFrameworkCore.Migrations;

namespace projecthomestrategies_api.Migrations
{
    public partial class BasicRecipeMigrtaion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BasicRecipeName",
                table: "PlannedMeals",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicRecipeName",
                table: "PlannedMeals");
        }
    }
}
