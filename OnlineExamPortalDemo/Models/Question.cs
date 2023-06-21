using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamPortalDemo.Models;

public class Question : Pagination
{
	public int Id { get; set; }

	public string Question1 { get; set; }

	public string Option1 { get; set; }

	public string Option2 { get; set; }

	public string Option3 { get; set; }

	public string Option4 { get; set; }

	public string Correctans { get; set; }

	public string Filepath { get; set; }
	
	[NotMapped]

	public string ext { get; set; }

	[NotMapped]

	public List<IFormFile> files { get; set; }
	public int UserId { get; set; }
}
