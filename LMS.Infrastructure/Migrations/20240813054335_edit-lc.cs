using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editlc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveClass_Teacher_CreatorId",
                table: "LiveClass");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "LiveClass",
                newName: "CourserId");

            migrationBuilder.RenameIndex(
                name: "IX_LiveClass_CreatorId",
                table: "LiveClass",
                newName: "IX_LiveClass_CourserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveClass_Course_CourserId",
                table: "LiveClass",
                column: "CourserId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveClass_Course_CourserId",
                table: "LiveClass");

            migrationBuilder.RenameColumn(
                name: "CourserId",
                table: "LiveClass",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_LiveClass_CourserId",
                table: "LiveClass",
                newName: "IX_LiveClass_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveClass_Teacher_CreatorId",
                table: "LiveClass",
                column: "CreatorId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
