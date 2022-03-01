using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace projecthomestrategies_api.Migrations
{
    public partial class BillImageMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Bills",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillImages",
                columns: table => new
                {
                    BillImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Image = table.Column<byte[]>(type: "varbinary(4000)", nullable: true),
                    BillId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillImages", x => x.BillImageId);
                    table.ForeignKey(
                        name: "FK_BillImages_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillImages_BillId",
                table: "BillImages",
                column: "BillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillImages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Bills");
        }
    }
}
