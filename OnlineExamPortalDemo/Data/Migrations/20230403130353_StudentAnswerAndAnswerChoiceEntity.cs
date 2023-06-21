using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExamPortalDemo.Data.Migrations
{
    /// <inheritdoc />
    public partial class StudentAnswerAndAnswerChoiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "studentanswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerChoiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentanswer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "answerchoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    Question1Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answerchoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_answerchoice_question1_Question1Id",
                        column: x => x.Question1Id,
                        principalTable: "question1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_answerchoice_Question1Id",
                table: "answerchoice",
                column: "Question1Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answerchoice");

            migrationBuilder.DropTable(
                name: "studentanswer");

            migrationBuilder.DropTable(
                name: "question1");
        }
    }
}
