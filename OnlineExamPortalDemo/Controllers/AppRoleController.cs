using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OnlineExamPortalDemo.Models;

namespace OnlineExamPortalDemo.Controllers
{
	//[Authorize(Roles ="admin")]
	public class AppRoleController : Controller
	{
		private readonly RoleManager<AppRole> _roleManager;
		public AppRoleController(RoleManager<AppRole> roleManager)
		{
			_roleManager = roleManager;
		}
		public IActionResult Index()
		{
			var roles = _roleManager.Roles;
			return View(roles);
		}
		[HttpGet]
		public async Task<IActionResult> create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> create(AppRole model)
		{
			AppRole user = new AppRole() {  Name = model.Name };

			if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
			{
				await _roleManager.CreateAsync(user);
			}
			return RedirectToAction("Index");
		}
	}
}
