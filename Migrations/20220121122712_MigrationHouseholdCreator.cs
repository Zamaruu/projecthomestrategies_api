using Microsoft.EntityFrameworkCore.Migrations;

namespace projecthomestrategies_api.Migrations
{
    public partial class MigrationHouseholdCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Households",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Households_AdminId",
                table: "Households",
                column: "AdminId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Households_User_AdminId",
                table: "Households",
                column: "AdminId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Households_User_AdminId",
                table: "Households");

            migrationBuilder.DropIndex(
                name: "IX_Households_AdminId",
                table: "Households");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Households");
        }
    }
}
