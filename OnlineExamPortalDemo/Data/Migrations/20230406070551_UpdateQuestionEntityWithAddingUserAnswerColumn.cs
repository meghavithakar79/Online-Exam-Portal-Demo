using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExamPortalDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestionEntityWithAddingUserAnswerColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answerchoice_question_QuestionId",
                table: "answerchoice");

            migrationBuilder.DropIndex(
                name: "IX_answerchoice_QuestionId",
                table: "answerchoice");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "answerchoice");

            migrationBuilder.AddColumn<string>(
                name: "UserAnswer",
                table: "question",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAnswer",
                table: "question");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "answerchoice",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_answerchoice_QuestionId",
                table: "answerchoice",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_answerchoice_question_QuestionId",
                table: "answerchoice",
                column: "QuestionId",
                principalTable: "question",
                principalColumn: "Id");
        }
    }
}
