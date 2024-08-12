using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editstudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Student");
        }
    }
}
