using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineExamPortalDemo.Models;

namespace OnlineExamPortalDemo.Data;

public class ExamDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public ExamDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Question> question { get; set; }
	
	//public DbSet<StudentAnswer> studentanswer { get; set; }
	//public DbSet<AnswerChoice> answerchoice { get; set; }
	public DbSet<UserAnswer> UserAnswers { get; set; }
	public DbSet<QuizResult> QuizResults { get; set; }
	//public DbSet<Category> Categories { get; set; }
}