using Microsoft.AspNetCore.Identity;

namespace OnlineExamPortalDemo.Models;

public class AppUser : IdentityUser<int>
{
	public string Firstname { get; set; }
	public string Lastname { get; set; }
}
