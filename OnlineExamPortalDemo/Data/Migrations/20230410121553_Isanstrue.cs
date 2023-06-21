using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExamPortalDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class Isanstrue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAnswer",
                table: "question");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnsTrue",
                table: "UserAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnsTrue",
                table: "UserAnswers");

            migrationBuilder.AddColumn<string>(
                name: "UserAnswer",
                table: "question",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
