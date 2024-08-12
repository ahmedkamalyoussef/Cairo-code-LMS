using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editliveclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "LiveClass",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LiveClass_CreatorId",
                table: "LiveClass",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveClass_Teacher_CreatorId",
                table: "LiveClass",
                column: "CreatorId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveClass_Teacher_CreatorId",
                table: "LiveClass");

            migrationBuilder.DropIndex(
                name: "IX_LiveClass_CreatorId",
                table: "LiveClass");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "LiveClass");
        }
    }
}
