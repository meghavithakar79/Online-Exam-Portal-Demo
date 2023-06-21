using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExamPortalDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answerchoice_question1_Question1Id",
                table: "answerchoice");

            migrationBuilder.DropTable(
                name: "question1");

            migrationBuilder.RenameColumn(
                name: "Question1Id",
                table: "answerchoice",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_answerchoice_Question1Id",
                table: "answerchoice",
                newName: "IX_answerchoice_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_answerchoice_question_QuestionId",
                table: "answerchoice",
                column: "QuestionId",
                principalTable: "question",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answerchoice_question_QuestionId",
                table: "answerchoice");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "answerchoice",
                newName: "Question1Id");

            migrationBuilder.RenameIndex(
                name: "IX_answerchoice_QuestionId",
                table: "answerchoice",
                newName: "IX_answerchoice_Question1Id");

            migrationBuilder.CreateTable(
                name: "question1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question1", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_answerchoice_question1_Question1Id",
                table: "answerchoice",
                column: "Question1Id",
                principalTable: "question1",
                principalColumn: "Id");
        }
    }
}
